using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Domain.Entities;
using Tour_Management.Infrastructure.Data;
using Tour_Management.Infrastructure.Repositories;
using Xunit;

namespace Tour_Management.IntegrationTests.Repositories;

/// <summary>
/// Integration tests for the TourRepository class using in-memory database.
/// </summary>
public class TourRepositoryTests : IDisposable
{
    private readonly TourManagementDbContext _context;
    private readonly TourRepository _repository;
    private readonly Mock<ILogger<TourRepository>> _mockLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TourRepositoryTests"/> class.
    /// </summary>
    public TourRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TourManagementDbContext(options);
        _mockLogger = new Mock<ILogger<TourRepository>>();
        _repository = new TourRepository(_context, _mockLogger.Object);
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
            Price = 100.00m,
            Locations = "Test Locations",
            TourInfo = "Test Info",
            IsActive = true,
            CreatedBy = "test"
        };

        // Act
        var result = await _repository.AddAsync(tour);

        // Assert
        result.TourId.Should().BeGreaterThan(0);
        result.TourName.Should().Be("Test Tour");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyActiveTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Active Tour", Place = "Place 1", Days = 3, Price = 100, Locations = "Loc 1", TourInfo = "Info 1", IsActive = true, CreatedBy = "test" },
            new Tour { TourName = "Inactive Tour", Place = "Place 2", Days = 4, Price = 200, Locations = "Loc 2", TourInfo = "Info 2", IsActive = false, CreatedBy = "test" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Be("Active Tour");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTour()
    {
        // Arrange
        var tour = new Tour { TourName = "Test Tour", Place = "Test Place", Days = 3, Price = 100, Locations = "Loc", TourInfo = "Info", IsActive = true, CreatedBy = "test" };
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(tour.TourId);

        // Assert
        result.Should().NotBeNull();
        result!.TourName.Should().Be("Test Tour");
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeleteTour()
    {
        // Arrange
        var tour = new Tour { TourName = "Test Tour", Place = "Test Place", Days = 3, Price = 100, Locations = "Loc", TourInfo = "Info", IsActive = true, CreatedBy = "test" };
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(tour.TourId);

        // Assert
        result.Should().BeTrue();
        var deletedTour = await _context.Tours.FindAsync(tour.TourId);
        deletedTour!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 100, Locations = "Goa Beach", TourInfo = "Beach tour", IsActive = true, CreatedBy = "test" },
            new Tour { TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 200, Locations = "Kashmir Valley", TourInfo = "Mountain tour", IsActive = true, CreatedBy = "test" }
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
