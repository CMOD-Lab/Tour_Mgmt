using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Xunit;

namespace Tour_Management.UnitTests.Services;

/// <summary>
/// Unit tests for the BookingService class.
/// </summary>
public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockRepository;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly IMapper _mapper;
    private readonly BookingService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookingServiceTests"/> class.
    /// </summary>
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
            new Booking { BookingId = 1, TourName = "Tour 1", Place = "Place 1", Email = "user1@test.com", FirstName = "John", IsActive = true },
            new Booking { BookingId = 2, TourName = "Tour 2", Place = "Place 2", Email = "user2@test.com", FirstName = "Jane", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnBookingsForEmail()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { BookingId = 1, TourName = "Tour 1", Place = "Place 1", Email = "user@test.com", FirstName = "John", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        // Act
        var result = await _service.GetByEmailAsync("user@test.com");

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateBookingAndReturnIt()
    {
        // Arrange
        var booking = new Booking { TourName = "New Tour", Place = "New Place", Email = "user@test.com", FirstName = "John" };
        var createdBooking = new Booking { BookingId = 1, TourName = "New Tour", Place = "New Place", Email = "user@test.com", FirstName = "John", IsActive = true };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdBooking);

        // Act
        var result = await _service.CreateAsync(booking);

        // Assert
        result.Should().NotBeNull();
        result.BookingId.Should().Be(1);
        result.TourName.Should().Be("New Tour");
    }
}
