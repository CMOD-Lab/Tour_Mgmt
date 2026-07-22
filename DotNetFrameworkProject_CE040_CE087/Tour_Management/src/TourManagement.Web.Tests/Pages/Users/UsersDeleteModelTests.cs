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

namespace TourManagement.Web.Tests.Pages.Users
{
    public class UsersDeleteModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<DeleteModel>> _loggerMock;
        private readonly DeleteModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersDeleteModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<DeleteModel>>();
            _pageModel = new DeleteModel(_userServiceMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WhenNotAdmin_RedirectsToAdminLogin()
        {
            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Admin/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenAdminAndUserExists_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            var user = new UserDto { Id = 1, Email = "user@example.com" };
            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(user);

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(_pageModel.User);
        }

        [Fact]
        public async Task OnGetAsync_WhenUserNotFound_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _userServiceMock.Setup(s => s.GetByIdAsync(999, default)).ReturnsAsync((UserDto?)null);

            // Act
            var result = await _pageModel.OnGetAsync(999);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotAdmin_RedirectsToAdminLogin()
        {
            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Admin/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenAdminAndUserExists_DeletesAndRedirects()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _userServiceMock.Setup(s => s.DeleteAsync(1, default)).Returns(Task.CompletedTask);

            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
            _userServiceMock.Verify(s => s.DeleteAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotFoundException_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _userServiceMock.Setup(s => s.DeleteAsync(1, default)).ThrowsAsync(new NotFoundException("User", 1));

            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _userServiceMock.Setup(s => s.DeleteAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }
    }
}
