using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.Pages.Admin;

namespace TourManagement.Web.Tests.Pages.Admin
{
    public class AdminLoginModelTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<LoginModel>> _loggerMock;
        private readonly LoginModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public AdminLoginModelTests()
        {
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<LoginModel>>();
            _pageModel = new LoginModel(_configMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public void OnGet_WhenAlreadyAdmin_RedirectsToDashboard()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");

            // Act
            var result = _pageModel.OnGet();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Dashboard", redirect.PageName);
        }

        [Fact]
        public void OnGet_WhenNotAdmin_ReturnsPage()
        {
            // Act
            var result = _pageModel.OnGet();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void OnPost_WithValidAdminCredentials_SetsSessionAndRedirects()
        {
            // Arrange
            _configMock.Setup(c => c["AdminCredentials:Email"]).Returns("admin@gmail.com");
            _configMock.Setup(c => c["AdminCredentials:Password"]).Returns("admin");
            _pageModel.Email = "admin@gmail.com";
            _pageModel.Password = "admin";

            // Act
            var result = _pageModel.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Dashboard", redirect.PageName);
            Assert.Equal("true", _httpContext.Session.GetString("IsAdmin"));
            Assert.Equal("admin@gmail.com", _httpContext.Session.GetString("UserEmail"));
        }

        [Fact]
        public void OnPost_WithInvalidCredentials_ReturnsPage()
        {
            // Arrange
            _configMock.Setup(c => c["AdminCredentials:Email"]).Returns("admin@gmail.com");
            _configMock.Setup(c => c["AdminCredentials:Password"]).Returns("admin");
            _pageModel.Email = "wrong@gmail.com";
            _pageModel.Password = "wrongpassword";

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(_pageModel.ModelState.IsValid);
        }

        [Fact]
        public void OnPost_WithDefaultCredentials_WhenConfigReturnsNull_UsesDefaults()
        {
            // Arrange - config returns null, so defaults are used
            _configMock.Setup(c => c["AdminCredentials:Email"]).Returns((string?)null);
            _configMock.Setup(c => c["AdminCredentials:Password"]).Returns((string?)null);
            _pageModel.Email = "admin@gmail.com";
            _pageModel.Password = "admin";

            // Act
            var result = _pageModel.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Dashboard", redirect.PageName);
        }

        [Fact]
        public void OnPost_WithWrongPassword_ReturnsPage()
        {
            // Arrange
            _configMock.Setup(c => c["AdminCredentials:Email"]).Returns("admin@gmail.com");
            _configMock.Setup(c => c["AdminCredentials:Password"]).Returns("correctpassword");
            _pageModel.Email = "admin@gmail.com";
            _pageModel.Password = "wrongpassword";

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void AdminLoginModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Equal(string.Empty, _pageModel.Email);
            Assert.Equal(string.Empty, _pageModel.Password);
        }
    }
}
