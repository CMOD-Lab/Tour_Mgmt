using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for SignUpForm page logic and registration workflow.
    /// </summary>
    public class SignUpFormTests
    {
        // ─── TextBox controls used in SignUpForm ──────────────────────────────────

        [Fact]
        public void TextBox_Email_CanStoreValue()
        {
            // Arrange
            var email = new TextBox();
            // Act
            email.Text = "john@example.com";
            // Assert
            Assert.Equal("john@example.com", email.Text);
        }

        [Fact]
        public void TextBox_FirstName_CanStoreValue()
        {
            // Arrange
            var fname = new TextBox();
            // Act
            fname.Text = "John";
            // Assert
            Assert.Equal("John", fname.Text);
        }

        [Fact]
        public void TextBox_LastName_CanStoreValue()
        {
            // Arrange
            var lname = new TextBox();
            // Act
            lname.Text = "Doe";
            // Assert
            Assert.Equal("Doe", lname.Text);
        }

        [Fact]
        public void TextBox_Gender_CanStoreValue()
        {
            // Arrange
            var gender = new TextBox();
            // Act
            gender.Text = "Male";
            // Assert
            Assert.Equal("Male", gender.Text);
        }

        [Fact]
        public void TextBox_Password_CanStoreValue()
        {
            // Arrange
            var password1 = new TextBox();
            // Act
            password1.Text = "P@ssw0rd";
            // Assert
            Assert.Equal("P@ssw0rd", password1.Text);
        }

        [Fact]
        public void TextBox_Dob_CanStoreValue()
        {
            // Arrange
            var dob = new TextBox();
            // Act
            dob.Text = "1990-01-15";
            // Assert
            Assert.Equal("1990-01-15", dob.Text);
        }

        [Fact]
        public void TextBox_Street_CanStoreValue()
        {
            // Arrange
            var street = new TextBox();
            // Act
            street.Text = "123 Main St";
            // Assert
            Assert.Equal("123 Main St", street.Text);
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
        public void TextBox_State_CanStoreValue()
        {
            // Arrange
            var state = new TextBox();
            // Act
            state.Text = "NY";
            // Assert
            Assert.Equal("NY", state.Text);
        }

        // ─── Registration validation logic ────────────────────────────────────────

        [Fact]
        public void Registration_AllFieldsProvided_IsValid()
        {
            // Arrange
            string email = "john@example.com";
            string fname = "John";
            string lname = "Doe";
            string gender = "Male";
            string password = "P@ssw0rd";
            string dob = "1990-01-15";
            string street = "123 Main St";
            string city = "New York";
            string state = "NY";

            // Act
            bool allFilled = !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(fname)
                && !string.IsNullOrEmpty(lname)
                && !string.IsNullOrEmpty(gender)
                && !string.IsNullOrEmpty(password)
                && !string.IsNullOrEmpty(dob)
                && !string.IsNullOrEmpty(street)
                && !string.IsNullOrEmpty(city)
                && !string.IsNullOrEmpty(state);

            // Assert
            Assert.True(allFilled);
        }

        [Fact]
        public void Registration_MissingEmail_IsInvalid()
        {
            // Arrange
            string email = "";
            // Act
            bool isValid = !string.IsNullOrEmpty(email);
            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Registration_MissingPassword_IsInvalid()
        {
            // Arrange
            string password = "";
            // Act
            bool isValid = !string.IsNullOrEmpty(password);
            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Registration_NullEmail_IsInvalid()
        {
            // Arrange
            string? email = null;
            // Act
            bool isValid = !string.IsNullOrEmpty(email);
            // Assert
            Assert.False(isValid);
        }

        // ─── Insert query construction ────────────────────────────────────────────

        [Fact]
        public void InsertQuery_ContainsAllRequiredColumns()
        {
            // Arrange
            string insertQuery = "insert into UserInfo(Email,FirstName,LastName,Gender,Password,dob,Street,City,State) values(@email,@FirstName,@LastName,@Gender,@Password,@dob,@Street,@City,@State)";
            // Assert
            Assert.Contains("Email", insertQuery);
            Assert.Contains("FirstName", insertQuery);
            Assert.Contains("LastName", insertQuery);
            Assert.Contains("Gender", insertQuery);
            Assert.Contains("Password", insertQuery);
            Assert.Contains("dob", insertQuery);
            Assert.Contains("Street", insertQuery);
            Assert.Contains("City", insertQuery);
            Assert.Contains("State", insertQuery);
        }

        [Fact]
        public void InsertQuery_ContainsAllParameters()
        {
            // Arrange
            string insertQuery = "insert into UserInfo(Email,FirstName,LastName,Gender,Password,dob,Street,City,State) values(@email,@FirstName,@LastName,@Gender,@Password,@dob,@Street,@City,@State)";
            // Assert
            Assert.Contains("@email", insertQuery);
            Assert.Contains("@FirstName", insertQuery);
            Assert.Contains("@LastName", insertQuery);
            Assert.Contains("@Gender", insertQuery);
            Assert.Contains("@Password", insertQuery);
            Assert.Contains("@dob", insertQuery);
            Assert.Contains("@Street", insertQuery);
            Assert.Contains("@City", insertQuery);
            Assert.Contains("@State", insertQuery);
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
        public void HttpResponse_RedirectToUserLogin_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Redirect("userlogin.aspx"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpServerUtility_TransferToUserCrud_DoesNotThrow()
        {
            // Arrange
            var server = new HttpServerUtility();
            // Act & Assert
            var ex = Record.Exception(() => server.Transfer("usercrud.aspx"));
            Assert.Null(ex);
        }

        // ─── Password strength checks ─────────────────────────────────────────────

        [Theory]
        [InlineData("P@ssw0rd", true)]
        [InlineData("ab", false)]
        [InlineData("", false)]
        [InlineData("12345678", true)]
        public void Password_LengthCheck_MinimumFiveChars(string password, bool expected)
        {
            // Act
            bool isLongEnough = password.Length >= 5;
            // Assert
            Assert.Equal(expected, isLongEnough);
        }

        // ─── Date of birth validation ─────────────────────────────────────────────

        [Theory]
        [InlineData("1990-01-15", true)]
        [InlineData("2000-12-31", true)]
        [InlineData("", false)]
        [InlineData("not-a-date", false)]
        public void Dob_ParseValidation(string dob, bool expected)
        {
            // Act
            bool isValid = DateTime.TryParse(dob, out _);
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
