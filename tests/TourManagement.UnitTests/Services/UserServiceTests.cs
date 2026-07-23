using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using Xunit;

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
            new UserInfo { Email = "user1@test.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "hash1", Dob = new DateTime(1990, 1, 1), Street = "123 Main St", City = "Mumbai", State = "Maharashtra" },
            new UserInfo { Email = "user2@test.com", FirstName = "Jane", LastName = "Smith", Gender = "Female", Password = "hash2", Dob = new DateTime(1992, 5, 15), Street = "456 Oak Ave", City = "Delhi", State = "Delhi" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Email.Should().Be("user1@test.com");
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
            Gender = "Male",
            Password = "password123",
            Dob = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Mumbai",
            State = "Maharashtra"
        };

        _mockRepository.Setup(r => r.ExistsAsync("existing@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task ValidateLoginAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new UserInfo
        {
            Email = "user@test.com",
            FirstName = "John",
            LastName = "Doe",
            Gender = "Male",
            Password = hashedPassword,
            Dob = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Mumbai",
            State = "Maharashtra"
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.ValidateLoginAsync("user@test.com", "password123");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        var user = new UserInfo
        {
            Email = "user@test.com",
            FirstName = "John",
            LastName = "Doe",
            Gender = "Male",
            Password = hashedPassword,
            Dob = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Mumbai",
            State = "Maharashtra"
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.ValidateLoginAsync("user@test.com", "wrongpassword");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithValidEmail_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync("user@test.com", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync("user@test.com");

        // Assert
        result.Should().BeTrue();
    }
}
