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
using TourManagement.Web.Pages.Bookings;

namespace TourManagement.Web.Tests.Pages.Bookings
{
    public class BookingsMyBookingsModelTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ILogger<MyBookingsModel>> _loggerMock;
        private readonly MyBookingsModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public BookingsMyBookingsModelTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _loggerMock = new Mock<ILogger<MyBookingsModel>>();
            _pageModel = new MyBookingsModel(_bookingServiceMock.Object, _loggerMock.Object);

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
            Assert.Equal("/Users/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenLoggedIn_ReturnsPageWithBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            var bookings = new List<BookingDto>
            {
                new BookingDto { Id = 1, TourName = "Tour A" },
                new BookingDto { Id = 2, TourName = "Tour B" }
            };
            _bookingServiceMock.Setup(s => s.GetByEmailAsync("user@example.com", default)).ReturnsAsync(bookings);

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(2, System.Linq.Enumerable.Count(_pageModel.Bookings));
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _bookingServiceMock.Setup(s => s.GetByEmailAsync("user@example.com", default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.True(_pageModel.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public void MyBookingsModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Empty(_pageModel.Bookings);
        }
    }
}
