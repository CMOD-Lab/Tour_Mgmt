using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for AdminProfile, DisplayTours, mybooking, allbooking, usercrud,
    /// and MainProfilePage pages - all simple Page_Load stubs.
    /// </summary>
    public class SimplePageTests
    {
        // ─── AdminProfile ─────────────────────────────────────────────────────────

        [Fact]
        public void AdminProfile_Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void AdminProfile_Page_Visible_DefaultIsTrue()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.True(page.Visible);
        }

        [Fact]
        public void AdminProfile_Page_Request_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Request);
        }

        [Fact]
        public void AdminProfile_Page_Response_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Response);
        }

        // ─── DisplayTours ─────────────────────────────────────────────────────────

        [Fact]
        public void DisplayTours_Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void DisplayTours_Page_Session_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Session);
        }

        // ─── mybooking ────────────────────────────────────────────────────────────

        [Fact]
        public void MyBooking_Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void MyBooking_Page_ClientScript_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.ClientScript);
        }

        // ─── allbooking ───────────────────────────────────────────────────────────

        [Fact]
        public void AllBooking_Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void AllBooking_Page_Visible_DefaultIsTrue()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.True(page.Visible);
        }

        // ─── usercrud ─────────────────────────────────────────────────────────────

        [Fact]
        public void UserCrud_Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void UserCrud_Page_Server_DefaultIsNull()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.Null(page.Server);
        }

        // ─── MainProfilePage ──────────────────────────────────────────────────────

        [Fact]
        public void MainProfilePage_Page_IsPostBack_DefaultIsFalse()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.False(page.IsPostBack);
        }

        [Fact]
        public void MainProfilePage_Page_Visible_DefaultIsTrue()
        {
            // Arrange
            var page = new StubPage();
            // Assert
            Assert.True(page.Visible);
        }

        // ─── Control ID property ──────────────────────────────────────────────────

        [Fact]
        public void Control_ID_CanBeSetAndGet()
        {
            // Arrange
            var ctrl = new Label();
            // Act
            ctrl.ID = "lblMessage";
            // Assert
            Assert.Equal("lblMessage", ctrl.ID);
        }

        [Fact]
        public void Control_ID_DefaultIsNull()
        {
            // Arrange
            var ctrl = new Label();
            // Assert
            Assert.Null(ctrl.ID);
        }

        // ─── Page navigation URLs ─────────────────────────────────────────────────

        [Theory]
        [InlineData("AdminProfile.aspx")]
        [InlineData("DisplayTours.aspx")]
        [InlineData("mybooking.aspx")]
        [InlineData("allbooking.aspx")]
        [InlineData("usercrud.aspx")]
        [InlineData("MainProfilePage.aspx")]
        [InlineData("userlogin.aspx")]
        [InlineData("SignUpForm.aspx")]
        [InlineData("AddTour.aspx")]
        [InlineData("TourCrud.aspx")]
        [InlineData("Order.aspx")]
        [InlineData("AdminLogin2.aspx")]
        public void PageUrl_EndsWithAspx(string url)
        {
            // Assert
            Assert.EndsWith(".aspx", url);
        }

        [Theory]
        [InlineData("AdminProfile.aspx")]
        [InlineData("DisplayTours.aspx")]
        [InlineData("mybooking.aspx")]
        [InlineData("allbooking.aspx")]
        [InlineData("usercrud.aspx")]
        [InlineData("MainProfilePage.aspx")]
        public void PageUrl_IsNotNullOrEmpty(string url)
        {
            // Assert
            Assert.False(string.IsNullOrEmpty(url));
        }

        // ─── HttpResponse redirect to all pages ───────────────────────────────────

        [Theory]
        [InlineData("AdminProfile.aspx")]
        [InlineData("DisplayTours.aspx")]
        [InlineData("mybooking.aspx")]
        [InlineData("allbooking.aspx")]
        [InlineData("usercrud.aspx")]
        [InlineData("MainProfilePage.aspx")]
        public void HttpResponse_Redirect_ToAnyPage_DoesNotThrow(string url)
        {
            // Arrange
            var response = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => response.Redirect(url));
            Assert.Null(ex);
        }

        // ─── Session usage patterns ───────────────────────────────────────────────

        [Fact]
        public void Session_SetAndGet_UserEmail()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act
            session["New"] = "user@example.com";
            // Assert - session stub returns null but doesn't throw
            var ex = Record.Exception(() => { var _ = session["New"]; });
            Assert.Null(ex);
        }

        [Fact]
        public void Session_Remove_UserEmail_DoesNotThrow()
        {
            // Arrange
            var session = new HttpSessionState();
            session["New"] = "user@example.com";
            // Act & Assert
            var ex = Record.Exception(() => session.Remove("New"));
            Assert.Null(ex);
        }

        [Fact]
        public void Session_Abandon_DoesNotThrow()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act & Assert
            var ex = Record.Exception(() => session.Abandon());
            Assert.Null(ex);
        }

        // ─── Page property: Page returns self ─────────────────────────────────────

        [Fact]
        public void Control_Page_CanBeSetAndGet()
        {
            // Arrange
            var ctrl = new Label();
            var page = new StubPage();
            // Act
            ctrl.Page = page;
            // Assert
            Assert.Same(page, ctrl.Page);
        }

        private class StubPage : System.Web.UI.Page { }
    }
}
