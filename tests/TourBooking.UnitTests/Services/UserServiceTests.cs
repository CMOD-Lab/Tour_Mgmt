using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TourBooking.Application.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Exceptions;
using TourBooking.Domain.Interfaces.Repositories;

namespace TourBooking.UnitTests.Services;

/// <summary>
/// Unit tests for UserService.
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _service;

    /// <summary>Initializes a new instance of the <see cref="UserServiceTests"/> class.</summary>
    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _service = new UserService(_mockRepository.Object, _mockLogger.Object);
    }

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
}
