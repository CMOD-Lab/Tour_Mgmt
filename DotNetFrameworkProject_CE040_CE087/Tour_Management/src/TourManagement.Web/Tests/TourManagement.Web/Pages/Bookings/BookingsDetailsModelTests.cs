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
    public class BookingsDetailsModelTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ILogger<DetailsModel>> _loggerMock;
        private readonly DetailsModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public BookingsDetailsModelTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _loggerMock = new Mock<ILogger<DetailsModel>>();
            _pageModel = new DetailsModel(_bookingServiceMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WithValidId_SetsBookingProperty()
        {
            // Arrange
            var booking = new BookingDto { Id = 1, TourName = "Paris Tour", Email = "user@example.com" };
            _bookingServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(booking);

            // Act
            await _pageModel.OnGetAsync(1);

            // Assert
            Assert.NotNull(_pageModel.Booking);
            Assert.Equal(1, _pageModel.Booking!.Id);
            Assert.Equal("Paris Tour", _pageModel.Booking.TourName);
        }

        [Fact]
        public async Task OnGetAsync_WithInvalidId_BookingIsNull()
        {
            // Arrange
            _bookingServiceMock.Setup(s => s.GetByIdAsync(999, default)).ReturnsAsync((BookingDto?)null);

            // Act
            await _pageModel.OnGetAsync(999);

            // Assert
            Assert.Null(_pageModel.Booking);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_DoesNotPropagateException()
        {
            // Arrange
            _bookingServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act & Assert (should not throw)
            await _pageModel.OnGetAsync(1);
        }

        [Fact]
        public void DetailsModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Null(_pageModel.Booking);
        }
    }
}
