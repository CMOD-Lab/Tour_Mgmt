using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
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
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _mockLogger = new Mock<ILogger<UserService>>();
        _service = new UserService(_mockRepository.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_WithNewEmail_ShouldRegisterUser()
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
        _mockRepository.Setup(r => r.ExistsAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.RegisterUserAsync(user);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingEmail_ShouldThrowValidationException()
    {
        // Arrange
        var user = new UserInfo { Email = "existing@example.com", FirstName = "Jane", LastName = "Doe", Gender = "Female", Password = "pass", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
        _mockRepository.Setup(r => r.ExistsAsync("existing@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<Domain.Exceptions.ValidationException>(() => _service.RegisterUserAsync(user));
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        var user = new UserInfo { Email = "user@example.com", FirstName = "Test", LastName = "User", Gender = "Male", Password = "pass123", Dob = DateTime.Now, Street = "St", City = "City", State = "State" };
        _mockRepository.Setup(r => r.AuthenticateAsync("user@example.com", "pass123", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("user@example.com", "pass123");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidCredentials_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.AuthenticateAsync("user@example.com", "wrongpass", It.IsAny<CancellationToken>())).ReturnsAsync((UserInfo?)null);

        // Act
        var result = await _service.AuthenticateAsync("user@example.com", "wrongpass");

        // Assert
        result.Should().BeNull();
    }
}
