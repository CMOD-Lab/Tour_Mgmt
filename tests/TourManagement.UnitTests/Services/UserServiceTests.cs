using Xunit;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.DTOs;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
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
        var createDto = new UserCreateDto
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
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.RegisterAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<UserInfo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowDuplicateEntityException()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            Email = "existing@example.com",
            FirstName = "Jane",
            LastName = "Doe",
            Gender = "Female",
            Password = "password123",
            Dob = new DateTime(1992, 5, 15),
            Street = "456 Oak Ave",
            City = "Delhi",
            State = "Delhi"
        };

        _mockRepository.Setup(r => r.ExistsAsync("existing@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await _service.Invoking(s => s.RegisterAsync(createDto))
            .Should().ThrowAsync<DuplicateEntityException>();
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        var user = new UserInfo
        {
            Email = "user@example.com",
            FirstName = "Alice",
            LastName = "Smith",
            Gender = "Female",
            Password = "correctpassword",
            Dob = new DateTime(1988, 3, 20),
            Street = "789 Pine Rd",
            City = "Bangalore",
            State = "Karnataka"
        };

        _mockRepository.Setup(r => r.AuthenticateAsync("user@example.com", "correctpassword", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("user@example.com", "correctpassword");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidCredentials_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.AuthenticateAsync("user@example.com", "wrongpassword", It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserInfo?)null);

        // Act
        var result = await _service.AuthenticateAsync("user@example.com", "wrongpassword");

        // Assert
        result.Should().BeNull();
    }
}
