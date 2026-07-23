using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;

namespace TourManagement.IntegrationTests.Repositories;

/// <summary>
/// Integration tests for TourRepository using in-memory database.
/// </summary>
public class TourRepositoryTests : IDisposable
{
    private readonly TourManagementDbContext _context;
    private readonly TourRepository _repository;

    public TourRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TourManagementDbContext(options);
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
            Locations = "North Goa, South Goa",
            TourInfo = "Beautiful beach tour",
            IsActive = true
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
            new Tour { TourName = "Tour 1", Place = "Place 1", Days = 3, Price = 10000, Locations = "L1", TourInfo = "Info 1", IsActive = true },
            new Tour { TourName = "Tour 2", Place = "Place 2", Days = 5, Price = 20000, Locations = "L2", TourInfo = "Info 2", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetActiveToursAsync_ShouldReturnOnlyActiveTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Active Tour", Place = "Place 1", Days = 3, Price = 10000, Locations = "L1", TourInfo = "Info 1", IsActive = true },
            new Tour { TourName = "Inactive Tour", Place = "Place 2", Days = 5, Price = 20000, Locations = "L2", TourInfo = "Info 2", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveToursAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Be("Active Tour");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTourFromDatabase()
    {
        // Arrange
        var tour = new Tour { TourName = "To Delete", Place = "Place", Days = 3, Price = 10000, Locations = "L", TourInfo = "Info", IsActive = true };
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(tour.TourId);

        // Assert
        var deletedTour = await _context.Tours.FindAsync(tour.TourId);
        deletedTour.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
