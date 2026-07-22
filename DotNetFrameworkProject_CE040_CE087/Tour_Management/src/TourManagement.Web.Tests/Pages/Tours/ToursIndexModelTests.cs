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
    public class ToursIndexModelTests
    {
        private readonly Mock<ITourService> _tourServiceMock;
        private readonly Mock<ILogger<IndexModel>> _loggerMock;
        private readonly IndexModel _pageModel;

        public ToursIndexModelTests()
        {
            _tourServiceMock = new Mock<ITourService>();
            _loggerMock = new Mock<ILogger<IndexModel>>();
            _pageModel = new IndexModel(_tourServiceMock.Object, _loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext { HttpContext = httpContext };
        }

        [Fact]
        public async Task OnGetAsync_WithNoSearchTerm_ReturnsAllTours()
        {
            // Arrange
            var tours = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour A" },
                new TourDto { Id = 2, TourName = "Tour B" }
            };
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(tours);

            // Act
            await _pageModel.OnGetAsync(null);

            // Assert
            Assert.Equal(2, System.Linq.Enumerable.Count(_pageModel.Tours));
            Assert.Null(_pageModel.SearchTerm);
            _tourServiceMock.Verify(s => s.GetAllAsync(default), Times.Once);
            _tourServiceMock.Verify(s => s.SearchAsync(It.IsAny<string>(), default), Times.Never);
        }

        [Fact]
        public async Task OnGetAsync_WithSearchTerm_CallsSearchAsync()
        {
            // Arrange
            var tours = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Paris Tour" }
            };
            _tourServiceMock.Setup(s => s.SearchAsync("Paris", default)).ReturnsAsync(tours);

            // Act
            await _pageModel.OnGetAsync("Paris");

            // Assert
            Assert.Equal("Paris", _pageModel.SearchTerm);
            Assert.Single(_pageModel.Tours);
            _tourServiceMock.Verify(s => s.SearchAsync("Paris", default), Times.Once);
            _tourServiceMock.Verify(s => s.GetAllAsync(default), Times.Never);
        }

        [Fact]
        public async Task OnGetAsync_WithWhitespaceSearchTerm_ReturnsAllTours()
        {
            // Arrange
            var tours = new List<TourDto> { new TourDto { Id = 1, TourName = "Tour A" } };
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ReturnsAsync(tours);

            // Act
            await _pageModel.OnGetAsync("   ");

            // Assert
            _tourServiceMock.Verify(s => s.GetAllAsync(default), Times.Once);
        }

        [Fact]
        public async Task OnGetAsync_WhenServiceThrows_SetsTempDataError()
        {
            // Arrange
            _tourServiceMock.Setup(s => s.GetAllAsync(default)).ThrowsAsync(new Exception("DB error"));

            // Act
            await _pageModel.OnGetAsync(null);

            // Assert
            Assert.True(_pageModel.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public void IndexModel_Constructor_InitializesProperties()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Empty(_pageModel.Tours);
            Assert.Null(_pageModel.SearchTerm);
        }
    }
}
