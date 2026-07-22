using Xunit;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.DTOs;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.UnitTests.Services;

/// <summary>
/// Unit tests for UserService.
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _service;

    /// <summary>
    /// Initializes test dependencies.
    /// </summary>
    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new UserService(_mockRepository.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_WithNewEmail_ShouldCreateUser()
    {
        // Arrange
        var dto = new UserCreateDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password123"
        };

        _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var createdUser = new User { Id = 1, Email = dto.Email, FirstName = dto.FirstName, LastName = dto.LastName, PasswordHash = "hashed", IsActive = true };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdUser);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var dto = new UserCreateDto
        {
            Email = "existing@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password123"
        };

        _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        // Hash "password123" using SHA256
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes("password123");
        var hash = sha256.ComputeHash(bytes);
        var hashedPassword = Convert.ToBase64String(hash);

        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = hashedPassword,
            IsActive = true
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("test@example.com", "password123");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = "wronghash",
            IsActive = true
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("test@example.com", "wrongpassword");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, Email = "user1@example.com", FirstName = "John", LastName = "Doe", PasswordHash = "hash1", IsActive = true },
            new() { Id = 2, Email = "user2@example.com", FirstName = "Jane", LastName = "Smith", PasswordHash = "hash2", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }
}
