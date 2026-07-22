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
using TourManagement.Web.Pages.Tours;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.Pages.Tours
{
    public class ToursCreateModelTests
    {
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly Mock<ILogger<CreateModel>> _loggerMock;
        private readonly CreateModel _pageModel;
        private readonly DefaultHttpContext _httpContext;

        public ToursCreateModelTests()
        {
            _tourServiceMock = new Mock<ITourService>();
            _envMock = new Mock<IWebHostEnvironment>();
            _loggerMock = new Mock<ILogger<CreateModel>>();
            _pageModel = new CreateModel(_tourServiceMock.Object, _envMock.Object, _loggerMock.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new MockSession();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(_httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = _httpContext };
        }

        [Fact]
        public void OnGet_WhenNotAdmin_RedirectsToAdminLogin()
        {
            // Act
            var result = _pageModel.OnGet();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Admin/Login", redirect.PageName);
        }

        [Fact]
        public void OnGet_WhenAdmin_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");

            // Act
            var result = _pageModel.OnGet();

            // Assert
            Assert.IsType<PageResult>(result);
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
        public async Task OnPostAsync_WhenAdminAndValidModel_CreatesAndRedirects()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _pageModel.Input = new TourCreateViewModel
            {
                TourName = "New Tour",
                Place = "Berlin",
                Days = 4,
                Price = 900m,
                Locations = "Brandenburg Gate",
                TourInfo = "Explore Berlin"
            };
            var createdTour = new TourDto { Id = 1, TourName = "New Tour" };
            _tourServiceMock.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>(), default)).ReturnsAsync(createdTour);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
            _tourServiceMock.Verify(s => s.CreateAsync(It.IsAny<TourCreateDto>(), default), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WhenServiceThrows_ReturnsPage()
        {
            // Arrange
            _httpContext.Session.SetString("IsAdmin", "true");
            _pageModel.Input = new TourCreateViewModel
            {
                TourName = "New Tour",
                Place = "Berlin",
                Days = 4,
                Price = 900m,
                Locations = "Brandenburg Gate",
                TourInfo = "Explore Berlin"
            };
            _tourServiceMock.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>(), default))
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
        }
    }
}
