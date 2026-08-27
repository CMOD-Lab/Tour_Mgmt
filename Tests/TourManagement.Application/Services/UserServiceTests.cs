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
    /// Comprehensive unit tests for UserService.
    /// </summary>
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserService>>();

            var config = new MapperConfiguration(MappingProfile.Configure, NullLoggerFactory.Instance);
            _mapper = config.CreateMapper();

            _service = new UserService(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        // ==================== GetAllAsync Tests ====================

        [Fact]
        public async Task GetAllAsync_WhenUsersExist_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<UserInfo>
            {
                new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", IsActive = true },
                new UserInfo { Email = "jane@example.com", FirstName = "Jane", LastName = "Smith", Gender = "Female", IsActive = true }
            };
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            var list = new List<UserDto>(result);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoUsers_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserInfo>());

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
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserInfo>());

            // Act
            await _service.GetAllAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ==================== GetByEmailAsync Tests ====================

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnsUserDto()
        {
            // Arrange
            var user = new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", IsActive = true };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmailAsync("john@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result!.Email);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetByEmailAsync_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync("nobody@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((UserInfo?)null);

            // Act
            var result = await _service.GetByEmailAsync("nobody@example.com");

            // Assert
            Assert.Null(result);
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

        [Fact]
        public async Task GetByEmailAsync_MapsAllProperties()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);
            var user = new UserInfo
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = dob,
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra",
                IsActive = true
            };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmailAsync("john@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result!.Email);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("Male", result.Gender);
            Assert.Equal(dob, result.DateOfBirth);
            Assert.Equal("123 Main St", result.Street);
            Assert.Equal("Mumbai", result.City);
            Assert.Equal("Maharashtra", result.State);
        }

        // ==================== RegisterAsync Tests ====================

        [Fact]
        public async Task RegisterAsync_WithNewEmail_CreatesUser()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User",
                Gender = "Male",
                Password = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            var createdUser = new UserInfo
            {
                Email = createDto.Email,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                IsActive = true
            };
            _mockRepository.Setup(r => r.ExistsAsync("newuser@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdUser);

            // Act
            var result = await _service.RegisterAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newuser@example.com", result.Email);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ThrowsDuplicateEntityException()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Email = "existing@example.com",
                FirstName = "Existing",
                LastName = "User",
                Gender = "Male",
                Password = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            _mockRepository.Setup(r => r.ExistsAsync("existing@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.RegisterAsync(createDto));
        }

        [Fact]
        public async Task RegisterAsync_CallsRepositoryAddOnce()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User",
                Gender = "Male",
                Password = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            var createdUser = new UserInfo { Email = createDto.Email, FirstName = createDto.FirstName, LastName = createDto.LastName };
            _mockRepository.Setup(r => r.ExistsAsync("newuser@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdUser);

            // Act
            await _service.RegisterAsync(createDto);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_HashesPassword()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User",
                Gender = "Male",
                Password = "PlainPassword123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            UserInfo? capturedUser = null;
            var createdUser = new UserInfo { Email = createDto.Email, FirstName = createDto.FirstName, LastName = createDto.LastName };
            _mockRepository.Setup(r => r.ExistsAsync("newuser@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .Callback<UserInfo, CancellationToken>((u, _) => capturedUser = u)
                .ReturnsAsync(createdUser);

            // Act
            await _service.RegisterAsync(createDto);

            // Assert
            Assert.NotNull(capturedUser);
            Assert.NotEqual("PlainPassword123!", capturedUser!.Password);
            Assert.True(BCrypt.Net.BCrypt.Verify("PlainPassword123!", capturedUser.Password));
        }

        [Fact]
        public async Task RegisterAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User",
                Gender = "Male",
                Password = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            _mockRepository.Setup(r => r.ExistsAsync("newuser@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.RegisterAsync(createDto));
        }

        // ==================== UpdateAsync Tests ====================

        [Fact]
        public async Task UpdateAsync_WithValidEmail_UpdatesUser()
        {
            // Arrange
            var existingUser = new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male", IsActive = true };
            var updateDto = new UserUpdateDto { FirstName = "Johnny", LastName = "Doe", Gender = "Male", City = "Delhi", Street = "456 New St", State = "Delhi", IsActive = true };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync("john@example.com", updateDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistentEmail_ThrowsNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync("nobody@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((UserInfo?)null);
            var updateDto = new UserUpdateDto { FirstName = "Test", LastName = "User", Gender = "Male", City = "Test", Street = "Test", State = "Test" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync("nobody@example.com", updateDto));
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var existingUser = new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", Gender = "Male" };
            var updateDto = new UserUpdateDto { FirstName = "Johnny", LastName = "Doe", Gender = "Male", City = "Delhi", Street = "456 New St", State = "Delhi" };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync("john@example.com", updateDto));
        }

        // ==================== DeleteAsync Tests ====================

        [Fact]
        public async Task DeleteAsync_WithValidEmail_DeletesUser()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync("john@example.com", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync("john@example.com");

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync("john@example.com", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentEmail_ThrowsNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync("nobody@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync("nobody@example.com"));
        }

        [Fact]
        public async Task DeleteAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync("john@example.com", It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync("john@example.com"));
        }

        // ==================== LoginAsync Tests ====================

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsUserDto()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123!");
            var user = new UserInfo
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = hashedPassword,
                IsActive = true
            };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync("john@example.com", "Password123!");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result!.Email);
        }

        [Fact]
        public async Task LoginAsync_WithNonExistentEmail_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync("nobody@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((UserInfo?)null);

            // Act
            var result = await _service.LoginAsync("nobody@example.com", "Password123!");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!");
            var user = new UserInfo
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = hashedPassword,
                IsActive = true
            };
            _mockRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync("john@example.com", "WrongPassword!");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.LoginAsync("test@example.com", "Password123!"));
        }

        // ==================== SearchAsync Tests ====================

        [Fact]
        public async Task SearchAsync_WithMatchingTerm_ReturnsUsers()
        {
            // Arrange
            var users = new List<UserInfo>
            {
                new UserInfo { Email = "john@example.com", FirstName = "John", LastName = "Doe", IsActive = true }
            };
            _mockRepository.Setup(r => r.SearchAsync("John", It.IsAny<CancellationToken>())).ReturnsAsync(users);

            // Act
            var result = await _service.SearchAsync("John");

            // Assert
            var list = new List<UserDto>(result);
            Assert.Single(list);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatch_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserInfo>());

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
            _mockRepository.Setup(r => r.SearchAsync("Mumbai", It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserInfo>());

            // Act
            await _service.SearchAsync("Mumbai");

            // Assert
            _mockRepository.Verify(r => r.SearchAsync("Mumbai", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
