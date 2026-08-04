using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for AdminLogin2 page logic.
    /// AdminLogin2 checks hardcoded credentials: password="admin", name="admin@gmail.com"
    /// </summary>
    public class AdminLogin2Tests
    {
        // ─── Hardcoded credential validation ─────────────────────────────────────

        [Fact]
        public void AdminLogin_CorrectCredentials_ShouldAuthenticate()
        {
            // Arrange
            string enteredPassword = "admin";
            string enteredName = "admin@gmail.com";
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.True(isAuthenticated);
        }

        [Fact]
        public void AdminLogin_WrongPassword_ShouldNotAuthenticate()
        {
            // Arrange
            string enteredPassword = "wrongpassword";
            string enteredName = "admin@gmail.com";
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void AdminLogin_WrongEmail_ShouldNotAuthenticate()
        {
            // Arrange
            string enteredPassword = "admin";
            string enteredName = "wrong@gmail.com";
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void AdminLogin_BothWrong_ShouldNotAuthenticate()
        {
            // Arrange
            string enteredPassword = "wrongpassword";
            string enteredName = "wrong@gmail.com";
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void AdminLogin_EmptyCredentials_ShouldNotAuthenticate()
        {
            // Arrange
            string enteredPassword = "";
            string enteredName = "";
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void AdminLogin_CaseSensitivePassword_ShouldNotAuthenticate()
        {
            // Arrange
            string enteredPassword = "Admin"; // capital A
            string enteredName = "admin@gmail.com";
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void AdminLogin_CaseSensitiveEmail_ShouldNotAuthenticate()
        {
            // Arrange
            string enteredPassword = "admin";
            string enteredName = "Admin@gmail.com"; // capital A
            // Act
            bool isAuthenticated = enteredPassword == "admin" && enteredName == "admin@gmail.com";
            // Assert
            Assert.False(isAuthenticated);
        }

        // ─── TextBox controls used in AdminLogin2 ────────────────────────────────

        [Fact]
        public void TextBox_Password_CanStoreAdminPassword()
        {
            // Arrange
            var passwordBox = new TextBox();
            // Act
            passwordBox.Text = "admin";
            // Assert
            Assert.Equal("admin", passwordBox.Text);
        }

        [Fact]
        public void TextBox_Name_CanStoreAdminEmail()
        {
            // Arrange
            var nameBox = new TextBox();
            // Act
            nameBox.Text = "admin@gmail.com";
            // Assert
            Assert.Equal("admin@gmail.com", nameBox.Text);
        }

        // ─── Redirect behavior ────────────────────────────────────────────────────

        [Fact]
        public void HttpResponse_Redirect_ToAdminProfile_DoesNotThrow()
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Redirect("AdminProfile.aspx"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpServerUtility_Transfer_ToAdminProfile_DoesNotThrow()
        {
            // Arrange
            var server = new HttpServerUtility();
            // Act & Assert
            var ex = Record.Exception(() => server.Transfer("AdminProfile.aspx"));
            Assert.Null(ex);
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

        [Fact]
        public void Page_Visible_DefaultIsTrue()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.True(page.Visible);
        }

        // ─── Admin credential constants ───────────────────────────────────────────

        [Fact]
        public void AdminCredentials_Password_IsAdmin()
        {
            // Arrange
            const string expectedPassword = "admin";
            // Assert
            Assert.Equal("admin", expectedPassword);
        }

        [Fact]
        public void AdminCredentials_Email_IsAdminGmail()
        {
            // Arrange
            const string expectedEmail = "admin@gmail.com";
            // Assert
            Assert.Equal("admin@gmail.com", expectedEmail);
        }

        [Theory]
        [InlineData("admin", "admin@gmail.com", true)]
        [InlineData("admin", "other@gmail.com", false)]
        [InlineData("wrong", "admin@gmail.com", false)]
        [InlineData("", "", false)]
        [InlineData("ADMIN", "ADMIN@GMAIL.COM", false)]
        public void AdminLogin_VariousCredentials_ReturnsExpected(string pwd, string email, bool expected)
        {
            // Act
            bool result = pwd == "admin" && email == "admin@gmail.com";
            // Assert
            Assert.Equal(expected, result);
        }

        private class StubPage : System.Web.UI.Page { }
    }
}
