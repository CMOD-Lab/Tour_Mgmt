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
using TourManagement.Web.Pages.Admin;

namespace TourManagement.Web.Tests.Pages.Admin
{
    public class AdminDashboardModelTests
    {
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<DashboardModel>> _loggerMock;
        private readonly DashboardModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public AdminDashboardModelTests()
        {
            _tourServiceMock = new Mock<ITourService>();
            _bookingServiceMock = new Mock<IBookingService>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<DashboardModel>>();
            _pageModel = new DashboardModel(
                _tourServiceMock.Object,
                _bookingServiceMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WhenNotAdmin_RedirectsToLogin()
        {
            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenAdminAndDataExists_ReturnsPageWithStats()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            var tours = new List<TourDto>
            {
                new TourDto { Id = 1 },
                new TourDto { Id = 2 },
                new TourDto { Id = 3 }
            };
            var bookings = new List<BookingDto>
            {
                new BookingDto { Id = 1 },
                new BookingDto { Id = 2 },
                new BookingDto { Id = 3 },
                new BookingDto { Id = 4 },
                new BookingDto { Id = 5 },
                new BookingDto { Id = 6 },
                new BookingDto { Id = 7 },
                new BookingDto { Id = 8 },
                new BookingDto { Id = 9 },
                new BookingDto { Id = 10 },
                new BookingDto { Id = 11 }
            };
            var users = new List<UserDto>
            {
                new UserDto { Id = 1 },
                new UserDto { Id = 2 }
            };

            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(tours);
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(bookings);
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(users);

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(3, _pageModel.TotalTours);
            Assert.Equal(11, _pageModel.TotalBookings);
            Assert.Equal(2, _pageModel.TotalUsers);
            // RecentBookings should be limited to 10
            Assert.Equal(10, System.Linq.Enumerable.Count(_pageModel.RecentBookings));
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_WithEmptyData_SetsZeroCounts()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<TourDto>());
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<BookingDto>());
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<UserDto>());

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(0, _pageModel.TotalTours);
            Assert.Equal(0, _pageModel.TotalBookings);
            Assert.Equal(0, _pageModel.TotalUsers);
            Assert.Empty(_pageModel.RecentBookings);
        }

        [Fact]
        public void DashboardModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Equal(0, _pageModel.TotalTours);
            Assert.Equal(0, _pageModel.TotalBookings);
            Assert.Equal(0, _pageModel.TotalUsers);
            Assert.Empty(_pageModel.RecentBookings);
        }
    }
}
