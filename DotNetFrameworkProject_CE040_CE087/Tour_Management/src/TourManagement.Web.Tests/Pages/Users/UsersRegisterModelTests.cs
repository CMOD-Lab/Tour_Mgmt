using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.Pages.Users;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.Pages.Users
{
    public class UsersRegisterModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<RegisterModel>> _loggerMock;
        private readonly RegisterModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersRegisterModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<RegisterModel>>();
            _pageModel = new RegisterModel(_userServiceMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public void OnGet_WhenAlreadyLoggedIn_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");

            // Act
            var result = _pageModel.OnGet();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirect.PageName);
        }

        [Fact]
        public void OnGet_WhenNotLoggedIn_ReturnsPage()
        {
            // Act
            var result = _pageModel.OnGet();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_CreatesUserAndRedirects()
        {
            // Arrange
            _pageModel.Input = new UserRegisterViewModel
            {
                Email = "newuser@example.com",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            var createdUser = new UserDto { Id = 1, Email = "newuser@example.com" };
            _userServiceMock.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), default)).ReturnsAsync(createdUser);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Login", redirect.PageName);
            _userServiceMock.Verify(s => s.CreateAsync(It.IsAny<UserCreateDto>(), default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenInvalidOperationException_ReturnsPage()
        {
            // Arrange
            _pageModel.Input = new UserRegisterViewModel
            {
                Email = "existing@example.com",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            _userServiceMock.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), default))
                .ThrowsAsync(new InvalidOperationException("Email already exists"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(_pageModel.ModelState.IsValid);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _pageModel.Input = new UserRegisterViewModel
            {
                Email = "user@example.com",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            _userServiceMock.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void RegisterModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.NotNull(_pageModel.Input);
        }
    }
}
