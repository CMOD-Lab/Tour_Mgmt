using System;
using System.Web.UI.WebControls;
using Xunit;

namespace Tour_Management.Tests
{
    /// <summary>
    /// Tests for SqlDataSourceStub.cs - covers SqlDataSource, ParameterCollection,
    /// Parameter, DataSourceSelectArguments, and related types.
    /// </summary>
    public class SqlDataSourceStubTests
    {
        // ─── SqlDataSource ────────────────────────────────────────────────────────

        [Fact]
        public void SqlDataSource_DefaultConstructor_DoesNotThrow()
        {
            // Arrange & Act
            var ex = Record.Exception(() => new SqlDataSource());
            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void SqlDataSource_ConnectionString_CanBeSetAndGet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.ConnectionString = "Host=localhost;Database=test";
            // Assert
            Assert.Equal("Host=localhost;Database=test", ds.ConnectionString);
        }

        [Fact]
        public void SqlDataSource_SelectCommand_CanBeSetAndGet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.SelectCommand = "SELECT * FROM Tour";
            // Assert
            Assert.Equal("SELECT * FROM Tour", ds.SelectCommand);
        }

        [Fact]
        public void SqlDataSource_InsertCommand_CanBeSetAndGet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.InsertCommand = "INSERT INTO Tour VALUES(@name)";
            // Assert
            Assert.Equal("INSERT INTO Tour VALUES(@name)", ds.InsertCommand);
        }

        [Fact]
        public void SqlDataSource_UpdateCommand_CanBeSetAndGet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.UpdateCommand = "UPDATE Tour SET name=@name WHERE id=@id";
            // Assert
            Assert.Equal("UPDATE Tour SET name=@name WHERE id=@id", ds.UpdateCommand);
        }

        [Fact]
        public void SqlDataSource_DeleteCommand_CanBeSetAndGet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.DeleteCommand = "DELETE FROM Tour WHERE id=@id";
            // Assert
            Assert.Equal("DELETE FROM Tour WHERE id=@id", ds.DeleteCommand);
        }

        [Fact]
        public void SqlDataSource_ProviderName_CanBeSetAndGet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.ProviderName = "Npgsql";
            // Assert
            Assert.Equal("Npgsql", ds.ProviderName);
        }

        [Fact]
        public void SqlDataSource_DataSourceMode_DefaultIsDataReader()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Assert
            Assert.Equal(SqlDataSourceMode.DataReader, ds.DataSourceMode);
        }

        [Fact]
        public void SqlDataSource_DataSourceMode_CanBeSetToDataSet()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            ds.DataSourceMode = SqlDataSourceMode.DataSet;
            // Assert
            Assert.Equal(SqlDataSourceMode.DataSet, ds.DataSourceMode);
        }

        [Fact]
        public void SqlDataSource_SelectParameters_IsNotNull()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Assert
            Assert.NotNull(ds.SelectParameters);
        }

        [Fact]
        public void SqlDataSource_InsertParameters_IsNotNull()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Assert
            Assert.NotNull(ds.InsertParameters);
        }

        [Fact]
        public void SqlDataSource_UpdateParameters_IsNotNull()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Assert
            Assert.NotNull(ds.UpdateParameters);
        }

        [Fact]
        public void SqlDataSource_DeleteParameters_IsNotNull()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Assert
            Assert.NotNull(ds.DeleteParameters);
        }

        [Fact]
        public void SqlDataSource_Select_ReturnsNull()
        {
            // Arrange
            var ds = new SqlDataSource();
            // Act
            var result = ds.Select(DataSourceSelectArguments.Empty);
            // Assert
            Assert.Null(result);
        }

        // ─── ParameterCollection ──────────────────────────────────────────────────

        [Fact]
        public void ParameterCollection_DefaultConstructor_IsEmpty()
        {
            // Arrange & Act
            var col = new ParameterCollection();
            // Assert
            Assert.Empty(col);
        }

        [Fact]
        public void ParameterCollection_AddParameter_IncreasesCount()
        {
            // Arrange
            var col = new ParameterCollection();
            var param = new Parameter { Name = "id", DefaultValue = 1 };
            // Act
            col.Add(param);
            // Assert
            Assert.Single(col);
        }

        [Fact]
        public void ParameterCollection_CanAddMultipleParameters()
        {
            // Arrange
            var col = new ParameterCollection();
            // Act
            col.Add(new Parameter { Name = "name" });
            col.Add(new Parameter { Name = "email" });
            col.Add(new Parameter { Name = "password" });
            // Assert
            Assert.Equal(3, col.Count);
        }

        // ─── Parameter ────────────────────────────────────────────────────────────

        [Fact]
        public void Parameter_Name_CanBeSetAndGet()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.Name = "tourId";
            // Assert
            Assert.Equal("tourId", param.Name);
        }

        [Fact]
        public void Parameter_DefaultValue_CanBeSetAndGet()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.DefaultValue = 42;
            // Assert
            Assert.Equal(42, param.DefaultValue);
        }

        [Fact]
        public void Parameter_DefaultValue_CanBeString()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.DefaultValue = "test@example.com";
            // Assert
            Assert.Equal("test@example.com", param.DefaultValue);
        }

        [Fact]
        public void Parameter_DefaultValue_CanBeNull()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.DefaultValue = null;
            // Assert
            Assert.Null(param.DefaultValue);
        }

        [Fact]
        public void Parameter_Direction_DefaultIsInput()
        {
            // Arrange
            var param = new Parameter();
            // Assert
            Assert.Equal(ParameterDirection.Input, param.Direction);
        }

        [Fact]
        public void Parameter_Direction_CanBeSetToOutput()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.Direction = ParameterDirection.Output;
            // Assert
            Assert.Equal(ParameterDirection.Output, param.Direction);
        }

        [Fact]
        public void Parameter_Direction_CanBeSetToInputOutput()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.Direction = ParameterDirection.InputOutput;
            // Assert
            Assert.Equal(ParameterDirection.InputOutput, param.Direction);
        }

        [Fact]
        public void Parameter_Direction_CanBeSetToReturnValue()
        {
            // Arrange
            var param = new Parameter();
            // Act
            param.Direction = ParameterDirection.ReturnValue;
            // Assert
            Assert.Equal(ParameterDirection.ReturnValue, param.Direction);
        }

        // ─── DataSourceSelectArguments ────────────────────────────────────────────

        [Fact]
        public void DataSourceSelectArguments_Empty_IsNotNull()
        {
            // Assert
            Assert.NotNull(DataSourceSelectArguments.Empty);
        }

        [Fact]
        public void DataSourceSelectArguments_Empty_IsSameInstance()
        {
            // Act
            var a = DataSourceSelectArguments.Empty;
            var b = DataSourceSelectArguments.Empty;
            // Assert
            Assert.Same(a, b);
        }

        [Fact]
        public void DataSourceSelectArguments_SortExpression_CanBeSetAndGet()
        {
            // Arrange
            var args = new DataSourceSelectArguments();
            // Act
            args.SortExpression = "TourName ASC";
            // Assert
            Assert.Equal("TourName ASC", args.SortExpression);
        }

        [Fact]
        public void DataSourceSelectArguments_StartRowIndex_CanBeSetAndGet()
        {
            // Arrange
            var args = new DataSourceSelectArguments();
            // Act
            args.StartRowIndex = 10;
            // Assert
            Assert.Equal(10, args.StartRowIndex);
        }

        [Fact]
        public void DataSourceSelectArguments_MaximumRows_CanBeSetAndGet()
        {
            // Arrange
            var args = new DataSourceSelectArguments();
            // Act
            args.MaximumRows = 25;
            // Assert
            Assert.Equal(25, args.MaximumRows);
        }

        [Fact]
        public void DataSourceSelectArguments_TotalRowCount_CanBeSetAndGet()
        {
            // Arrange
            var args = new DataSourceSelectArguments();
            // Act
            args.TotalRowCount = 100;
            // Assert
            Assert.Equal(100, args.TotalRowCount);
        }

        // ─── SqlDataSourceStatusEventArgs ────────────────────────────────────────

        [Fact]
        public void SqlDataSourceStatusEventArgs_AffectedRows_CanBeSetAndGet()
        {
            // Arrange
            var args = new SqlDataSourceStatusEventArgs();
            // Act
            args.AffectedRows = 5;
            // Assert
            Assert.Equal(5, args.AffectedRows);
        }

        [Fact]
        public void SqlDataSourceStatusEventArgs_ExceptionHandled_DefaultIsFalse()
        {
            // Arrange
            var args = new SqlDataSourceStatusEventArgs();
            // Assert
            Assert.False(args.ExceptionHandled);
        }

        [Fact]
        public void SqlDataSourceStatusEventArgs_Exception_DefaultIsNull()
        {
            // Arrange
            var args = new SqlDataSourceStatusEventArgs();
            // Assert
            Assert.Null(args.Exception);
        }

        [Fact]
        public void SqlDataSourceStatusEventArgs_Exception_CanBeSetAndGet()
        {
            // Arrange
            var args = new SqlDataSourceStatusEventArgs();
            var ex = new InvalidOperationException("DB error");
            // Act
            args.Exception = ex;
            // Assert
            Assert.NotNull(args.Exception);
            Assert.Equal("DB error", args.Exception.Message);
        }

        // ─── SqlDataSourceMode enum ───────────────────────────────────────────────

        [Fact]
        public void SqlDataSourceMode_DataReader_HasValueZero()
        {
            Assert.Equal(0, (int)SqlDataSourceMode.DataReader);
        }

        [Fact]
        public void SqlDataSourceMode_DataSet_HasValueOne()
        {
            Assert.Equal(1, (int)SqlDataSourceMode.DataSet);
        }

        // ─── ParameterDirection enum ──────────────────────────────────────────────

        [Fact]
        public void ParameterDirection_Input_HasValueZero()
        {
            Assert.Equal(0, (int)ParameterDirection.Input);
        }

        [Fact]
        public void ParameterDirection_Output_HasValueOne()
        {
            Assert.Equal(1, (int)ParameterDirection.Output);
        }
    }
}
