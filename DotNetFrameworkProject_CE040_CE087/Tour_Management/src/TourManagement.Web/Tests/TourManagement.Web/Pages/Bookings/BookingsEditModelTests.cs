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
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.Pages.Bookings
{
    public class BookingsEditModelTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ILogger<EditModel>> _loggerMock;
        private readonly EditModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public BookingsEditModelTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _loggerMock = new Mock<ILogger<EditModel>>();
            _pageModel = new EditModel(_bookingServiceMock.Object, _loggerMock.Object);

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
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Admin/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenAdminAndBookingExists_ReturnsPageWithInput()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            var booking = new BookingDto
            {
                Id = 1,
                TourName = "Paris Tour",
                Place = "Paris",
                Email = "user@example.com",
                FirstName = "Alice",
                IsActive = true
            };
            _bookingServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(booking);

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(1, _pageModel.Input.Id);
            Assert.Equal("Paris Tour", _pageModel.Input.TourName);
            Assert.Equal("user@example.com", _pageModel.Input.Email);
        }

        [Fact]
        public async Task OnGetAsync_WhenBookingNotFound_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _bookingServiceMock.Setup(s => s.GetByIdAsync(999, default)).ReturnsAsync((BookingDto?)null);

            // Act
            var result = await _pageModel.OnGetAsync(999);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _bookingServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotAdmin_RedirectsToAdminLogin()
        {
            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Admin/Login", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenAdminAndValidModel_UpdatesAndRedirects()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _pageModel.Input = new BookingEditViewModel
            {
                Id = 1,
                TourName = "Updated Tour",
                Place = "London",
                Email = "user@example.com",
                FirstName = "Alice",
                IsActive = true
            };
            _bookingServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<BookingUpdateDto>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
            _bookingServiceMock.Verify(s => s.UpdateAsync(1, It.IsAny<BookingUpdateDto>(), default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotFoundException_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _pageModel.Input = new BookingEditViewModel
            {
                Id = 1,
                TourName = "Tour",
                Place = "Place",
                Email = "user@example.com",
                FirstName = "Alice"
            };
            _bookingServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<BookingUpdateDto>(), default))
                .ThrowsAsync(new NotFoundException("Booking", 1));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _pageModel.Input = new BookingEditViewModel
            {
                Id = 1,
                TourName = "Tour",
                Place = "Place",
                Email = "user@example.com",
                FirstName = "Alice"
            };
            _bookingServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<BookingUpdateDto>(), default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
