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
    public class UsersProfileModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ILogger<ProfileModel>> _loggerMock;
        private readonly ProfileModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public UsersProfileModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _bookingServiceMock = new Mock<IBookingService>();
            _loggerMock = new Mock<ILogger<ProfileModel>>();
            _pageModel = new ProfileModel(_userServiceMock.Object, _bookingServiceMock.Object, _loggerMock.Object);

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
            var result = await _pageModel.OnGetAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenLoggedIn_LoadsUserAndBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _httpContext.Session.SetInt32("UserId", 1);

            var user = new UserDto { Id = 1, Email = "user@example.com", FirstName = "Alice" };
            var bookings = new List<BookingDto>
            {
                new BookingDto { Id = 1, TourName = "Tour 1" },
                new BookingDto { Id = 2, TourName = "Tour 2" },
                new BookingDto { Id = 3, TourName = "Tour 3" },
                new BookingDto { Id = 4, TourName = "Tour 4" },
                new BookingDto { Id = 5, TourName = "Tour 5" },
                new BookingDto { Id = 6, TourName = "Tour 6" }
            };

            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(user);
            _bookingServiceMock.Setup(s => s.GetByEmailAsync("user@example.com", default)).ReturnsAsync(bookings);

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(_pageModel.User);
            Assert.Equal("Alice", _pageModel.User!.FirstName);
            // RecentBookings should be limited to 5
            Assert.Equal(5, System.Linq.Enumerable.Count(_pageModel.RecentBookings));
        }

        [Fact]
        public async Task OnGetAsync_WhenNoUserId_StillLoadsBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            // No UserId in session

            _bookingServiceMock.Setup(s => s.GetByEmailAsync("user@example.com", default))
                .ReturnsAsync(new List<BookingDto>());

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Null(_pageModel.User);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _httpContext.Session.SetInt32("UserId", 1);
            _userServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void ProfileModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Null(_pageModel.User);
            Assert.Empty(_pageModel.RecentBookings);
        }
    }
}
