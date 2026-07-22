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
using TourManagement.Web.Pages;

namespace TourManagement.Web.Tests.Pages
{
    public class IndexModelTests
    {
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<IndexModel>> _loggerMock;
        private readonly IndexModel _pageModel;

        public IndexModelTests()
        {
            _tourServiceMock = new Mock<ITourService>();
            _bookingServiceMock = new Mock<IBookingService>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<IndexModel>>();

            _pageModel = new IndexModel(
                _tourServiceMock.Object,
                _bookingServiceMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object);

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task OnGetAsync_WithTours_PopulatesFeaturedToursAndCounts()
        {
            // Arrange
            var tours = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour 1", IsActive = true },
                new TourDto { Id = 2, TourName = "Tour 2", IsActive = true },
                new TourDto { Id = 3, TourName = "Tour 3", IsActive = true },
                new TourDto { Id = 4, TourName = "Tour 4", IsActive = true },
                new TourDto { Id = 5, TourName = "Tour 5", IsActive = true },
                new TourDto { Id = 6, TourName = "Tour 6", IsActive = true },
                new TourDto { Id = 7, TourName = "Tour 7", IsActive = true }
            };
            var bookings = new List<BookingDto>
            {
                new BookingDto { Id = 1 },
                new BookingDto { Id = 2 }
            };
            var users = new List<UserDto>
            {
                new UserDto { Id = 1 },
                new UserDto { Id = 2 },
                new UserDto { Id = 3 }
            };

            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(tours);
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(bookings);
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(users);

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.Equal(7, _pageModel.TotalTours);
            Assert.Equal(2, _pageModel.TotalBookings);
            Assert.Equal(3, _pageModel.TotalUsers);
            // FeaturedTours should be limited to 6
            Assert.Equal(6, System.Linq.Enumerable.Count(_pageModel.FeaturedTours));
        }

        [Fact]
        public async Task OnGetAsync_WithEmptyData_SetsZeroCounts()
        {
            // Arrange
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<TourDto>());
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<BookingDto>());
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<UserDto>());

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.Equal(0, _pageModel.TotalTours);
            Assert.Equal(0, _pageModel.TotalBookings);
            Assert.Equal(0, _pageModel.TotalUsers);
            Assert.Empty(_pageModel.FeaturedTours);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_DoesNotPropagateException()
        {
            // Arrange
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ThrowsAsync(new Exception("DB error"));

            // Act & Assert (should not throw)
            await _pageModel.OnGetAsync();
        }

        [Fact]
        public async Task OnGetAsync_WithFewerThanSixTours_ShowsAllTours()
        {
            // Arrange
            var tours = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour 1" },
                new TourDto { Id = 2, TourName = "Tour 2" }
            };
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(tours);
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<BookingDto>());
            _userServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(new List<UserDto>());

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.Equal(2, _pageModel.TotalTours);
            Assert.Equal(2, System.Linq.Enumerable.Count(_pageModel.FeaturedTours));
        }

        [Fact]
        public void IndexModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Empty(_pageModel.FeaturedTours);
            Assert.Equal(0, _pageModel.TotalTours);
            Assert.Equal(0, _pageModel.TotalBookings);
            Assert.Equal(0, _pageModel.TotalUsers);
        }
    }
}
