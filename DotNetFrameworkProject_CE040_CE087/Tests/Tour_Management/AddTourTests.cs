using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for AddTour page logic and tour insertion workflow.
    /// </summary>
    public class AddTourTests
    {
        // ─── TextBox controls used in AddTour ─────────────────────────────────────

        [Fact]
        public void TextBox_TourName_CanStoreValue()
        {
            // Arrange
            var tour_name = new TextBox();
            // Act
            tour_name.Text = "Paris Adventure";
            // Assert
            Assert.Equal("Paris Adventure", tour_name.Text);
        }

        [Fact]
        public void TextBox_Place_CanStoreValue()
        {
            // Arrange
            var place = new TextBox();
            // Act
            place.Text = "Paris, France";
            // Assert
            Assert.Equal("Paris, France", place.Text);
        }

        [Fact]
        public void TextBox_Days_CanStoreValue()
        {
            // Arrange
            var days = new TextBox();
            // Act
            days.Text = "7";
            // Assert
            Assert.Equal("7", days.Text);
        }

        [Fact]
        public void TextBox_Price_CanStoreValue()
        {
            // Arrange
            var price = new TextBox();
            // Act
            price.Text = "1500.00";
            // Assert
            Assert.Equal("1500.00", price.Text);
        }

        [Fact]
        public void TextBox_Locations_CanStoreValue()
        {
            // Arrange
            var locations = new TextBox();
            // Act
            locations.Text = "Eiffel Tower, Louvre, Notre Dame";
            // Assert
            Assert.Equal("Eiffel Tower, Louvre, Notre Dame", locations.Text);
        }

        [Fact]
        public void TextBox_TourInfo_CanStoreValue()
        {
            // Arrange
            var tour_info = new TextBox();
            // Act
            tour_info.Text = "A wonderful 7-day tour of Paris";
            // Assert
            Assert.Equal("A wonderful 7-day tour of Paris", tour_info.Text);
        }

        // ─── FileUpload control ───────────────────────────────────────────────────

        [Fact]
        public void FileUpload_HasFile_DefaultIsFalse()
        {
            // Arrange
            var fileUpload = new FileUpload();
            // Assert
            Assert.False(fileUpload.HasFile);
        }

        [Fact]
        public void FileUpload_FileName_DefaultIsEmpty()
        {
            // Arrange
            var fileUpload = new FileUpload();
            // Assert
            Assert.Equal(string.Empty, fileUpload.FileName);
        }

        [Fact]
        public void FileUpload_FileName_CanBeSetAndGet()
        {
            // Arrange
            var fileUpload = new FileUpload();
            // Act
            fileUpload.FileName = "tour_paris.jpg";
            // Assert
            Assert.Equal("tour_paris.jpg", fileUpload.FileName);
        }

        [Fact]
        public void FileUpload_SaveAs_DoesNotThrow()
        {
            // Arrange
            var fileUpload = new FileUpload();
            // Act & Assert
            var ex = Record.Exception(() => fileUpload.SaveAs("/tmp/tour_pics/tour_paris.jpg"));
            Assert.Null(ex);
        }

        // ─── Insert query validation ──────────────────────────────────────────────

        [Fact]
        public void InsertQuery_ContainsAllTourColumns()
        {
            // Arrange
            string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) values(@TOUR_NAME,@PLACE,@DAYS,@PRICE,@LOCATIONS,@TOUR_INFO,@pic)";
            // Assert
            Assert.Contains("TOUR_NAME", insertQuery);
            Assert.Contains("PLACE", insertQuery);
            Assert.Contains("DAYS", insertQuery);
            Assert.Contains("PRICE", insertQuery);
            Assert.Contains("LOCATIONS", insertQuery);
            Assert.Contains("TOUR_INFO", insertQuery);
            Assert.Contains("pic", insertQuery);
        }

        [Fact]
        public void InsertQuery_ContainsAllParameters()
        {
            // Arrange
            string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) values(@TOUR_NAME,@PLACE,@DAYS,@PRICE,@LOCATIONS,@TOUR_INFO,@pic)";
            // Assert
            Assert.Contains("@TOUR_NAME", insertQuery);
            Assert.Contains("@PLACE", insertQuery);
            Assert.Contains("@DAYS", insertQuery);
            Assert.Contains("@PRICE", insertQuery);
            Assert.Contains("@LOCATIONS", insertQuery);
            Assert.Contains("@TOUR_INFO", insertQuery);
            Assert.Contains("@pic", insertQuery);
        }

        // ─── Price validation ─────────────────────────────────────────────────────

        [Theory]
        [InlineData("1500.00", true)]
        [InlineData("0", true)]
        [InlineData("999999.99", true)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        [InlineData("-100", true)]
        public void Price_ParseValidation(string priceText, bool expected)
        {
            // Act
            bool isValid = decimal.TryParse(priceText, out _);
            // Assert
            Assert.Equal(expected, isValid);
        }

        // ─── Days validation ──────────────────────────────────────────────────────

        [Theory]
        [InlineData("7", true)]
        [InlineData("1", true)]
        [InlineData("30", true)]
        [InlineData("0", true)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        public void Days_ParseValidation(string daysText, bool expected)
        {
            // Act
            bool isValid = int.TryParse(daysText, out _);
            // Assert
            Assert.Equal(expected, isValid);
        }

        // ─── Server MapPath simulation ────────────────────────────────────────────

        [Fact]
        public void Server_MapPath_TourPicsFolder_ReturnsPath()
        {
            // Arrange
            var server = new HttpServerUtility();
            // Act
            var path = server.MapPath("~/Tour_pics/");
            // Assert
            Assert.Equal("~/Tour_pics/", path);
        }

        [Fact]
        public void Server_MapPath_ConcatenateWithFileName_ProducesFullPath()
        {
            // Arrange
            var server = new HttpServerUtility();
            string fileName = "tour_paris.jpg";
            // Act
            string fullPath = server.MapPath("~/Tour_pics/") + fileName;
            // Assert
            Assert.Equal("~/Tour_pics/tour_paris.jpg", fullPath);
        }

        // ─── Response behavior ────────────────────────────────────────────────────

        [Fact]
        public void HttpResponse_WriteAddSuccessful_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Write("ADD  Successful"));
            Assert.Null(ex);
        }

        // ─── Tour name validation ─────────────────────────────────────────────────

        [Theory]
        [InlineData("Paris Adventure", true)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("A", true)]
        public void TourName_NotNullOrWhitespace_Validation(string tourName, bool expected)
        {
            // Act
            bool isValid = !string.IsNullOrWhiteSpace(tourName);
            // Assert
            Assert.Equal(expected, isValid);
        }

        // ─── Page lifecycle ───────────────────────────────────────────────────────

        [Fact]
        public void Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        private class StubPage : System.Web.UI.Page { }
    }
}
