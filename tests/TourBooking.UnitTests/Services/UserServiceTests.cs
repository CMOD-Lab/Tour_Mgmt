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
    /// Comprehensive unit tests for UserService.
    /// </summary>
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _service = new UserService(_mockRepository.Object, _mockLogger.Object);
        }

        // GetAllAsync tests
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<UserInfo>
            {
                new UserInfo { Email = "user1@test.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" },
                new UserInfo { Email = "user2@test.com", FirstName = "Jane", LastName = "Smith", Gender = "Female", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" }
            };
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(u => u.Email == "user1@test.com");
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UserInfo>());

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

        // GetByEmailAsync tests
        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ShouldReturnUser()
        {
            // Arrange
            var user = new UserInfo { Email = "user@test.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
            _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmailAsync("user@test.com");

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("user@test.com");
            result.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync("nonexistent@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserInfo?)null);

            // Act
            var result = await _service.GetByEmailAsync("nonexistent@test.com");

            // Assert
            result.Should().BeNull();
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

        // RegisterAsync tests
        [Fact]
        public async Task RegisterAsync_WithNewEmail_ShouldSucceed()
        {
            // Arrange
            var user = new UserInfo
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "password123",
                Dob = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            _mockRepository.Setup(r => r.ExistsAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.RegisterAsync(user);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldThrowDuplicateEntityException()
        {
            // Arrange
            var user = new UserInfo { Email = "existing@example.com", FirstName = "Jane", LastName = "Doe", Gender = "Female", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };

            _mockRepository.Setup(r => r.ExistsAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await _service.Invoking(s => s.RegisterAsync(user))
                .Should().ThrowAsync<DuplicateEntityException>();
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldNotCallAddAsync()
        {
            // Arrange
            var user = new UserInfo { Email = "existing@example.com", FirstName = "Jane", LastName = "Doe", Gender = "Female", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
            _mockRepository.Setup(r => r.ExistsAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            try { await _service.RegisterAsync(user); } catch { }

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            var user = new UserInfo { Email = "new@test.com", FirstName = "New", LastName = "User", Gender = "Male", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
            _mockRepository.Setup(r => r.ExistsAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.RegisterAsync(user))
                .Should().ThrowAsync<Exception>();
        }

        // UpdateAsync tests
        [Fact]
        public async Task UpdateAsync_WithExistingUser_ShouldReturnTrue()
        {
            // Arrange
            var user = new UserInfo { Email = "user@test.com", FirstName = "Updated", LastName = "Name", Gender = "Male", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
            _mockRepository.Setup(r => r.ExistsAsync("user@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(user);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistentUser_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = new UserInfo { Email = "nonexistent@test.com", FirstName = "Non", LastName = "Existent", Gender = "Male", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
            _mockRepository.Setup(r => r.ExistsAsync("nonexistent@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _service.Invoking(s => s.UpdateAsync(user))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            var user = new UserInfo { Email = "user@test.com", FirstName = "User", LastName = "Name", Gender = "Male", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
            _mockRepository.Setup(r => r.ExistsAsync("user@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.UpdateAsync(user))
                .Should().ThrowAsync<Exception>();
        }

        // DeleteAsync tests
        [Fact]
        public async Task DeleteAsync_WithExistingEmail_ShouldReturnTrue()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync("user@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync("user@test.com", It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync("user@test.com");

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync("user@test.com", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentEmail_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync("nonexistent@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync("nonexistent@test.com"))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallDelete_WhenUserDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync("nonexistent@test.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            try { await _service.DeleteAsync("nonexistent@test.com"); } catch { }

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        // LoginAsync tests
        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var user = new UserInfo { Email = "user@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "password", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };

            _mockRepository.Setup(r => r.ValidateCredentialsAsync("user@example.com", "password", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync("user@example.com", "password");

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("user@example.com");
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.ValidateCredentialsAsync("user@example.com", "wrongpassword", It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserInfo?)null);

            // Act
            var result = await _service.LoginAsync("user@example.com", "wrongpassword");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await _service.Invoking(s => s.LoginAsync("user@test.com", "pass"))
                .Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task LoginAsync_ShouldCallValidateCredentials_WithCorrectParameters()
        {
            // Arrange
            var email = "user@test.com";
            var password = "mypassword";
            _mockRepository.Setup(r => r.ValidateCredentialsAsync(email, password, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserInfo?)null);

            // Act
            await _service.LoginAsync(email, password);

            // Assert
            _mockRepository.Verify(r => r.ValidateCredentialsAsync(email, password, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
