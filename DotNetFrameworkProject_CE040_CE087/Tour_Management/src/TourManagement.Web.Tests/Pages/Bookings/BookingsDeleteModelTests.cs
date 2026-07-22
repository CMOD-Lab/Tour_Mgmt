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
using TourManagement.Web.Pages.Bookings;

namespace TourManagement.Web.Tests.Pages.Bookings
{
    public class BookingsDeleteModelTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ILogger<DeleteModel>> _loggerMock;
        private readonly DeleteModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public BookingsDeleteModelTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _loggerMock = new Mock<ILogger<DeleteModel>>();
            _pageModel = new DeleteModel(_bookingServiceMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WhenNotLoggedIn_RedirectsToUserLogin()
        {
            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Users/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenLoggedInAndBookingExists_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            var booking = new BookingDto { Id = 1, TourName = "Paris Tour" };
            _bookingServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(booking);

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(_pageModel.Booking);
            Assert.Equal("Paris Tour", _pageModel.Booking!.TourName);
        }

        [Fact]
        public async Task OnGetAsync_WhenBookingNotFound_RedirectsToMyBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _bookingServiceMock.Setup(s => s.GetByIdAsync(999, default)).ReturnsAsync((BookingDto?)null);

            // Act
            var result = await _pageModel.OnGetAsync(999);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("MyBookings", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_RedirectsToMyBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _bookingServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("MyBookings", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotLoggedIn_RedirectsToUserLogin()
        {
            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Users/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenLoggedIn_DeletesAndRedirects()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _bookingServiceMock.Setup(s => s.DeleteAsync(1, default)).Returns(Task.CompletedTask);

            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("MyBookings", redirect.PageName);
            _bookingServiceMock.Verify(s => s.DeleteAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotFoundException_RedirectsToMyBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _bookingServiceMock.Setup(s => s.DeleteAsync(1, default)).ThrowsAsync(new NotFoundException("Booking", 1));

            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("MyBookings", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_RedirectsToMyBookings()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _bookingServiceMock.Setup(s => s.DeleteAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("MyBookings", redirect.PageName);
        }
    }
}
