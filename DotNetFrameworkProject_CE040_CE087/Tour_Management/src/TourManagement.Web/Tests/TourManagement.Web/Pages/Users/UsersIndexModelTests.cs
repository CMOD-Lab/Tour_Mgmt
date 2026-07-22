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

namespace TourManagement.Web.Tests.Pages.Users
{
    public class UsersIndexModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<IndexModel>> _loggerMock;
        private readonly IndexModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersIndexModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<IndexModel>>();
            _pageModel = new IndexModel(_userServiceMock.Object, _loggerMock.Object);

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
            var result = await _pageModel.OnGetAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Admin/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenAdminAndUsersExist_ReturnsPageWithUsers()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            var users = new List<UserDto>
            {
                new UserDto { Id = 1, Email = "user1@example.com" },
                new UserDto { Id = 2, Email = "user2@example.com" }
            };
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(users);

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(2, System.Linq.Enumerable.Count(_pageModel.Users));
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.True(_pageModel.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public void IndexModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Empty(_pageModel.Users);
        }
    }
}
