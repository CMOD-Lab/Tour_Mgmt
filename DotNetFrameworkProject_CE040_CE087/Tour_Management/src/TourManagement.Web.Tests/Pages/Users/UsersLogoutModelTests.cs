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
using TourManagement.Web.Pages.Users;

namespace TourManagement.Web.Tests.Pages.Users
{
    public class UsersLogoutModelTests
    {
        private readonly Mock<ILogger<LogoutModel>> _loggerMock;
        private readonly LogoutModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersLogoutModelTests()
        {
            _loggerMock = new Mock<ILogger<LogoutModel>>();
            _pageModel = new LogoutModel(_loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public void OnGet_ClearsSessionAndRedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _httpContext.Session.SetString("IsAdmin", "false");

            // Act
            var result = _pageModel.OnGet();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirect.PageName);
            Assert.Null(_httpContext.Session.GetString("UserEmail"));
        }

        [Fact]
        public void OnGet_WhenNotLoggedIn_StillRedirectsToIndex()
        {
            // Act
            var result = _pageModel.OnGet();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirect.PageName);
        }

        [Fact]
        public void OnGet_SetsTempDataSuccessMessage()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.True(_pageModel.TempData.ContainsKey("SuccessMessage"));
        }

        [Fact]
        public void LogoutModel_Constructor_InitializesCorrectly()
        {
            // Assert
            Assert.NotNull(_pageModel);
        }
    }
}
