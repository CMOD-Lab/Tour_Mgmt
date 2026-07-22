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
using TourManagement.Web.Pages.Tours;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.Pages.Tours
{
    public class ToursEditModelTests
    {
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly Mock<ILogger<EditModel>> _loggerMock;
        private readonly EditModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public ToursEditModelTests()
        {
            _tourServiceMock = new Mock<ITourService>();
            _envMock = new Mock<IWebHostEnvironment>();
            _loggerMock = new Mock<ILogger<EditModel>>();
            _pageModel = new EditModel(_tourServiceMock.Object, _envMock.Object, _loggerMock.Object);

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
        public async Task OnGetAsync_WhenAdminAndTourExists_ReturnsPageWithInput()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            var tour = new TourDto
            {
                Id = 1,
                TourName = "Paris Tour",
                Place = "Paris",
                Days = 7,
                Price = 1500m,
                Locations = "Eiffel Tower",
                TourInfo = "Great tour",
                Pic = "paris.jpg",
                IsActive = true
            };
            _tourServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(tour);

            // Act
            var result = await _pageModel.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(1, _pageModel.Input.Id);
            Assert.Equal("Paris Tour", _pageModel.Input.TourName);
            Assert.Equal("Paris", _pageModel.Input.Place);
            Assert.Equal(7, _pageModel.Input.Days);
            Assert.Equal(1500m, _pageModel.Input.Price);
            Assert.Equal("paris.jpg", _pageModel.Input.CurrentPic);
        }

        [Fact]
        public async Task OnGetAsync_WhenTourNotFound_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _tourServiceMock.Setup(s => s.GetByIdAsync(999, default)).ReturnsAsync((TourDto?)null);

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
            _tourServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

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
            _pageModel.Input = new TourEditViewModel
            {
                Id = 1,
                TourName = "Updated Tour",
                Place = "London",
                Days = 5,
                Price = 1000m,
                Locations = "Big Ben",
                TourInfo = "Great tour",
                IsActive = true
            };
            _tourServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<TourUpdateDto>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
            _tourServiceMock.Verify(s => s.UpdateAsync(1, It.IsAny<TourUpdateDto>(), default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenNotFoundException_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _pageModel.Input = new TourEditViewModel
            {
                Id = 1,
                TourName = "Tour",
                Place = "Place",
                Days = 1,
                Price = 100m,
                Locations = "Loc",
                TourInfo = "Info"
            };
            _tourServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<TourUpdateDto>(), default))
                .ThrowsAsync(new NotFoundException("Tour", 1));

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
            _pageModel.Input = new TourEditViewModel
            {
                Id = 1,
                TourName = "Tour",
                Place = "Place",
                Days = 1,
                Price = 100m,
                Locations = "Loc",
                TourInfo = "Info"
            };
            _tourServiceMock.Setup(s => s.UpdateAsync(1, It.IsAny<TourUpdateDto>(), default))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void EditModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.NotNull(_pageModel.Input);
        }
    }
}
