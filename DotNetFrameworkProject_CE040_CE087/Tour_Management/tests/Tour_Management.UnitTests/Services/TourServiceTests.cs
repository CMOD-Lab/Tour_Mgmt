using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;
using Xunit;

namespace Tour_Management.UnitTests.Services;

/// <summary>
/// Unit tests for the TourService class.
/// </summary>
public class TourServiceTests
{
    private readonly Mock<ITourRepository> _mockRepository;
    private readonly Mock<ILogger<TourService>> _mockLogger;
    private readonly IMapper _mapper;
    private readonly TourService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="TourServiceTests"/> class.
    /// </summary>
    public TourServiceTests()
    {
        _mockRepository = new Mock<ITourRepository>();
        _mockLogger = new Mock<ILogger<TourService>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new TourService(_mockRepository.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTours()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour { TourId = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 100, Locations = "Loc 1", TourInfo = "Info 1", IsActive = true },
            new Tour { TourId = 2, TourName = "Tour 2", Place = "Place 2", Days = 7, Price = 200, Locations = "Loc 2", TourInfo = "Info 2", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tours);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.TourName == "Tour 1");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTour()
    {
        // Arrange
        var tour = new Tour { TourId = 1, TourName = "Test Tour", Place = "Test Place", Days = 3, Price = 150, Locations = "Test Loc", TourInfo = "Test Info", IsActive = true };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(tour);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.TourName.Should().Be("Test Tour");
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Tour?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTourAndReturnIt()
    {
        // Arrange
        var tour = new Tour { TourName = "New Tour", Place = "New Place", Days = 4, Price = 300, Locations = "New Loc", TourInfo = "New Info" };
        var createdTour = new Tour { TourId = 1, TourName = "New Tour", Place = "New Place", Days = 4, Price = 300, Locations = "New Loc", TourInfo = "New Info", IsActive = true };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdTour);

        // Act
        var result = await _service.CreateAsync(tour);

        // Assert
        result.Should().NotBeNull();
        result.TourId.Should().Be(1);
        result.TourName.Should().Be("New Tour");
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTours()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour { TourId = 1, TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 100, Locations = "Goa", TourInfo = "Beach tour", IsActive = true }
        };
        _mockRepository.Setup(r => r.SearchAsync("Goa", It.IsAny<CancellationToken>())).ReturnsAsync(tours);

        // Act
        var result = await _service.SearchAsync("Goa");

        // Assert
        result.Should().HaveCount(1);
        result.First().TourName.Should().Be("Goa Beach Tour");
    }
}
