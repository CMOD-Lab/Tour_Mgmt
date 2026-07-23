using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Domain.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;
using Xunit;

namespace Tour_Management.UnitTests.Services;

/// <summary>Unit tests for UserService.</summary>
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

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new UserService(_mockRepository.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<UserInfo>
        {
            new() { UserId = 1, Email = "user1@test.com", FirstName = "John", LastName = "Doe", PasswordHash = "hash1", IsActive = true },
            new() { UserId = 2, Email = "user2@test.com", FirstName = "Jane", LastName = "Smith", PasswordHash = "hash2", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ShouldThrowDuplicateEntityException()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            Email = "existing@test.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password123"
        };
        _mockRepository.Setup(r => r.EmailExistsAsync("existing@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task ValidateLoginAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        var loginDto = new UserLoginDto { Email = "user@test.com", Password = "password123" };

        // Compute the expected hash
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes("password123" + "TourMgmt_Salt_2024");
        var hash = sha256.ComputeHash(bytes);
        var expectedHash = Convert.ToBase64String(hash);

        var user = new UserInfo
        {
            UserId = 1,
            Email = "user@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = expectedHash,
            IsActive = true
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.ValidateLoginAsync(loginDto);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var loginDto = new UserLoginDto { Email = "user@test.com", Password = "wrongpassword" };
        var user = new UserInfo
        {
            UserId = 1,
            Email = "user@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "differenthash",
            IsActive = true
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.ValidateLoginAsync(loginDto);

        // Assert
        result.Should().BeNull();
    }
}
