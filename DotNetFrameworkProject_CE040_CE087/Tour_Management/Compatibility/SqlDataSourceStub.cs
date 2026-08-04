// SqlDataSourceStub.cs
// Stub for System.Web.UI.WebControls.SqlDataSource - not available in .NET 8.
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace System.Web.UI.WebControls
{
    /// <summary>
    /// Stub for System.Web.UI.WebControls.SqlDataSource - not available in .NET 8.
    /// SqlDataSource is a WebForms data source control that connects to SQL databases.
    /// This stub allows compilation; runtime functionality requires ASP.NET Core migration.
    /// </summary>
    public class SqlDataSource : UI.Control
    {
        public string? ConnectionString { get; set; }
        public string? SelectCommand { get; set; }
        public string? InsertCommand { get; set; }
        public string? UpdateCommand { get; set; }
        public string? DeleteCommand { get; set; }
        public string? ProviderName { get; set; }
        public SqlDataSourceMode DataSourceMode { get; set; }
        public ParameterCollection SelectParameters { get; } = new ParameterCollection();
        public ParameterCollection InsertParameters { get; } = new ParameterCollection();
        public ParameterCollection UpdateParameters { get; } = new ParameterCollection();
        public ParameterCollection DeleteParameters { get; } = new ParameterCollection();
        public event SqlDataSourceStatusEventHandler? Selected;
        public event SqlDataSourceStatusEventHandler? Inserted;
        public event SqlDataSourceStatusEventHandler? Updated;
        public event SqlDataSourceStatusEventHandler? Deleted;
        public System.Collections.IEnumerable? Select(DataSourceSelectArguments args) => null;
    }

    public enum SqlDataSourceMode { DataReader, DataSet }

    public class DataSourceSelectArguments
    {
        public static DataSourceSelectArguments Empty { get; } = new DataSourceSelectArguments();
        public string? SortExpression { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRowCount { get; set; }
    }

    public class ParameterCollection : Collections.Generic.List<Parameter> { }

    public class Parameter
    {
        public string? Name { get; set; }
        public object? DefaultValue { get; set; }
        public TypeCode Type { get; set; }
        public ParameterDirection Direction { get; set; }
    }

    public enum ParameterDirection { Input, Output, InputOutput, ReturnValue }

    public delegate void SqlDataSourceStatusEventHandler(object sender, SqlDataSourceStatusEventArgs e);
    public class SqlDataSourceStatusEventArgs : EventArgs
    {
        public int AffectedRows { get; set; }
        public Exception? Exception { get; set; }
        public bool ExceptionHandled { get; set; }
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8625
