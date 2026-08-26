using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Data;
using TourBooking.Infrastructure.Repositories;

namespace TourBooking.IntegrationTests.Repositories;

/// <summary>
/// Integration tests for TourRepository using in-memory database.
/// </summary>
public class TourRepositoryTests : IDisposable
{
    private readonly TourBookingDbContext _context;
    private readonly TourRepository _repository;

    /// <summary>Initializes a new instance of the <see cref="TourRepositoryTests"/> class.</summary>
    public TourRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TourBookingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TourBookingDbContext(options);
        var mockLogger = new Mock<ILogger<TourRepository>>();
        _repository = new TourRepository(_context, mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTourToDatabase()
    {
        // Arrange
        var tour = new Tour
        {
            TourName = "Goa Tour",
            Place = "Goa",
            Days = 5,
            Price = 15000,
            Locations = "Panaji, Calangute",
            TourInfo = "Beautiful beach tour"
        };

        // Act
        await _repository.AddAsync(tour);

        // Assert
        var savedTour = await _context.Tours.FirstOrDefaultAsync(t => t.TourName == "Goa Tour");
        savedTour.Should().NotBeNull();
        savedTour!.Place.Should().Be("Goa");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Tour 1", Place = "Place 1", Days = 3, Price = 10000, Locations = "L1", TourInfo = "Info 1" },
            new Tour { TourName = "Tour 2", Place = "Place 2", Days = 5, Price = 20000, Locations = "L2", TourInfo = "Info 2" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa", TourInfo = "Beach" },
            new Tour { TourName = "Kashmir Valley Tour", Place = "Kashmir", Days = 7, Price = 25000, Locations = "Kashmir", TourInfo = "Valley" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Goa");

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Be("Goa Beach Tour");
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _context.Dispose();
    }
}
