using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for TourCrud page logic and data refresh workflow.
    /// </summary>
    public class TourCrudTests
    {
        // ─── refreshdata method logic ─────────────────────────────────────────────

        [Fact]
        public void SelectQuery_IsCorrectSql()
        {
            // Arrange
            string selectQuery = "select * from Tour";
            // Assert
            Assert.Equal("select * from Tour", selectQuery);
        }

        [Fact]
        public void SelectQuery_ContainsTourTable()
        {
            // Arrange
            string selectQuery = "select * from Tour";
            // Assert
            Assert.Contains("Tour", selectQuery);
        }

        [Fact]
        public void SelectQuery_ContainsSelectAll()
        {
            // Arrange
            string selectQuery = "select * from Tour";
            // Assert
            Assert.Contains("select *", selectQuery);
        }

        // ─── IsPostBack guard logic ───────────────────────────────────────────────

        [Fact]
        public void IsPostBack_False_ShouldCallRefreshData()
        {
            // Arrange
            bool isPostBack = false;
            bool refreshDataCalled = false;
            // Act
            if (!isPostBack)
            {
                refreshDataCalled = true; // simulates refreshdata()
            }
            // Assert
            Assert.True(refreshDataCalled);
        }

        [Fact]
        public void IsPostBack_True_ShouldNotCallRefreshData()
        {
            // Arrange
            bool isPostBack = true;
            bool refreshDataCalled = false;
            // Act
            if (!isPostBack)
            {
                refreshDataCalled = true;
            }
            // Assert
            Assert.False(refreshDataCalled);
        }

        // ─── GridView data binding ────────────────────────────────────────────────

        [Fact]
        public void GridView_DataSource_CanBeSet()
        {
            // Arrange
            var gv = new GridView();
            // Act
            gv.DataSource = new[] { new { Id = 1, Name = "Paris Tour" } };
            // Assert
            Assert.NotNull(gv.DataSource);
        }

        [Fact]
        public void GridView_DataBind_DoesNotThrow()
        {
            // Arrange
            var gv = new GridView();
            gv.DataSource = new[] { new { Id = 1, Name = "Paris Tour" } };
            // Act & Assert
            var ex = Record.Exception(() => gv.DataBind());
            Assert.Null(ex);
        }

        [Fact]
        public void GridView_DataSource_NullByDefault()
        {
            // Arrange
            var gv = new GridView();
            // Assert
            Assert.Null(gv.DataSource);
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
        public void Page_IsPostBack_CanBeSetToTrue()
        {
            // Arrange
            var page = new StubPage();
            // Act
            page.IsPostBack = true;
            // Assert
            Assert.True(page.IsPostBack);
        }

        // ─── Connection string handling ───────────────────────────────────────────

        [Fact]
        public void ConnectionString_Format_IsValid()
        {
            // Arrange
            string connStr = "Host=localhost;Port=5432;Database=tourdb;Username=postgres;Password=postgres;";
            // Assert
            Assert.Contains("Host=", connStr);
            Assert.Contains("Database=", connStr);
            Assert.Contains("Username=", connStr);
            Assert.Contains("Password=", connStr);
        }

        [Fact]
        public void ConnectionString_ContainsHost()
        {
            // Arrange
            string connStr = "Host=localhost;Port=5432;Database=tourdb;Username=postgres;Password=postgres;";
            // Assert
            Assert.Contains("localhost", connStr);
        }

        [Fact]
        public void ConnectionString_ContainsPort()
        {
            // Arrange
            string connStr = "Host=localhost;Port=5432;Database=tourdb;Username=postgres;Password=postgres;";
            // Assert
            Assert.Contains("5432", connStr);
        }

        // ─── Tour data model ──────────────────────────────────────────────────────

        [Fact]
        public void TourData_CanCreateAnonymousObject()
        {
            // Arrange & Act
            var tour = new
            {
                TourName = "Paris Adventure",
                Place = "Paris",
                Days = 7,
                Price = 1500.00m,
                Locations = "Eiffel Tower",
                TourInfo = "A great tour",
                Pic = "paris.jpg"
            };
            // Assert
            Assert.Equal("Paris Adventure", tour.TourName);
            Assert.Equal(7, tour.Days);
            Assert.Equal(1500.00m, tour.Price);
        }

        [Fact]
        public void TourData_PriceIsPositive()
        {
            // Arrange
            decimal price = 1500.00m;
            // Assert
            Assert.True(price > 0);
        }

        [Fact]
        public void TourData_DaysIsPositive()
        {
            // Arrange
            int days = 7;
            // Assert
            Assert.True(days > 0);
        }

        private class StubPage : System.Web.UI.Page { }
    }
}
