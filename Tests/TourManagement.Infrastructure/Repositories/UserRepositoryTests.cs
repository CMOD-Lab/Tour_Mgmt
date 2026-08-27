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
    /// Unit tests for UserRepository using in-memory database.
    /// </summary>
    public class UserRepositoryTests : IDisposable
    {
        private readonly TourManagementDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TourManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TourManagementDbContext(options);
            _repository = new UserRepository(_context, NullLogger<UserRepository>.Instance);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // ==================== GetAllAsync Tests ====================

        [Fact]
        public async Task GetAllAsync_WhenActiveUsersExist_ReturnsActiveUsers()
        {
            // Arrange
            _context.UserInfos.AddRange(
                new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash1", IsActive = true },
                new UserInfo { Email = "jane@example.com", FirstName = "Jane", LastName = "Smith", Gender = "Female", Password = "hash2", IsActive = true },
                new UserInfo { Email = "inactive@example.com", FirstName = "Inactive", LastName = "User", Gender = "Male", Password = "hash3", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var list = new List<UserInfo>(result);
            Assert.Equal(2, list.Count);
            Assert.All(list, u => Assert.True(u.IsActive));
        }

        [Fact]
        public async Task GetAllAsync_WhenNoActiveUsers_ReturnsEmptyList()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "inactive@example.com", FirstName = "Inactive", LastName = "User", Gender = "Male", Password = "hash", IsActive = false });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsUsersOrderedByLastNameThenFirstName()
        {
            // Arrange
            _context.UserInfos.AddRange(
                new UserInfo { Email = "zach@example.com", FirstName = "Zach", LastName = "Brown", Gender = "Male", Password = "hash1", IsActive = true },
                new UserInfo { Email = "alice@example.com", FirstName = "Alice", LastName = "Adams", Gender = "Female", Password = "hash2", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var list = new List<UserInfo>(result);
            Assert.Equal("Adams", list[0].LastName);
            Assert.Equal("Brown", list[1].LastName);
        }

        // ==================== GetByEmailAsync Tests ====================

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnsUser()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync("john@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result!.Email);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetByEmailAsync_WithInvalidEmail_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByEmailAsync("nobody@example.com");

            // Assert
            Assert.Null(result);
        }

        // ==================== AddAsync Tests ====================

        [Fact]
        public async Task AddAsync_WithValidUser_AddsUserToDatabase()
        {
            // Arrange
            var user = new UserInfo
            {
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User",
                Gender = "Male",
                Password = "hashedpassword",
                IsActive = true
            };

            // Act
            var result = await _repository.AddAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newuser@example.com", result.Email);
            Assert.Equal(1, await _context.UserInfos.CountAsync());
        }

        // ==================== UpdateAsync Tests ====================

        [Fact]
        public async Task UpdateAsync_WithValidUser_UpdatesUserInDatabase()
        {
            // Arrange
            var user = new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true };
            _context.UserInfos.Add(user);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var updatedUser = new UserInfo { Email = "john@example.com", FirstName = "Johnny", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true };

            // Act
            await _repository.UpdateAsync(updatedUser);

            // Assert
            var dbUser = await _context.UserInfos.FindAsync("john@example.com");
            Assert.NotNull(dbUser);
            Assert.Equal("Johnny", dbUser!.FirstName);
        }

        // ==================== DeleteAsync Tests ====================

        [Fact]
        public async Task DeleteAsync_WithValidEmail_SetsIsActiveToFalse()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            // Act
            await _repository.DeleteAsync("john@example.com");

            // Assert
            var dbUser = await _context.UserInfos.FindAsync("john@example.com");
            Assert.NotNull(dbUser);
            Assert.False(dbUser!.IsActive);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentEmail_DoesNotThrow()
        {
            // Act & Assert (should not throw)
            await _repository.DeleteAsync("nobody@example.com");
        }

        // ==================== ExistsAsync Tests ====================

        [Fact]
        public async Task ExistsAsync_WithExistingEmail_ReturnsTrue()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync("john@example.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistentEmail_ReturnsFalse()
        {
            // Act
            var result = await _repository.ExistsAsync("nobody@example.com");

            // Assert
            Assert.False(result);
        }

        // ==================== ValidateCredentialsAsync Tests ====================

        [Fact]
        public async Task ValidateCredentialsAsync_WithValidActiveUser_ReturnsUser()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ValidateCredentialsAsync("john@example.com", "anypassword");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result!.Email);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_WithInactiveUser_ReturnsNull()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "inactive@example.com", FirstName = "Inactive", LastName = "User", Gender = "Male", Password = "hash", IsActive = false });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ValidateCredentialsAsync("inactive@example.com", "anypassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_WithNonExistentEmail_ReturnsNull()
        {
            // Act
            var result = await _repository.ValidateCredentialsAsync("nobody@example.com", "anypassword");

            // Assert
            Assert.Null(result);
        }

        // ==================== SearchAsync Tests ====================

        [Fact]
        public async Task SearchAsync_WithMatchingFirstName_ReturnsUsers()
        {
            // Arrange
            _context.UserInfos.AddRange(
                new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash1", IsActive = true },
                new UserInfo { Email = "jane@example.com", FirstName = "Jane", LastName = "Smith", Gender = "Female", Password = "hash2", IsActive = true }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("John");

            // Assert
            var list = new List<UserInfo>(result);
            Assert.Single(list);
            Assert.Equal("john@example.com", list[0].Email);
        }

        [Fact]
        public async Task SearchAsync_WithMatchingEmail_ReturnsUsers()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("john@example");

            // Assert
            var list = new List<UserInfo>(result);
            Assert.Single(list);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatch_ReturnsEmptyList()
        {
            // Arrange
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
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
            _context.UserInfos.Add(new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash", IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("john");

            // Assert
            var list = new List<UserInfo>(result);
            Assert.Single(list);
        }

        [Fact]
        public async Task SearchAsync_ExcludesInactiveUsers()
        {
            // Arrange
            _context.UserInfos.AddRange(
                new UserInfo { Email = "john.active@example.com", FirstName = "John", LastName = "Active", Gender = "Male", Password = "hash1", IsActive = true },
                new UserInfo { Email = "john.inactive@example.com", FirstName = "John", LastName = "Inactive", Gender = "Male", Password = "hash2", IsActive = false }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SearchAsync("John");

            // Assert
            var list = new List<UserInfo>(result);
            Assert.Single(list);
            Assert.Equal("john.active@example.com", list[0].Email);
        }
    }
}
