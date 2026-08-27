using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

namespace TourManagement.IntegrationTests.Repositories;

/// <summary>
/// Integration tests for TourRepository using in-memory database.
/// </summary>
public class TourRepositoryTests : IDisposable
{
    private readonly TourManagementDbContext _context;
    private readonly TourRepository _repository;

    /// <summary>Initializes test dependencies with in-memory database.</summary>
    public TourRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TourManagementDbContext(options);
        var logger = new Mock<ILogger<TourRepository>>().Object;
        _repository = new TourRepository(_context, logger);
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
            IsActive = true
        };

        // Act
        var result = await _repository.AddAsync(tour);

        // Assert
        result.TourId.Should().BeGreaterThan(0);
        var saved = await _context.Tours.FindAsync(result.TourId);
        saved.Should().NotBeNull();
        saved!.TourName.Should().Be("Test Tour");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyActiveTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Active Tour", Place = "Place1", Days = 3, Price = 5000, Locations = "Loc1", TourInfo = "Info1", IsActive = true },
            new Tour { TourName = "Inactive Tour", Place = "Place2", Days = 4, Price = 6000, Locations = "Loc2", TourInfo = "Info2", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Be("Active Tour");
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _context.Dispose();
    }
}

// Minimal Mock class for ILogger (avoids Moq dependency in integration tests)
internal class Mock<T> where T : class
{
    public T Object => (T)(object)new LoggerFactory().CreateLogger<T>();
}
