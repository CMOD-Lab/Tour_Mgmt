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
    public class UsersLoginModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<LoginModel>> _loggerMock;
        private readonly LoginModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersLoginModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<LoginModel>>();
            _pageModel = new LoginModel(_userServiceMock.Object, _loggerMock.Object);

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
        public async Task OnPostAsync_WithValidCredentials_SetsSessionAndRedirects()
        {
            // Arrange
            _pageModel.Input = new UserLoginViewModel
            {
                Email = "user@example.com",
                Password = "password123"
            };
            var user = new UserDto
            {
                Id = 1,
                Email = "user@example.com",
                FirstName = "Alice",
                LastName = "Wonder",
                IsAdmin = false
            };
            _userServiceMock.Setup(s => s.AuthenticateAsync("user@example.com", "password123", default))
                .ReturnsAsync(user);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirect.PageName);
            Assert.Equal("user@example.com", _httpContext.Session.GetString("UserEmail"));
            Assert.Equal("false", _httpContext.Session.GetString("IsAdmin"));
        }

        [Fact]
        public async Task OnPostAsync_WithAdminCredentials_SetsAdminSession()
        {
            // Arrange
            _pageModel.Input = new UserLoginViewModel
            {
                Email = "admin@example.com",
                Password = "adminpass"
            };
            var user = new UserDto
            {
                Id = 2,
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                IsAdmin = true
            };
            _userServiceMock.Setup(s => s.AuthenticateAsync("admin@example.com", "adminpass", default))
                .ReturnsAsync(user);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirect.PageName);
            Assert.Equal("true", _httpContext.Session.GetString("IsAdmin"));
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidCredentials_ReturnsPage()
        {
            // Arrange
            _pageModel.Input = new UserLoginViewModel
            {
                Email = "user@example.com",
                Password = "wrongpassword"
            };
            _userServiceMock.Setup(s => s.AuthenticateAsync("user@example.com", "wrongpassword", default))
                .ReturnsAsync((UserDto?)null);

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
            _pageModel.Input = new UserLoginViewModel
            {
                Email = "user@example.com",
                Password = "password123"
            };
            _userServiceMock.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void LoginModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.NotNull(_pageModel.Input);
        }
    }
}
