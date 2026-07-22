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
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.Pages.Bookings
{
    public class BookingsCreateModelTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<ILogger<CreateModel>> _loggerMock;
        private readonly CreateModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public BookingsCreateModelTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _tourServiceMock = new Mock<ITourService>();
            _loggerMock = new Mock<ILogger<CreateModel>>();
            _pageModel = new CreateModel(_bookingServiceMock.Object, _tourServiceMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WithNoTourId_DoesNotLoadTour()
        {
            // Act
            await _pageModel.OnGetAsync(null);

            // Assert
            Assert.Null(_pageModel.SelectedTour);
            _tourServiceMock.Verify(s => s.GetByIdAsync(It.IsAny<int>(), default), Times.Never);
        }

        [Fact]
        public async Task OnGetAsync_WithTourId_LoadsTourAndPreFillsInput()
        {
            // Arrange
            var tour = new TourDto { Id = 3, TourName = "Rome Tour", Place = "Rome" };
            _tourServiceMock.Setup(s => s.GetByIdAsync(3, default)).ReturnsAsync(tour);

            // Act
            await _pageModel.OnGetAsync(3);

            // Assert
            Assert.NotNull(_pageModel.SelectedTour);
            Assert.Equal("Rome Tour", _pageModel.Input.TourName);
            Assert.Equal("Rome", _pageModel.Input.Place);
            Assert.Equal(3, _pageModel.Input.TourId);
        }

        [Fact]
        public async Task OnGetAsync_WithSessionEmail_PreFillsEmail()
        {
            // Arrange
            _httpContext.Session.SetString("UserEmail", "user@example.com");
            _httpContext.Session.SetString("UserName", "Alice Wonder");

            // Act
            await _pageModel.OnGetAsync(null);

            // Assert
            Assert.Equal("user@example.com", _pageModel.Input.Email);
            Assert.Equal("Alice", _pageModel.Input.FirstName);
        }

        [Fact]
        public async Task OnGetAsync_WhenTourServiceThrows_DoesNotPropagateException()
        {
            // Arrange
            _tourServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act & Assert (should not throw)
            await _pageModel.OnGetAsync(1);
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_CreatesBookingAndRedirects()
        {
            // Arrange
            _pageModel.Input = new BookingCreateViewModel
            {
                TourName = "Paris Tour",
                Place = "Paris",
                Email = "user@example.com",
                FirstName = "Alice",
                TourId = 1
            };
            var createdBooking = new BookingDto { Id = 1, TourName = "Paris Tour" };
            _bookingServiceMock.Setup(s => s.CreateAsync(It.IsAny<BookingCreateDto>(), default)).ReturnsAsync(createdBooking);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("MyBookings", redirect.PageName);
            _bookingServiceMock.Verify(s => s.CreateAsync(It.IsAny<BookingCreateDto>(), default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _pageModel.Input = new BookingCreateViewModel
            {
                TourName = "Paris Tour",
                Place = "Paris",
                Email = "user@example.com",
                FirstName = "Alice"
            };
            _bookingServiceMock.Setup(s => s.CreateAsync(It.IsAny<BookingCreateDto>(), default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void CreateModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.NotNull(_pageModel.Input);
            Assert.Null(_pageModel.SelectedTour);
        }
    }
}
