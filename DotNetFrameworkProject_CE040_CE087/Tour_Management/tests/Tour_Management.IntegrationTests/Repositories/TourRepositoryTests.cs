using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Domain.Entities;
using Tour_Management.Infrastructure.Data;
using Tour_Management.Infrastructure.Repositories;
using Xunit;

namespace Tour_Management.IntegrationTests.Repositories;

/// <summary>Integration tests for TourRepository using in-memory database.</summary>
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
            new Tour { TourName = "Active Tour", Place = "Place1", Days = 3, Price = 5000, Locations = "L1", TourInfo = "Info1", IsActive = true, CreatedBy = "test" },
            new Tour { TourName = "Inactive Tour", Place = "Place2", Days = 4, Price = 6000, Locations = "L2", TourInfo = "Info2", IsActive = false, CreatedBy = "test" }
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
        var tour = new Tour { TourName = "Test Tour", Place = "Place", Days = 3, Price = 5000, Locations = "L1", TourInfo = "Info", IsActive = true, CreatedBy = "test" };
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
        var tour = new Tour { TourName = "Test Tour", Place = "Place", Days = 3, Price = 5000, Locations = "L1", TourInfo = "Info", IsActive = true, CreatedBy = "test" };
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
            new Tour { TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa", TourInfo = "Beach", IsActive = true, CreatedBy = "test" },
            new Tour { TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 25000, Locations = "Kashmir", TourInfo = "Mountains", IsActive = true, CreatedBy = "test" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("goa");

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Contain("Goa");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
