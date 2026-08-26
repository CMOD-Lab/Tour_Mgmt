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
    /// Comprehensive unit tests for BookingService.
    /// </summary>
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockRepository;
        private readonly Mock<ILogger<BookingService>> _mockLogger;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _mockRepository = new Mock<IBookingRepository>();
            _mockLogger = new Mock<ILogger<BookingService>>();
            _service = new BookingService(_mockRepository.Object, _mockLogger.Object);
        }

        // GetAllAsync tests
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { TourId = 1, TourName = "Goa Tour", Place = "Goa", Email = "user1@test.com", FirstName = "John" },
                new Booking { TourId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Email = "user2@test.com", FirstName = "Jane" }
            };
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(b => b.TourName == "Goa Tour");
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Booking>());

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
        public async Task GetByIdAsync_WithValidId_ShouldReturnBooking()
        {
            // Arrange
            var booking = new Booking { TourId = 1, TourName = "Goa Tour", Place = "Goa", Email = "user@test.com", FirstName = "John" };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.TourName.Should().Be("Goa Tour");
            result.TourId.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

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

        // GetByEmailAsync tests
        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ShouldReturnBookings()
        {
            // Arrange
            var email = "user@test.com";
            var bookings = new List<Booking>
            {
                new Booking { TourId = 1, TourName = "Goa Tour", Email = email, FirstName = "John" },
                new Booking { TourId = 2, TourName = "Kerala Tour", Email = email, FirstName = "John" }
            };
            _mockRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);

            // Act
            var result = await _service.GetByEmailAsync(email);

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(b => b.Email == email);
        }

        [Fact]
        public async Task GetByEmailAsync_WithNoBookings_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Booking>());

            // Act
            var result = await _service.GetByEmailAsync("noone@test.com");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByEmailAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.GetByEmailAsync("user@test.com"))
                .Should().ThrowAsync<Exception>();
        }

        // CreateAsync tests
        [Fact]
        public async Task CreateAsync_WithValidBooking_ShouldReturnTrue()
        {
            // Arrange
            var booking = new Booking { TourName = "Goa Tour", Place = "Goa", Email = "user@test.com", FirstName = "John" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(booking);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            var booking = new Booking { TourName = "Goa Tour" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.CreateAsync(booking))
                .Should().ThrowAsync<Exception>();
        }

        // UpdateAsync tests
        [Fact]
        public async Task UpdateAsync_WithExistingBooking_ShouldReturnTrue()
        {
            // Arrange
            var booking = new Booking { TourId = 1, TourName = "Updated Tour", Place = "Updated Place" };
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(booking);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistentBooking_ShouldThrowNotFoundException()
        {
            // Arrange
            var booking = new Booking { TourId = 999, TourName = "Non-existent Tour" };
            _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _service.Invoking(s => s.UpdateAsync(booking))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            var booking = new Booking { TourId = 1, TourName = "Tour" };
            _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.UpdateAsync(booking))
                .Should().ThrowAsync<Exception>();
        }

        // DeleteAsync tests
        [Fact]
        public async Task DeleteAsync_WithExistingId_ShouldReturnTrue()
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
        public async Task DeleteAsync_ShouldNotCallDelete_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            try { await _service.DeleteAsync(999); } catch { }

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WithCancellationToken_ShouldPassTokenToRepository()
        {
            // Arrange
            var booking = new Booking { TourName = "Tour", Place = "Place", Email = "e@e.com", FirstName = "F" };
            var cts = new CancellationTokenSource();
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), cts.Token))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(booking, cts.Token);

            // Assert
            result.Should().BeTrue();
        }
    }
}
