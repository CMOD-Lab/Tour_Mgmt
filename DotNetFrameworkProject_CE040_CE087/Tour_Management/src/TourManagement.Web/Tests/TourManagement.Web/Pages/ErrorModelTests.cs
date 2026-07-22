using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TourManagement.Web.Pages;

namespace TourManagement.Web.Tests.Pages
{
    public class ErrorModelTests
    {
        private readonly Mock<ILogger<ErrorModel>> _loggerMock;
        private readonly ErrorModel _pageModel;

        public ErrorModelTests()
        {
            _loggerMock = new Mock<ILogger<ErrorModel>>();
            _pageModel = new ErrorModel(_loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
            _pageModel.TempData = tempData;
            _pageModel.PageContext = new PageContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void ErrorModel_Constructor_InitializesCorrectly()
        {
            // Assert
            Assert.NotNull(_pageModel);
            Assert.Null(_pageModel.RequestId);
            Assert.False(_pageModel.ShowRequestId);
        }

        [Fact]
        public void OnGet_SetsRequestId_FromHttpContext()
        {
            // Arrange
            _pageModel.HttpContext.TraceIdentifier = "test-trace-id";

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.Equal("test-trace-id", _pageModel.RequestId);
        }

        [Fact]
        public void ShowRequestId_WhenRequestIdIsSet_ReturnsTrue()
        {
            // Arrange
            _pageModel.RequestId = "some-request-id";

            // Assert
            Assert.True(_pageModel.ShowRequestId);
        }

        [Fact]
        public void ShowRequestId_WhenRequestIdIsNull_ReturnsFalse()
        {
            // Arrange
            _pageModel.RequestId = null;

            // Assert
            Assert.False(_pageModel.ShowRequestId);
        }

        [Fact]
        public void ShowRequestId_WhenRequestIdIsEmpty_ReturnsFalse()
        {
            // Arrange
            _pageModel.RequestId = string.Empty;

            // Assert
            Assert.False(_pageModel.ShowRequestId);
        }

        [Fact]
        public void OnGet_WithTraceIdentifier_SetsRequestId()
        {
            // Arrange
            _pageModel.HttpContext.TraceIdentifier = "trace-123";

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.NotNull(_pageModel.RequestId);
            Assert.Equal("trace-123", _pageModel.RequestId);
            Assert.True(_pageModel.ShowRequestId);
        }
    }
}
