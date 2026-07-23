using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;
using Xunit;

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
            TourName = "Test Tour",
            Place = "Test Place",
            Days = 5,
            Price = 10000,
            Locations = "Test Locations",
            TourInfo = "Test Info"
        };

        // Act
        await _repository.AddAsync(tour);

        // Assert
        var savedTour = await _context.Tours.FirstOrDefaultAsync(t => t.TourName == "Test Tour");
        savedTour.Should().NotBeNull();
        savedTour!.Place.Should().Be("Test Place");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Tour 1", Place = "Place 1", Days = 3, Price = 5000, Locations = "Loc 1", TourInfo = "Info 1" },
            new Tour { TourName = "Tour 2", Place = "Place 2", Days = 5, Price = 10000, Locations = "Loc 2", TourInfo = "Info 2" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTourFromDatabase()
    {
        // Arrange
        var tour = new Tour { TourName = "Delete Tour", Place = "Place", Days = 3, Price = 5000, Locations = "Loc", TourInfo = "Info" };
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(tour.TourId);

        // Assert
        var deletedTour = await _context.Tours.FindAsync(tour.TourId);
        deletedTour.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var tour = new Tour { TourName = "Exists Tour", Place = "Place", Days = 3, Price = 5000, Locations = "Loc", TourInfo = "Info" };
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(tour.TourId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(9999);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
