using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using FluentAssertions;
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
    public async Task RegisterAsync_WithNewEmail_ShouldCreateUser()
    {
        // Arrange
        var user = new UserInfo
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Role = "User",
            CreatedBy = "system"
        };
        _mockRepository.Setup(r => r.ExistsByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserInfo u, CancellationToken _) => { u.Id = 1; return u; });

        // Act
        var result = await _service.RegisterAsync(user, "password123");

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowDuplicateEntityException()
    {
        // Arrange
        var user = new UserInfo
        {
            Email = "existing@example.com",
            FirstName = "John",
            LastName = "Doe",
            Role = "User",
            CreatedBy = "system"
        };
        _mockRepository.Setup(r => r.ExistsByEmailAsync("existing@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await _service.Invoking(s => s.RegisterAsync(user, "password123"))
            .Should().ThrowAsync<DuplicateEntityException>();
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange - create a user with a known password hash
        var password = "password123";
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        var passwordHash = Convert.ToBase64String(hash);

        var user = new UserInfo
        {
            Id = 1,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = passwordHash,
            IsActive = true,
            Role = "User",
            CreatedBy = "system"
        };
        _mockRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("test@example.com", password);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var user = new UserInfo
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = "wronghash",
            IsActive = true,
            Role = "User",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "system"
        };
        _mockRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("test@example.com", "wrongpassword");

        // Assert
        result.Should().BeNull();
    }
}
