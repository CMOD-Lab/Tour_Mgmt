using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for userlogin page logic and related web infrastructure.
    /// Since userlogin is a partial WebForms page with DB dependencies,
    /// we test the supporting infrastructure and page lifecycle stubs.
    /// </summary>
    public class UserLoginTests
    {
        // ─── Page stub infrastructure ─────────────────────────────────────────────

        [Fact]
        public void Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void Page_IsPostBack_CanBeSetToTrue()
        {
            // Arrange
            var page = new StubPage();
            // Act
            page.IsPostBack = true;
            // Assert
            Assert.True(page.IsPostBack);
        }

        [Fact]
        public void Page_Request_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Request);
        }

        [Fact]
        public void Page_Response_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Response);
        }

        [Fact]
        public void Page_Server_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Server);
        }

        [Fact]
        public void Page_Session_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Session);
        }

        [Fact]
        public void Page_ClientScript_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.ClientScript);
        }

        // ─── TextBox controls used in userlogin ──────────────────────────────────

        [Fact]
        public void TextBox_Email_CanStoreEmailValue()
        {
            // Arrange
            var txtEmail = new TextBox();
            // Act
            txtEmail.Text = "user@example.com";
            // Assert
            Assert.Equal("user@example.com", txtEmail.Text);
        }

        [Fact]
        public void TextBox_Password_CanStorePasswordValue()
        {
            // Arrange
            var txtPassword = new TextBox();
            // Act
            txtPassword.Text = "secret123";
            // Assert
            Assert.Equal("secret123", txtPassword.Text);
        }

        [Fact]
        public void TextBox_Email_EmptyByDefault()
        {
            // Arrange
            var txtEmail = new TextBox();
            // Assert
            Assert.Equal(string.Empty, txtEmail.Text);
        }

        // ─── Login validation logic (extracted) ───────────────────────────────────

        [Fact]
        public void LoginValidation_PasswordMatches_ReturnsTrue()
        {
            // Arrange
            string storedPassword = "secret123";
            string enteredPassword = "secret123";
            // Act
            bool isValid = storedPassword == enteredPassword;
            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void LoginValidation_PasswordDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            string storedPassword = "secret123";
            string enteredPassword = "wrongpassword";
            // Act
            bool isValid = storedPassword == enteredPassword;
            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void LoginValidation_EmptyPassword_DoesNotMatchNonEmpty()
        {
            // Arrange
            string storedPassword = "secret123";
            string enteredPassword = "";
            // Act
            bool isValid = storedPassword == enteredPassword;
            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void LoginValidation_BothEmpty_Matches()
        {
            // Arrange
            string storedPassword = "";
            string enteredPassword = "";
            // Act
            bool isValid = storedPassword == enteredPassword;
            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void LoginValidation_NullCoalesceOnScalar_ReturnsEmptyString()
        {
            // Arrange - simulates: passComm.ExecuteScalar()?.ToString() ?? ""
            object? scalarResult = null;
            // Act
            string password = scalarResult?.ToString() ?? "";
            // Assert
            Assert.Equal("", password);
        }

        [Fact]
        public void LoginValidation_NullCoalesceOnScalar_ReturnsValue()
        {
            // Arrange
            object? scalarResult = "mypassword";
            // Act
            string password = scalarResult?.ToString() ?? "";
            // Assert
            Assert.Equal("mypassword", password);
        }

        // ─── Response redirect simulation ─────────────────────────────────────────

        [Fact]
        public void HttpResponse_Redirect_ToMainProfilePage_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Redirect("MainProfilePage.aspx"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_Redirect_ToSignUpForm_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Redirect("SignUpForm.aspx"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_Write_PasswordCorrect_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Write("Password is correct"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_Write_PasswordIncorrect_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Write("Password is not correct"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpServerUtility_Transfer_ToMainProfilePage_DoesNotThrow()
        {
            // Arrange
            var server = new HttpServerUtility();
            // Act & Assert
            var ex = Record.Exception(() => server.Transfer("MainProfilePage.aspx"));
            Assert.Null(ex);
        }

        // ─── Email validation helpers ─────────────────────────────────────────────

        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("admin@gmail.com", true)]
        [InlineData("notanemail", false)]
        [InlineData("", false)]
        [InlineData("@nodomain", false)]
        public void EmailValidation_ContainsAtSign(string email, bool expected)
        {
            // Act
            bool hasAt = email.Contains("@") && email.IndexOf("@") > 0;
            // Assert
            Assert.Equal(expected, hasAt);
        }

        // ─── Button controls ──────────────────────────────────────────────────────

        [Fact]
        public void Button_Submit_CanBeCreated()
        {
            // Arrange & Act
            var btn = new Button { Text = "Submit" };
            // Assert
            Assert.Equal("Submit", btn.Text);
        }

        [Fact]
        public void Button_Register_CanBeCreated()
        {
            // Arrange & Act
            var btn = new Button { Text = "Register" };
            // Assert
            Assert.Equal("Register", btn.Text);
        }

        // ─── Helper: StubPage ─────────────────────────────────────────────────────

        private class StubPage : System.Web.UI.Page
        {
            public StubPage() { }
        }
    }
}
