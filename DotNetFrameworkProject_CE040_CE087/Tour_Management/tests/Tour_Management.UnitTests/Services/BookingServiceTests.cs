using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Domain.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;
using Xunit;

namespace Tour_Management.UnitTests.Services;

/// <summary>Unit tests for BookingService.</summary>
public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _mockRepository = new Mock<IBookingRepository>();
        _mockLogger = new Mock<ILogger<BookingService>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new BookingService(_mockRepository.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBookings()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Goa", Email = "test@test.com", FirstName = "John", IsActive = true },
            new Booking { BookingId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Email = "user@test.com", FirstName = "Jane", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateBooking()
    {
        // Arrange
        var createDto = new BookingCreateDto
        {
            TourName = "Goa Tour",
            Place = "Goa",
            Email = "test@test.com",
            FirstName = "John"
        };
        var createdBooking = new Booking
        {
            BookingId = 1,
            TourName = createDto.TourName,
            Place = createDto.Place,
            Email = createDto.Email,
            FirstName = createDto.FirstName,
            IsActive = true
        };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdBooking);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.TourName.Should().Be("Goa Tour");
        result.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUserBookings()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Goa", Email = "test@test.com", FirstName = "John", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetByEmailAsync("test@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        // Act
        var result = await _service.GetByEmailAsync("test@test.com");

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        await _service.Invoking(s => s.DeleteAsync(999))
            .Should().ThrowAsync<NotFoundException>();
    }
}
