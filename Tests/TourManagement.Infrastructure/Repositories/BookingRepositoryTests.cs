using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;
using Xunit;

namespace TourManagement.Infrastructure.Repositories.Tests
{
    /// <summary>
    /// Unit tests for BookingRepository using in-memory database.
    /// </summary>
    public class BookingRepositoryTests : IDisposable
    {
        private readonly TourManagementDbContext _context;
        private readonly BookingRepository _repository;

        public BookingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TourManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TourManagementDbContext(options);
            _repository = new BookingRepository(_context, NullLogger<BookingRepository>.Instance);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // ==================== GetAllAsync Tests ====================

        [Fact]
        public async Task GetAllAsync_WhenActiveBookingsExist_ReturnsActiveBookings()
        {
            // Arrange
            _context.Bookings.AddRange(
                new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Kashmir Tour", Place = "Delhi", Email = "jane@example.com", FirstName = "Jane", IsActive = true },
                new Booking { BookingId = 3, TourName = "Inactive Tour", Place = "Nowhere", Email = "inactive@example.com", FirstName = "Inactive", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var list = new List<Booking>(result);
            Assert.Equal(2, list.Count);
            Assert.All(list, b => Assert.True(b.IsActive));
        }

        [Fact]
        public async Task GetAllAsync_WhenNoActiveBookings_ReturnsEmptyList()
        {
            // Arrange
            _context.Bookings.Add(new Booking { BookingId = 1, TourName = "Inactive Tour", Place = "Nowhere", Email = "inactive@example.com", FirstName = "Inactive", IsActive = false });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        // ==================== GetByIdAsync Tests ====================

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsBooking()
        {
            // Arrange
            _context.Bookings.Add(new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.BookingId);
            Assert.Equal("Goa Tour", result.TourName);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        // ==================== GetByEmailAsync Tests ====================

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnsBookings()
        {
            // Arrange
            _context.Bookings.AddRange(
                new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Kerala Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 3, TourName = "Kashmir Tour", Place = "Delhi", Email = "jane@example.com", FirstName = "Jane", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync("john@example.com");

            // Assert
            var list = new List<Booking>(result);
            Assert.Equal(2, list.Count);
            Assert.All(list, b => Assert.Equal("john@example.com", b.Email));
        }

        [Fact]
        public async Task GetByEmailAsync_WithNoBookings_ReturnsEmptyList()
        {
            // Act
            var result = await _repository.GetByEmailAsync("nobody@example.com");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ExcludesInactiveBookings()
        {
            // Arrange
            _context.Bookings.AddRange(
                new Booking { BookingId = 1, TourName = "Active Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Inactive Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync("john@example.com");

            // Assert
            var list = new List<Booking>(result);
            Assert.Single(list);
            Assert.Equal("Active Tour", list[0].TourName);
        }

        // ==================== AddAsync Tests ====================

        [Fact]
        public async Task AddAsync_WithValidBooking_AddsBookingToDatabase()
        {
            // Arrange
            var booking = new Booking
            {
                TourName = "New Tour",
                Place = "New Place",
                Email = "newuser@example.com",
                FirstName = "New",
                IsActive = true
            };

            // Act
            var result = await _repository.AddAsync(booking);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.BookingId > 0);
            Assert.Equal("New Tour", result.TourName);
            Assert.Equal(1, await _context.Bookings.CountAsync());
        }

        // ==================== UpdateAsync Tests ====================

        [Fact]
        public async Task UpdateAsync_WithValidBooking_UpdatesBookingInDatabase()
        {
            // Arrange
            var booking = new Booking { BookingId = 1, TourName = "Old Tour", Place = "Old Place", Email = "old@test.com", FirstName = "Old", IsActive = true };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var updatedBooking = new Booking { BookingId = 1, TourName = "New Tour", Place = "New Place", Email = "new@test.com", FirstName = "New", IsActive = true };

            // Act
            await _repository.UpdateAsync(updatedBooking);

            // Assert
            var dbBooking = await _context.Bookings.FindAsync(1);
            Assert.NotNull(dbBooking);
            Assert.Equal("New Tour", dbBooking!.TourName);
        }

        // ==================== DeleteAsync Tests ====================

        [Fact]
        public async Task DeleteAsync_WithValidId_SetsIsActiveToFalse()
        {
            // Arrange
            _context.Bookings.Add(new Booking { BookingId = 1, TourName = "Tour To Delete", Place = "Place", Email = "user@test.com", FirstName = "User", IsActive = true });
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var dbBooking = await _context.Bookings.FindAsync(1);
            Assert.NotNull(dbBooking);
            Assert.False(dbBooking!.IsActive);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_DoesNotThrow()
        {
            // Act & Assert (should not throw)
            await _repository.DeleteAsync(999);
        }

        // ==================== ExistsAsync Tests ====================

        [Fact]
        public async Task ExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            _context.Bookings.Add(new Booking { BookingId = 1, TourName = "Existing Tour", Place = "Place", Email = "user@test.com", FirstName = "User", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistentId_ReturnsFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        // ==================== SearchAsync Tests ====================

        [Fact]
        public async Task SearchAsync_WithMatchingTourName_ReturnsBookings()
        {
            // Arrange
            _context.Bookings.AddRange(
                new Booking { BookingId = 1, TourName = "Goa Beach Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Kashmir Tour", Place = "Delhi", Email = "jane@example.com", FirstName = "Jane", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("Goa");

            // Assert
            var list = new List<Booking>(result);
            Assert.Single(list);
            Assert.Equal("Goa Beach Tour", list[0].TourName);
        }

        [Fact]
        public async Task SearchAsync_WithMatchingFirstName_ReturnsBookings()
        {
            // Arrange
            _context.Bookings.AddRange(
                new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Kashmir Tour", Place = "Delhi", Email = "jane@example.com", FirstName = "Jane", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("John");

            // Assert
            var list = new List<Booking>(result);
            Assert.Single(list);
            Assert.Equal("John", list[0].FirstName);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatch_ReturnsEmptyList()
        {
            // Arrange
            _context.Bookings.Add(new Booking { BookingId = 1, TourName = "Goa Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("NonExistent");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchAsync_IsCaseInsensitive()
        {
            // Arrange
            _context.Bookings.Add(new Booking { BookingId = 1, TourName = "Goa Beach Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("goa");

            // Assert
            var list = new List<Booking>(result);
            Assert.Single(list);
        }

        [Fact]
        public async Task SearchAsync_ExcludesInactiveBookings()
        {
            // Arrange
            _context.Bookings.AddRange(
                new Booking { BookingId = 1, TourName = "Goa Active Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = true },
                new Booking { BookingId = 2, TourName = "Goa Inactive Tour", Place = "Mumbai", Email = "john@example.com", FirstName = "John", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("Goa");

            // Assert
            var list = new List<Booking>(result);
            Assert.Single(list);
            Assert.Equal("Goa Active Tour", list[0].TourName);
        }
    }
}
