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

namespace TourManagement.Web.Tests.Pages.Tours
{
    public class ToursDetailsModelTests
    {
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<ILogger<DetailsModel>> _loggerMock;
        private readonly DetailsModel _pageModel;

        public ToursDetailsModelTests()
        {
            _tourServiceMock = new Mock<ITourService>();
            _loggerMock = new Mock<ILogger<DetailsModel>>();
            _pageModel = new DetailsModel(_tourServiceMock.Object, _loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WithValidId_SetsTourProperty()
        {
            // Arrange
            var tour = new TourDto { Id = 1, TourName = "Paris Tour", Place = "Paris" };
            _tourServiceMock.Setup(s => s.GetByIdAsync(1, default)).ReturnsAsync(tour);

            // Act
            await _pageModel.OnGetAsync(1);

            // Assert
            Assert.NotNull(_pageModel.Tour);
            Assert.Equal(1, _pageModel.Tour!.Id);
            Assert.Equal("Paris Tour", _pageModel.Tour.TourName);
        }

        [Fact]
        public async Task OnGetAsync_WithInvalidId_TourIsNull()
        {
            // Arrange
            _tourServiceMock.Setup(s => s.GetByIdAsync(999, default)).ReturnsAsync((TourDto?)null);

            // Act
            await _pageModel.OnGetAsync(999);

            // Assert
            Assert.Null(_pageModel.Tour);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_DoesNotPropagateException()
        {
            // Arrange
            _tourServiceMock.Setup(s => s.GetByIdAsync(1, default)).ThrowsAsync(new Exception("DB error"));

            // Act & Assert (should not throw)
            await _pageModel.OnGetAsync(1);
        }

        [Fact]
        public void DetailsModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Null(_pageModel.Tour);
        }
    }
}
