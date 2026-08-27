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
    /// Comprehensive unit tests for BookingService.
    /// </summary>
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<BookingService>> _mockLogger;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _mockRepository = new Mock<IBookingRepository>();
            _mockLogger = new Mock<ILogger<BookingService>>();

            var config = new MapperConfiguration(MappingProfile.Configure, NullLoggerFactory.Instance);
            _mapper = config.CreateMapper();

            _service = new BookingService(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        // ==================== GetAllAsync Tests ====================

        [Fact]
        public async Task GetAllAsync_WhenBookingsExist_ReturnsAllBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Kashmir Tour", Place = "Delhi", Email = "jane@example.com", FirstName = "Jane", IsActive = true }
            };
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            var list = new List<BookingDto>(result);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoBookings_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Booking>());

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
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Booking>());

            // Act
            await _service.GetAllAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ==================== GetByIdAsync Tests ====================

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsBookingDto()
        {
            // Arrange
            var booking = new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(booking);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.BookingId);
            Assert.Equal("Goa Tour", result.TourName);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Booking?)null);

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
            var bookingDate = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
            var booking = new Booking
            {
                BookingId = 5,
                TourName = "Kerala Tour",
                Place = "Bangalore",
                Email = "user@example.com",
                FirstName = "Alice",
                TourId = 3,
                BookingDate = bookingDate,
                IsActive = true
            };
            _mockRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(booking);

            // Act
            var result = await _service.GetByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result!.BookingId);
            Assert.Equal("Kerala Tour", result.TourName);
            Assert.Equal("Bangalore", result.Place);
            Assert.Equal("user@example.com", result.Email);
            Assert.Equal("Alice", result.FirstName);
            Assert.Equal(3, result.TourId);
        }

        // ==================== GetByEmailAsync Tests ====================

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnsBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { BookingId = 1, TourName = "Goa Tour", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Kerala Tour", Email = "john@example.com", FirstName = "John", IsActive = true }
            };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

            // Act
            var result = await _service.GetByEmailAsync("john@example.com");

            // Assert
            var list = new List<BookingDto>(result);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetByEmailAsync_WithNoBookings_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync("nobody@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Booking>());

            // Act
            var result = await _service.GetByEmailAsync("nobody@example.com");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByEmailAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetByEmailAsync("test@example.com"));
        }

        // ==================== CreateAsync Tests ====================

        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsBookingDto()
        {
            // Arrange
            var createDto = new BookingCreateDto
            {
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john@example.com",
                FirstName = "John",
                TourId = 1
            };
            var createdBooking = new Booking
            {
                BookingId = 1,
                TourName = createDto.TourName,
                Place = createDto.Place,
                Email = createDto.Email,
                FirstName = createDto.FirstName,
                TourId = createDto.TourId,
                IsActive = true
            };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdBooking);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.BookingId);
            Assert.Equal("Goa Tour", result.TourName);
        }

        [Fact]
        public async Task CreateAsync_CallsRepositoryAddOnce()
        {
            // Arrange
            var createDto = new BookingCreateDto { TourName = "Test Tour", Place = "Test", Email = "test@test.com", FirstName = "Test" };
            var createdBooking = new Booking { BookingId = 1, TourName = "Test Tour" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdBooking);

            // Act
            await _service.CreateAsync(createDto);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var createDto = new BookingCreateDto { TourName = "Test Tour", Place = "Test", Email = "test@test.com", FirstName = "Test" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(createDto));
        }

        // ==================== UpdateAsync Tests ====================

        [Fact]
        public async Task UpdateAsync_WithValidId_UpdatesBooking()
        {
            // Arrange
            var existingBooking = new Booking { BookingId = 1, TourName = "Old Tour", Place = "Old Place", Email = "old@test.com", FirstName = "Old", IsActive = true };
            var updateDto = new BookingUpdateDto { TourName = "New Tour", Place = "New Place", Email = "new@test.com", FirstName = "New", IsActive = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingBooking);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updateDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Booking?)null);
            var updateDto = new BookingUpdateDto { TourName = "New Tour", Place = "New Place", Email = "new@test.com", FirstName = "New" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(999, updateDto));
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var existingBooking = new Booking { BookingId = 1, TourName = "Old Tour", Place = "Old Place", Email = "old@test.com", FirstName = "Old" };
            var updateDto = new BookingUpdateDto { TourName = "New Tour", Place = "New Place", Email = "new@test.com", FirstName = "New" };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingBooking);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, updateDto));
        }

        // ==================== DeleteAsync Tests ====================

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesBooking()
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
        public async Task SearchAsync_WithMatchingTerm_ReturnsBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { BookingId = 1, TourName = "Goa Beach Tour", Email = "john@example.com", FirstName = "John", IsActive = true }
            };
            _mockRepository.Setup(r => r.SearchAsync("Goa", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

            // Act
            var result = await _service.SearchAsync("Goa");

            // Assert
            var list = new List<BookingDto>(result);
            Assert.Single(list);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatch_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Booking>());

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
    }
}
