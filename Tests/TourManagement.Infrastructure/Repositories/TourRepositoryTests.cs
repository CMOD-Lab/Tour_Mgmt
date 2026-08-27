using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;
using Xunit;

namespace TourManagement.Infrastructure.Repositories.Tests
{
    /// <summary>
    /// Unit tests for TourRepository using in-memory database.
    /// </summary>
    public class TourRepositoryTests : IDisposable
    {
        private readonly TourManagementDbContext _context;
        private readonly TourRepository _repository;
        private readonly ILogger<TourRepository> _logger;

        public TourRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TourManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TourManagementDbContext(options);
            _logger = NullLogger<TourRepository>.Instance;
            _repository = new TourRepository(_context, _logger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // ==================== GetAllAsync Tests ====================

        [Fact]
        public async Task GetAllAsync_WhenActiveToursExist_ReturnsActiveTours()
        {
            // Arrange
            _context.Tours.AddRange(
                new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true },
                new Tour { TourId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 25000, Locations = "Dal Lake", TourInfo = "Paradise", IsActive = true },
                new Tour { TourId = 3, TourName = "Inactive Tour", Place = "Nowhere", Days = 1, Price = 100, Locations = "None", TourInfo = "Inactive", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var list = new List<Tour>(result);
            Assert.Equal(2, list.Count);
            Assert.All(list, t => Assert.True(t.IsActive));
        }

        [Fact]
        public async Task GetAllAsync_WhenNoActiveTours_ReturnsEmptyList()
        {
            // Arrange
            _context.Tours.Add(new Tour { TourId = 1, TourName = "Inactive Tour", Place = "Nowhere", Days = 1, Price = 100, Locations = "None", TourInfo = "Inactive", IsActive = false });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsToursOrderedByName()
        {
            // Arrange
            _context.Tours.AddRange(
                new Tour { TourId = 1, TourName = "Zulu Tour", Place = "Z", Days = 1, Price = 100, Locations = "Z", TourInfo = "Z", IsActive = true },
                new Tour { TourId = 2, TourName = "Alpha Tour", Place = "A", Days = 1, Price = 100, Locations = "A", TourInfo = "A", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var list = new List<Tour>(result);
            Assert.Equal("Alpha Tour", list[0].TourName);
            Assert.Equal("Zulu Tour", list[1].TourName);
        }

        // ==================== GetByIdAsync Tests ====================

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsTour()
        {
            // Arrange
            _context.Tours.Add(new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.TourId);
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

        // ==================== AddAsync Tests ====================

        [Fact]
        public async Task AddAsync_WithValidTour_AddsTourToDatabase()
        {
            // Arrange
            var tour = new Tour { TourName = "New Tour", Place = "New Place", Days = 3, Price = 5000, Locations = "New Locations", TourInfo = "New info", IsActive = true };

            // Act
            var result = await _repository.AddAsync(tour);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.TourId > 0);
            Assert.Equal("New Tour", result.TourName);
            Assert.Equal(1, await _context.Tours.CountAsync());
        }

        // ==================== UpdateAsync Tests ====================

        [Fact]
        public async Task UpdateAsync_WithValidTour_UpdatesTourInDatabase()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Old Name", Place = "Old Place", Days = 3, Price = 5000, Locations = "Old", TourInfo = "Old info", IsActive = true };
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var updatedTour = new Tour { TourId = 1, TourName = "New Name", Place = "New Place", Days = 7, Price = 25000, Locations = "New", TourInfo = "New info", IsActive = true };

            // Act
            await _repository.UpdateAsync(updatedTour);

            // Assert
            var dbTour = await _context.Tours.FindAsync(1);
            Assert.NotNull(dbTour);
            Assert.Equal("New Name", dbTour!.TourName);
        }

        // ==================== DeleteAsync Tests ====================

        [Fact]
        public async Task DeleteAsync_WithValidId_SetsIsActiveToFalse()
        {
            // Arrange
            _context.Tours.Add(new Tour { TourId = 1, TourName = "Tour To Delete", Place = "Place", Days = 3, Price = 5000, Locations = "Loc", TourInfo = "Info", IsActive = true });
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var dbTour = await _context.Tours.FindAsync(1);
            Assert.NotNull(dbTour);
            Assert.False(dbTour!.IsActive);
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
            _context.Tours.Add(new Tour { TourId = 1, TourName = "Existing Tour", Place = "Place", Days = 3, Price = 5000, Locations = "Loc", TourInfo = "Info", IsActive = true });
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
        public async Task SearchAsync_WithMatchingTourName_ReturnsTours()
        {
            // Arrange
            _context.Tours.AddRange(
                new Tour { TourId = 1, TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true },
                new Tour { TourId = 2, TourName = "Kashmir Tour", Place = "Kashmir", Days = 7, Price = 25000, Locations = "Dal Lake", TourInfo = "Paradise", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("Goa");

            // Assert
            var list = new List<Tour>(result);
            Assert.Single(list);
            Assert.Equal("Goa Beach Tour", list[0].TourName);
        }

        [Fact]
        public async Task SearchAsync_WithMatchingPlace_ReturnsTours()
        {
            // Arrange
            _context.Tours.AddRange(
                new Tour { TourId = 1, TourName = "Beach Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true },
                new Tour { TourId = 2, TourName = "Mountain Tour", Place = "Kashmir", Days = 7, Price = 25000, Locations = "Dal Lake", TourInfo = "Paradise", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("Kashmir");

            // Assert
            var list = new List<Tour>(result);
            Assert.Single(list);
            Assert.Equal("Mountain Tour", list[0].TourName);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatch_ReturnsEmptyList()
        {
            // Arrange
            _context.Tours.Add(new Tour { TourId = 1, TourName = "Goa Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true });
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
            _context.Tours.Add(new Tour { TourId = 1, TourName = "Goa Beach Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("goa");

            // Assert
            var list = new List<Tour>(result);
            Assert.Single(list);
        }

        [Fact]
        public async Task SearchAsync_ExcludesInactiveTours()
        {
            // Arrange
            _context.Tours.AddRange(
                new Tour { TourId = 1, TourName = "Goa Active Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = true },
                new Tour { TourId = 2, TourName = "Goa Inactive Tour", Place = "Goa", Days = 5, Price = 15000, Locations = "Goa Beach", TourInfo = "Beautiful beaches", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("Goa");

            // Assert
            var list = new List<Tour>(result);
            Assert.Single(list);
            Assert.Equal("Goa Active Tour", list[0].TourName);
        }
    }
}
