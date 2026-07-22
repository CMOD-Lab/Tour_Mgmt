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
    public class BookingsIndexModelTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ILogger<IndexModel>> _loggerMock;
        private readonly IndexModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public BookingsIndexModelTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _loggerMock = new Mock<ILogger<IndexModel>>();
            _pageModel = new IndexModel(_bookingServiceMock.Object, _loggerMock.Object);

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
        public async Task OnGetAsync_WhenAdminAndBookingsExist_ReturnsPageWithBookings()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            var bookings = new List<BookingDto>
            {
                new BookingDto { Id = 1, TourName = "Tour A" },
                new BookingDto { Id = 2, TourName = "Tour B" }
            };
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(bookings);

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
            _httpContext.Session.SetString("IsAdmin", "true");
            _bookingServiceMock.Setup(s => s.GetAllAsync(default)).ThrowsAsync(new Exception("DB error"));

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
            Assert.Empty(_pageModel.Bookings);
        }
    }
}
