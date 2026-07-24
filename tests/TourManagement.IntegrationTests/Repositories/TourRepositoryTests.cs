using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;
using Xunit;

namespace TourManagement.IntegrationTests.Repositories;

/// <summary>Integration tests for TourRepository using in-memory database.</summary>
public class TourRepositoryTests : IDisposable
{
    private readonly TourManagementDbContext _context;
    private readonly TourRepository _repository;

    /// <summary>Initializes a new instance of <see cref="TourRepositoryTests"/>.</summary>
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
            TourName = "Kashmir Tour",
            Place = "Kashmir",
            Days = 7,
            Price = 15000,
            Locations = "Srinagar, Gulmarg",
            TourInfo = "Beautiful Kashmir tour"
        };

        // Act
        await _repository.AddAsync(tour);

        // Assert
        var savedTour = await _context.Tours.FirstOrDefaultAsync(t => t.TourName == "Kashmir Tour");
        savedTour.Should().NotBeNull();
        savedTour!.Place.Should().Be("Kashmir");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTours()
    {
        // Arrange
        _context.Tours.AddRange(
            new Tour { TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 10000, Locations = "Loc1", TourInfo = "Info1" },
            new Tour { TourName = "Tour 2", Place = "Place 2", Days = 7, Price = 15000, Locations = "Loc2", TourInfo = "Info2" }
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
            new Tour { TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 15000, Locations = "Srinagar", TourInfo = "Info" },
            new Tour { TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 12000, Locations = "North Goa", TourInfo = "Info" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Kashmir");

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Be("Kashmir Tour");
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _context.Dispose();
    }
}
