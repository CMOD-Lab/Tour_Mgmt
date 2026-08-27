using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TourManagement.Application.DTOs;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using Xunit;

namespace TourManagement.Application.Services.Tests
{
    /// <summary>
    /// Comprehensive unit tests for TourService.
    /// </summary>
    public class TourServiceTests
    {
        private readonly Mock<ITourRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TourService>> _mockLogger;
        private readonly TourService _service;

        public TourServiceTests()
        {
            _mockRepository = new Mock<ITourRepository>();
            _mockLogger = new Mock<ILogger<TourService>>();

            var config = new MapperConfiguration(MappingProfile.Configure, NullLoggerFactory.Instance);
            _mapper = config.CreateMapper();

            _service = new TourService(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        // ==================== GetAllAsync Tests ====================

        [Fact]
        public async Task GetAllAsync_WhenToursExist_ReturnsAllTours()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true },
                new Tour { TourId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 25000, Locations = "Dal Lake", TourInfo = "Paradise on earth", IsActive = true }
            };
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tours);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            var list = new List<TourDto>(result);
            Assert.Equal(2, list.Count);
            Assert.Equal("Goa Tour", list[0].TourName);
            Assert.Equal("Kashmir Tour", list[1].TourName);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoTours_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Tour>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
        }

        [Fact]
        public async Task GetAllAsync_CallsRepositoryOnce()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Tour>());

            // Act
            await _service.GetAllAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ==================== GetByIdAsync Tests ====================

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsTourDto()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(tour);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.TourId);
            Assert.Equal("Goa Tour", result.TourName);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Tour?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByIdAsync_MapsAllProperties()
        {
            // Arrange
            var tour = new Tour
            {
                TourId = 5,
                TourName = "Kerala Tour",
                Place = "Kerala",
                Days = 6,
                Price = 20000m,
                Locations = "Backwaters",
                TourInfo = "God's own country",
                Pic = "kerala.jpg",
                IsActive = true
            };
            _mockRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(tour);

            // Act
            var result = await _service.GetByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result!.TourId);
            Assert.Equal("Kerala Tour", result.TourName);
            Assert.Equal("Kerala", result.Place);
            Assert.Equal(6, result.Days);
            Assert.Equal(20000m, result.Price);
            Assert.Equal("Backwaters", result.Locations);
            Assert.Equal("God's own country", result.TourInfo);
            Assert.Equal("kerala.jpg", result.Pic);
        }

        // ==================== CreateAsync Tests ====================

        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsTourDto()
        {
            // Arrange
            var createDto = new TourCreateDto
            {
                TourName = "Kerala Tour",
                Place = "Kerala",
                Days = 6,
                Price = 20000,
                Locations = "Backwaters",
                TourInfo = "God's own country"
            };
            var createdTour = new Tour
            {
                TourId = 3,
                TourName = createDto.TourName,
                Place = createDto.Place,
                Days = createDto.Days,
                Price = createDto.Price,
                Locations = createDto.Locations,
                TourInfo = createDto.TourInfo,
                IsActive = true
            };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdTour);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TourId);
            Assert.Equal("Kerala Tour", result.TourName);
        }

        [Fact]
        public async Task CreateAsync_CallsRepositoryAddOnce()
        {
            // Arrange
            var createDto = new TourCreateDto { TourName = "Test Tour", Place = "Test", Days = 3, Price = 5000, Locations = "Test", TourInfo = "Test info" };
            var createdTour = new Tour { TourId = 1, TourName = "Test Tour" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdTour);

            // Act
            await _service.CreateAsync(createDto);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var createDto = new TourCreateDto { TourName = "Test Tour", Place = "Test", Days = 3, Price = 5000, Locations = "Test", TourInfo = "Test info" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(createDto));
        }

        // ==================== UpdateAsync Tests ====================

        [Fact]
        public async Task UpdateAsync_WithValidId_UpdatesTour()
        {
            // Arrange
            var existingTour = new Tour { TourId = 1, TourName = "Old Name", Place = "Old Place", Days = 3, Price = 5000, Locations = "Old", TourInfo = "Old info", IsActive = true };
            var updateDto = new TourUpdateDto { TourName = "New Name", Place = "New Place", Days = 5, Price = 10000, Locations = "New", TourInfo = "New info", IsActive = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingTour);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updateDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Tour?)null);
            var updateDto = new TourUpdateDto { TourName = "New Name", Place = "New Place", Days = 5, Price = 10000, Locations = "New", TourInfo = "New info" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(999, updateDto));
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var existingTour = new Tour { TourId = 1, TourName = "Old Name", Place = "Old Place", Days = 3, Price = 5000, Locations = "Old", TourInfo = "Old info" };
            var updateDto = new TourUpdateDto { TourName = "New Name", Place = "New Place", Days = 5, Price = 10000, Locations = "New", TourInfo = "New info" };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingTour);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, updateDto));
        }

        // ==================== DeleteAsync Tests ====================

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesTour()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(999));
        }

        [Fact]
        public async Task DeleteAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
        }

        // ==================== SearchAsync Tests ====================

        [Fact]
        public async Task SearchAsync_WithMatchingTerm_ReturnsTours()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { TourId = 1, TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true }
            };
            _mockRepository.Setup(r => r.SearchAsync("Goa", It.IsAny<CancellationToken>())).ReturnsAsync(tours);

            // Act
            var result = await _service.SearchAsync("Goa");

            // Assert
            var list = new List<TourDto>(result);
            Assert.Single(list);
            Assert.Equal("Goa Beach Tour", list[0].TourName);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatch_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Tour>());

            // Act
            var result = await _service.SearchAsync("NonExistent");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.SearchAsync("test"));
        }

        [Fact]
        public async Task SearchAsync_CallsRepositoryWithCorrectTerm()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync("Kerala", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Tour>());

            // Act
            await _service.SearchAsync("Kerala");

            // Assert
            _mockRepository.Verify(r => r.SearchAsync("Kerala", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
