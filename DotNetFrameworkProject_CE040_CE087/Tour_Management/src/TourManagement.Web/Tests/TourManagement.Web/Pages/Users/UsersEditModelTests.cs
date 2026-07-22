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
using TourManagement.Domain.Exceptions;
using TourManagement.Web.Pages.Users;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.Pages.Users
{
    public class UsersEditModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<EditModel>> _loggerMock;
        private readonly EditModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersEditModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<EditModel>>();
            _pageModel = new EditModel(_userServiceMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WhenNotLoggedIn_RedirectsToLogin()
        {
            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenUserEditingOwnProfile_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            var user = new UserDto
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Wonder",
                IsActive = true
            };
            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(user);

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("Alice", _pageModel.Input.FirstName);
        }

        [Fact]
        public async Task OnGetAsync_WhenAdminEditingOtherUser_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 99);
            _httpContext.Session.SetString("IsAdmin", "true");
            var user = new UserDto { Id = 5, FirstName = "Bob", LastName = "Builder", IsActive = true };
            _userServiceMock.Setup(s => s.GetByIdAsync(5, default)).ReturnsAsync(user);

            // Act
            var result = await _pageModel.OnGetAsync(5);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("Bob", _pageModel.Input.FirstName);
        }

        [Fact]
        public async Task OnGetAsync_WhenUserNotFound_RedirectsToProfile()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync((UserDto?)null);

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Profile", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_RedirectsToProfile()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Profile", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotLoggedIn_RedirectsToLogin()
        {
            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Login", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenValidAndAuthorized_UpdatesAndRedirects()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            _pageModel.Input = new UserEditViewModel
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Updated",
                IsActive = true
            };
            _userServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<UserUpdateDto>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Profile", redirect.PageName);
            _userServiceMock.Verify(s => s.UpdateAsync(1, It.IsAny<UserUpdateDto>(), default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotFoundException_RedirectsToProfile()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            _pageModel.Input = new UserEditViewModel { Id = 1, FirstName = "Alice", LastName = "Wonder" };
            _userServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<UserUpdateDto>(), default))
                .ThrowsAsync(new NotFoundException("User", 1));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Profile", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            _pageModel.Input = new UserEditViewModel { Id = 1, FirstName = "Alice", LastName = "Wonder" };
            _userServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<UserUpdateDto>(), default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
