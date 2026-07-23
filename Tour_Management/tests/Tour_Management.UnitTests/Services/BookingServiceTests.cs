using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Application.Services;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;
using AutoMapper;
using Tour_Management.Application.Mappings;
using Xunit;

namespace Tour_Management.UnitTests.Services;

/// <summary>
/// Unit tests for BookingService.
/// </summary>
public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _mockRepository = new Mock<IBookingRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _mockLogger = new Mock<ILogger<BookingService>>();
        _service = new BookingService(_mockRepository.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllBookingsAsync_ShouldReturnAllBookings()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new() { BookingId = 1, TourName = "Goa Tour", Place = "Goa", Email = "user@test.com", FirstName = "John", IsActive = true },
            new() { BookingId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Email = "user2@test.com", FirstName = "Jane", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        // Act
        var result = await _service.GetAllBookingsAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldCreateAndReturnBooking()
    {
        // Arrange
        var booking = new Booking { TourName = "Goa Tour", Place = "Goa", Email = "user@test.com", FirstName = "John" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(booking);

        // Act
        var result = await _service.CreateBookingAsync(booking);

        // Assert
        result.Should().NotBeNull();
        result.TourName.Should().Be("Goa Tour");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteBookingAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBookingAsync(999));
    }

    [Fact]
    public async Task GetBookingsByEmailAsync_ShouldReturnUserBookings()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new() { BookingId = 1, TourName = "Goa Tour", Place = "Goa", Email = "user@test.com", FirstName = "John", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        // Act
        var result = await _service.GetBookingsByEmailAsync("user@test.com");

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("user@test.com");
    }
}
