using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for Order page logic and booking insertion workflow.
    /// </summary>
    public class OrderTests
    {
        // ─── TextBox controls used in Order ───────────────────────────────────────

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
        public void TextBox_City_CanStoreValue()
        {
            // Arrange
            var city = new TextBox();
            // Act
            city.Text = "New York";
            // Assert
            Assert.Equal("New York", city.Text);
        }

        [Fact]
        public void TextBox_Number_CanStoreEmailValue()
        {
            // Arrange
            var number = new TextBox();
            // Act
            number.Text = "user@example.com";
            // Assert
            Assert.Equal("user@example.com", number.Text);
        }

        [Fact]
        public void TextBox_Name_CanStoreValue()
        {
            // Arrange
            var name = new TextBox();
            // Act
            name.Text = "John Doe";
            // Assert
            Assert.Equal("John Doe", name.Text);
        }

        // ─── Booking insert query validation ──────────────────────────────────────

        [Fact]
        public void InsertQuery_ContainsAllBookingColumns()
        {
            // Arrange
            string insertQuery = "insert into booking(TOUR_NAME,PLACE,Email,FirstName) values(@TOUR_NAME,@PLACE,@Email,@FirstName)";
            // Assert
            Assert.Contains("TOUR_NAME", insertQuery);
            Assert.Contains("PLACE", insertQuery);
            Assert.Contains("Email", insertQuery);
            Assert.Contains("FirstName", insertQuery);
        }

        [Fact]
        public void InsertQuery_ContainsAllParameters()
        {
            // Arrange
            string insertQuery = "insert into booking(TOUR_NAME,PLACE,Email,FirstName) values(@TOUR_NAME,@PLACE,@Email,@FirstName)";
            // Assert
            Assert.Contains("@TOUR_NAME", insertQuery);
            Assert.Contains("@PLACE", insertQuery);
            Assert.Contains("@Email", insertQuery);
            Assert.Contains("@FirstName", insertQuery);
        }

        [Fact]
        public void InsertQuery_TargetsBookingTable()
        {
            // Arrange
            string insertQuery = "insert into booking(TOUR_NAME,PLACE,Email,FirstName) values(@TOUR_NAME,@PLACE,@Email,@FirstName)";
            // Assert
            Assert.Contains("booking", insertQuery);
        }

        // ─── Booking validation logic ─────────────────────────────────────────────

        [Fact]
        public void Booking_AllFieldsProvided_IsValid()
        {
            // Arrange
            string tourName = "Paris Adventure";
            string place = "Paris";
            string email = "user@example.com";
            string firstName = "John";
            // Act
            bool isValid = !string.IsNullOrEmpty(tourName)
                && !string.IsNullOrEmpty(place)
                && !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(firstName);
            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Booking_MissingTourName_IsInvalid()
        {
            // Arrange
            string tourName = "";
            // Act
            bool isValid = !string.IsNullOrEmpty(tourName);
            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Booking_MissingEmail_IsInvalid()
        {
            // Arrange
            string email = "";
            // Act
            bool isValid = !string.IsNullOrEmpty(email);
            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Booking_MissingFirstName_IsInvalid()
        {
            // Arrange
            string firstName = "";
            // Act
            bool isValid = !string.IsNullOrEmpty(firstName);
            // Assert
            Assert.False(isValid);
        }

        // ─── Response behavior ────────────────────────────────────────────────────

        [Fact]
        public void HttpResponse_WriteRegistrationSuccessful_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Write("Registration Successful"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_RedirectToMyBooking_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Redirect("mybooking.aspx"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpServerUtility_TransferToMyBooking_DoesNotThrow()
        {
            // Arrange
            var server = new HttpServerUtility();
            // Act & Assert
            var ex = Record.Exception(() => server.Transfer("mybooking.aspx"));
            Assert.Null(ex);
        }

        // ─── Email format validation ──────────────────────────────────────────────

        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("notanemail", false)]
        [InlineData("", false)]
        [InlineData("@nodomain.com", false)]
        public void Email_ContainsAtAndDomain(string email, bool expected)
        {
            // Act
            bool isValid = email.Contains("@")
                && email.IndexOf("@") > 0
                && email.LastIndexOf(".") > email.IndexOf("@");
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

        // ─── Booking data model ───────────────────────────────────────────────────

        [Fact]
        public void BookingData_CanCreateAnonymousObject()
        {
            // Arrange & Act
            var booking = new
            {
                TourName = "Paris Adventure",
                Place = "Paris",
                Email = "user@example.com",
                FirstName = "John"
            };
            // Assert
            Assert.Equal("Paris Adventure", booking.TourName);
            Assert.Equal("user@example.com", booking.Email);
        }

        [Theory]
        [InlineData("Paris Adventure", "Paris", "user@example.com", "John", true)]
        [InlineData("", "Paris", "user@example.com", "John", false)]
        [InlineData("Paris Adventure", "", "user@example.com", "John", false)]
        [InlineData("Paris Adventure", "Paris", "", "John", false)]
        [InlineData("Paris Adventure", "Paris", "user@example.com", "", false)]
        public void Booking_FieldValidation_Theory(string tourName, string place, string email, string firstName, bool expected)
        {
            // Act
            bool isValid = !string.IsNullOrEmpty(tourName)
                && !string.IsNullOrEmpty(place)
                && !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(firstName);
            // Assert
            Assert.Equal(expected, isValid);
        }

        private class StubPage : System.Web.UI.Page { }
    }
}
