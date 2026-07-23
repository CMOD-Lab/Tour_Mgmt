using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
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
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly IMapper _mapper;
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
            new UserInfo { Email = "user1@test.com", FirstName = "John", LastName = "Doe", Gender = "Male", Password = "pass1", Dob = new DateTime(1990, 1, 1), Street = "123 St", City = "Mumbai", State = "MH" },
            new UserInfo { Email = "user2@test.com", FirstName = "Jane", LastName = "Smith", Gender = "Female", Password = "pass2", Dob = new DateTime(1992, 5, 15), Street = "456 Ave", City = "Delhi", State = "DL" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_ShouldReturnTrue()
    {
        // Arrange
        var user = new UserInfo { Email = "new@test.com", FirstName = "New", LastName = "User", Gender = "Male", Password = "password", Dob = new DateTime(1990, 1, 1), Street = "St", City = "City", State = "State" };
        _mockRepository.Setup(r => r.ExistsAsync("new@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.RegisterAsync(user);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowDuplicateEntityException()
    {
        // Arrange
        var user = new UserInfo { Email = "existing@test.com", FirstName = "Existing", LastName = "User", Gender = "Male", Password = "password", Dob = new DateTime(1990, 1, 1), Street = "St", City = "City", State = "State" };
        _mockRepository.Setup(r => r.ExistsAsync("existing@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await _service.Invoking(s => s.RegisterAsync(user))
            .Should().ThrowAsync<DuplicateEntityException>();
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        var user = new UserInfo { Email = "user@test.com", FirstName = "Test", LastName = "User", Gender = "Male", Password = "password", Dob = new DateTime(1990, 1, 1), Street = "St", City = "City", State = "State" };
        _mockRepository.Setup(r => r.ValidateCredentialsAsync("user@test.com", "password", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("user@test.com", "password");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidCredentials_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.ValidateCredentialsAsync("user@test.com", "wrongpassword", It.IsAny<CancellationToken>())).ReturnsAsync((UserInfo?)null);

        // Act
        var result = await _service.AuthenticateAsync("user@test.com", "wrongpassword");

        // Assert
        result.Should().BeNull();
    }
}
