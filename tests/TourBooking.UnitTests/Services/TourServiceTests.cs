using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using TourBooking.Application.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Exceptions;
using TourBooking.Domain.Interfaces.Repositories;

namespace TourBooking.UnitTests.Services
{
    /// <summary>
    /// Comprehensive unit tests for TourService.
    /// </summary>
    public class TourServiceTests
    {
        private readonly Mock<ITourRepository> _mockRepository;
        private readonly Mock<ILogger<TourService>> _mockLogger;
        private readonly TourService _service;

        public TourServiceTests()
        {
            _mockRepository = new Mock<ITourRepository>();
            _mockLogger = new Mock<ILogger<TourService>>();
            _service = new TourService(_mockRepository.Object, _mockLogger.Object);
        }

        // GetAllAsync tests
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTours()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000 },
                new Tour { TourId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 25000 }
            };
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tours);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(t => t.TourName == "Goa Tour");
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Tour>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await _service.Invoking(s => s.GetAllAsync())
                .Should().ThrowAsync<Exception>().WithMessage("Database error");
        }

        // GetByIdAsync tests
        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnTour()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000 };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tour);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.TourName.Should().Be("Goa Tour");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tour?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.GetByIdAsync(1))
                .Should().ThrowAsync<Exception>();
        }

        // CreateAsync tests
        [Fact]
        public async Task CreateAsync_WithValidTour_ShouldSucceed()
        {
            // Arrange
            var tour = new Tour { TourName = "Kerala Tour", Place = "Kerala", Days = 6, Price = 20000, Locations = "Munnar, Alleppey", TourInfo = "Beautiful tour" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(tour);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            var tour = new Tour { TourName = "Tour" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.CreateAsync(tour))
                .Should().ThrowAsync<Exception>();
        }

        // UpdateAsync tests
        [Fact]
        public async Task UpdateAsync_WithExistingTour_ShouldReturnTrue()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Updated Tour", Place = "Updated Place", Days = 5, Price = 10000, Locations = "Loc", TourInfo = "Info" };
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(tour);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistentTour_ShouldThrowNotFoundException()
        {
            // Arrange
            var tour = new Tour { TourId = 999, TourName = "Non-existent Tour" };
            _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _service.Invoking(s => s.UpdateAsync(tour))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Tour" };
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.UpdateAsync(tour))
                .Should().ThrowAsync<Exception>();
        }

        // DeleteAsync tests
        [Fact]
        public async Task DeleteAsync_WithNonExistentId_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(999))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldSucceed()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(1))
                .Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallDelete_WhenTourDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            try { await _service.DeleteAsync(999); } catch { }

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        // SearchAsync tests
        [Fact]
        public async Task SearchAsync_WithValidTerm_ShouldReturnMatchingTours()
        {
            // Arrange
            var searchTerm = "Goa";
            var tours = new List<Tour>
            {
                new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000 }
            };
            _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tours);

            // Act
            var result = await _service.SearchAsync(searchTerm);

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(t => t.TourName == "Goa Tour");
        }

        [Fact]
        public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Tour>());

            // Act
            var result = await _service.SearchAsync("NonExistentTour");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.SearchAsync("term"))
                .Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task SearchAsync_ShouldCallRepositoryWithCorrectTerm()
        {
            // Arrange
            var searchTerm = "Kerala";
            _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Tour>());

            // Act
            await _service.SearchAsync(searchTerm);

            // Assert
            _mockRepository.Verify(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
