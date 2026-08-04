// Compatibility shims for System.Web and System.Web.UI types not available in .NET 8.
// full runtime support.
// ============================================================================

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type

namespace System.Web
{
    /// <summary>
    /// Stub for System.Web.HttpContext - not available in .NET 8.
    /// </summary>
    public class HttpContext
    {
        public static HttpContext? Current { get; set; }
        public HttpRequest? Request { get; set; }
        public HttpResponse? Response { get; set; }
        public HttpServerUtility? Server { get; set; }
        public Collections.Specialized.NameValueCollection? Items { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.HttpRequest - not available in .NET 8.
    /// </summary>
    public class HttpRequest
    {
        public string? this[string key] => null;
        public Collections.Specialized.NameValueCollection QueryString { get; } = new();
        public Collections.Specialized.NameValueCollection Form { get; } = new();
    }

    /// <summary>
    /// Stub for System.Web.HttpResponse - not available in .NET 8.
    /// </summary>
    public class HttpResponse
    {
        public void Write(string s) { }
        public void Redirect(string url) { }
        public void Redirect(string url, bool endResponse) { }
        public void End() { }
    }

    /// <summary>
    /// Stub for System.Web.HttpServerUtility - not available in .NET 8.
    /// </summary>
    public class HttpServerUtility
    {
        public string MapPath(string path) => path;
        public void Transfer(string path) { }
        public void Transfer(string path, bool preserveForm) { }
        public string HtmlEncode(string s) => s;
        public string HtmlDecode(string s) => s;
        public string UrlEncode(string s) => s;
        public string UrlDecode(string s) => s;
    }

    /// <summary>
    /// Stub for System.Web.SessionState.HttpSessionState - not available in .NET 8.
    /// </summary>
    public class HttpSessionState
    {
        public object? this[string key]
        {
            get => null;
            set { }
        }
        public void Remove(string key) { }
        public void Clear() { }
        public void Abandon() { }
    }
}

namespace System.Web.UI
{
    /// <summary>
    /// Stub for System.Web.UI.Page - not available in .NET 8.
    /// ASP.NET WebForms Page class requires migration to ASP.NET Core Razor Pages.
    /// </summary>
    public abstract class Page : Control
    {
        public bool IsPostBack { get; set; } = false;
        public HttpRequest? Request { get; protected set; }
        public HttpResponse? Response { get; protected set; }
        public HttpServerUtility? Server { get; protected set; }
        public HttpSessionState? Session { get; protected set; }
        public ClientScriptManager? ClientScript { get; protected set; }

        protected virtual void Page_Load(object sender, EventArgs e) { }
    }

    /// <summary>
    /// Stub for System.Web.UI.Control - not available in .NET 8.
    /// </summary>
    public abstract class Control
    {
        public string? ID { get; set; }
        public bool Visible { get; set; } = true;
        public ControlCollection? Controls { get; protected set; }
        // Page property returns the containing Page - in derived Page classes, returns 'this'
        public virtual Page? Page { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.ControlCollection - not available in .NET 8.
    /// </summary>
    public class ControlCollection : Collections.Generic.List<Control> { }

    /// <summary>
    /// Stub for System.Web.UI.ClientScriptManager - not available in .NET 8.
    /// </summary>
    public class ClientScriptManager
    {
        public void RegisterStartupScript(Type type, string key, string script) { }
        public void RegisterStartupScript(Type type, string key, string script, bool addScriptTags) { }
    }
}

namespace System.Web.UI.HtmlControls
{
    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlControl - not available in .NET 8.
    /// </summary>
    public abstract class HtmlControl : UI.Control
    {
        public string? Style { get; set; }
        public Collections.Generic.Dictionary<string, string> Attributes { get; } = new();
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlContainerControl - not available in .NET 8.
    /// </summary>
    public abstract class HtmlContainerControl : HtmlControl
    {
        public string? InnerHtml { get; set; }
        public string? InnerText { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlForm - not available in .NET 8.
    /// </summary>
    public class HtmlForm : HtmlContainerControl
    {
        public string? Action { get; set; }
        public string? Method { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlInputControl - not available in .NET 8.
    /// </summary>
    public abstract class HtmlInputControl : HtmlControl
    {
        public string? Value { get; set; }
        public string? Type { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlInputText - not available in .NET 8.
    /// </summary>
    public class HtmlInputText : HtmlInputControl { }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlInputPassword - not available in .NET 8.
    /// </summary>
    public class HtmlInputPassword : HtmlInputControl { }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlInputButton - not available in .NET 8.
    /// </summary>
    public class HtmlInputButton : HtmlInputControl
    {
        public event EventHandler? ServerClick;
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlAnchor - not available in .NET 8.
    /// </summary>
    public class HtmlAnchor : HtmlContainerControl
    {
        public string? HRef { get; set; }
        public string? Target { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlImage - not available in .NET 8.
    /// </summary>
    public class HtmlImage : HtmlControl
    {
        public string? Src { get; set; }
        public string? Alt { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlTable - not available in .NET 8.
    /// </summary>
    public class HtmlTable : HtmlContainerControl { }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlTableRow - not available in .NET 8.
    /// </summary>
    public class HtmlTableRow : HtmlContainerControl { }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlTableCell - not available in .NET 8.
    /// </summary>
    public class HtmlTableCell : HtmlContainerControl { }

    /// <summary>
    /// Stub for System.Web.UI.HtmlControls.HtmlGenericControl - not available in .NET 8.
    /// </summary>
    public class HtmlGenericControl : HtmlContainerControl
    {
        public HtmlGenericControl() { }
        public HtmlGenericControl(string tag) { }
    }
}

namespace System.Web.UI.WebControls
{
    /// <summary>
    /// Stub for System.Web.UI.WebControls.WebControl - not available in .NET 8.
    /// </summary>
    public abstract class WebControl : UI.Control
    {
        public string? CssClass { get; set; }
        public string? ToolTip { get; set; }
        public bool Enabled { get; set; } = true;
        public Drawing.Color ForeColor { get; set; }
        public Drawing.Color BackColor { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public Collections.Generic.Dictionary<string, string> Attributes { get; } = new();
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Unit - not available in .NET 8.
    /// </summary>
    public struct Unit
    {
        public double Value { get; }
        public UnitType Type { get; }
        public Unit(double value) { Value = value; Type = UnitType.Pixel; }
        public Unit(double value, UnitType type) { Value = value; Type = type; }
        public static Unit Parse(string s) => new Unit(0);
        public static Unit Pixel(int n) => new Unit(n, UnitType.Pixel);
        public static Unit Percentage(double n) => new Unit(n, UnitType.Percentage);
        public override string ToString() => $"{Value}px";
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.UnitType - not available in .NET 8.
    /// </summary>
    public enum UnitType { Pixel, Point, Pica, Inch, Mm, Cm, Percentage, Em, Ex }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Label - not available in .NET 8.
    /// </summary>
    public class Label : WebControl
    {
        public string? Text { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.TextBox - not available in .NET 8.
    /// </summary>
    public class TextBox : WebControl
    {
        public string Text { get; set; } = string.Empty;
        public TextBoxMode TextMode { get; set; }
        public int MaxLength { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool ReadOnly { get; set; }
        public event EventHandler? TextChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.TextBoxMode - not available in .NET 8.
    /// </summary>
    public enum TextBoxMode { SingleLine, MultiLine, Password, Color, Date, DateTime, DateTimeLocal, Email, Month, Number, Range, Search, Phone, Time, Url, Week }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Button - not available in .NET 8.
    /// </summary>
    public class Button : WebControl
    {
        public string? Text { get; set; }
        public string? CommandName { get; set; }
        public string? CommandArgument { get; set; }
        public bool UseSubmitBehavior { get; set; } = true;
        public event EventHandler? Click;
        public event CommandEventHandler? Command;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.LinkButton - not available in .NET 8.
    /// </summary>
    public class LinkButton : WebControl
    {
        public string? Text { get; set; }
        public string? CommandName { get; set; }
        public string? CommandArgument { get; set; }
        public event EventHandler? Click;
        public event CommandEventHandler? Command;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ImageButton - not available in .NET 8.
    /// </summary>
    public class ImageButton : WebControl
    {
        public string? ImageUrl { get; set; }
        public string? AlternateText { get; set; }
        public event ImageClickEventHandler? Click;
        public event CommandEventHandler? Command;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.HyperLink - not available in .NET 8.
    /// </summary>
    public class HyperLink : WebControl
    {
        public string? Text { get; set; }
        public string? NavigateUrl { get; set; }
        public string? Target { get; set; }
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Image - not available in .NET 8.
    /// </summary>
    public class Image : WebControl
    {
        public string? ImageUrl { get; set; }
        public string? AlternateText { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.CheckBox - not available in .NET 8.
    /// </summary>
    public class CheckBox : WebControl
    {
        public string? Text { get; set; }
        public bool Checked { get; set; }
        public event EventHandler? CheckedChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.RadioButton - not available in .NET 8.
    /// </summary>
    public class RadioButton : CheckBox
    {
        public string? GroupName { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.DropDownList - not available in .NET 8.
    /// </summary>
    public class DropDownList : ListControl
    {
        public string? Text { get => SelectedValue; set => SelectedValue = value; }
        public event EventHandler? SelectedIndexChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ListBox - not available in .NET 8.
    /// </summary>
    public class ListBox : ListControl
    {
        public int Rows { get; set; }
        public ListSelectionMode SelectionMode { get; set; }
        public event EventHandler? SelectedIndexChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ListSelectionMode - not available in .NET 8.
    /// </summary>
    public enum ListSelectionMode { Single, Multiple }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ListControl - not available in .NET 8.
    /// </summary>
    public abstract class ListControl : WebControl
    {
        public ListItemCollection Items { get; } = new ListItemCollection();
        public int SelectedIndex { get; set; } = -1;
        public ListItem? SelectedItem => SelectedIndex >= 0 && SelectedIndex < Items.Count ? Items[SelectedIndex] : null;
        public string? SelectedValue { get; set; }
        public string? DataSource { get; set; }
        public string? DataTextField { get; set; }
        public string? DataValueField { get; set; }
        public void DataBind() { }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ListItemCollection - not available in .NET 8.
    /// </summary>
    public class ListItemCollection : Collections.Generic.List<ListItem>
    {
        public void Add(string text) => Add(new ListItem(text));
        public void Add(string text, string value) => Add(new ListItem(text, value));
        public bool Contains(ListItem item) => base.Contains(item);
        public void Remove(string text) { }
        public void Remove(ListItem item) => base.Remove(item);
        public ListItem? FindByText(string text) => Find(i => i.Text == text);
        public ListItem? FindByValue(string value) => Find(i => i.Value == value);
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ListItem - not available in .NET 8.
    /// </summary>
    public class ListItem
    {
        public string Text { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public bool Enabled { get; set; } = true;
        public ListItem() { }
        public ListItem(string text) { Text = text; Value = text; }
        public ListItem(string text, string value) { Text = text; Value = value; }
        public ListItem(string text, string value, bool enabled) { Text = text; Value = value; Enabled = enabled; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.GridView - not available in .NET 8.
    /// </summary>
    public class GridView : WebControl
    {
        public object? DataSource { get; set; }
        public bool AllowPaging { get; set; }
        public bool AllowSorting { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; }
        public bool AutoGenerateColumns { get; set; } = true;
        public bool AutoGenerateEditButton { get; set; }
        public bool AutoGenerateDeleteButton { get; set; }
        public string? DataKeyNames { get; set; }
        public DataKeyArray? DataKeys { get; protected set; }
        public GridViewRowCollection? Rows { get; protected set; }
        public event GridViewRowEventHandler? RowDataBound;
        public event GridViewEditEventHandler? RowEditing;
        public event GridViewDeleteEventHandler? RowDeleting;
        public event GridViewUpdateEventHandler? RowUpdating;
        public event GridViewCancelEditEventHandler? RowCancelingEdit;
        public event GridViewPageEventHandler? PageIndexChanging;
        public void DataBind() { }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.GridViewRowCollection - not available in .NET 8.
    /// </summary>
    public class GridViewRowCollection : Collections.Generic.List<GridViewRow> { }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.GridViewRow - not available in .NET 8.
    /// </summary>
    public class GridViewRow : UI.Control
    {
        public int RowIndex { get; set; }
        public DataControlRowType RowType { get; set; }
        public DataControlRowState RowState { get; set; }
        public TableCellCollection? Cells { get; protected set; }
        public object? DataItem { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.DataControlRowType - not available in .NET 8.
    /// </summary>
    public enum DataControlRowType { Header, Footer, DataRow, Separator, Pager, EmptyDataRow }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.DataControlRowState - not available in .NET 8.
    /// </summary>
    public enum DataControlRowState { Normal, Alternate, Edit, Selected }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.TableCellCollection - not available in .NET 8.
    /// </summary>
    public class TableCellCollection : Collections.Generic.List<TableCell> { }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.TableCell - not available in .NET 8.
    /// </summary>
    public class TableCell : WebControl
    {
        public string? Text { get; set; }
        public T? FindControl<T>(string id) where T : UI.Control => null;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.DataKeyArray - not available in .NET 8.
    /// </summary>
    public class DataKeyArray : Collections.Generic.List<DataKey> { }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.DataKey - not available in .NET 8.
    /// </summary>
    public class DataKey
    {
        public object? Value { get; set; }
        public object? this[string key] => null;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Repeater - not available in .NET 8.
    /// </summary>
    public class Repeater : UI.Control
    {
        public object? DataSource { get; set; }
        public event RepeaterItemEventHandler? ItemDataBound;
        public event RepeaterCommandEventHandler? ItemCommand;
        public void DataBind() { }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.DataList - not available in .NET 8.
    /// </summary>
    public class DataList : UI.Control
    {
        public object? DataSource { get; set; }
        public event DataListItemEventHandler? ItemDataBound;
        public event DataListCommandEventHandler? ItemCommand;
        public void DataBind() { }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.FileUpload - not available in .NET 8.
    /// </summary>
    public class FileUpload : WebControl
    {
        public string FileName { get; set; } = string.Empty;
        public bool HasFile { get; set; }
        public IO.Stream? FileContent { get; set; }
        public HttpPostedFile? PostedFile { get; set; }
        public void SaveAs(string filename) { }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Panel - not available in .NET 8.
    /// </summary>
    public class Panel : WebControl
    {
        public string? GroupingText { get; set; }
        public ScrollBars ScrollBars { get; set; }
        public string? DefaultButton { get; set; }
        public string? Direction { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ScrollBars - not available in .NET 8.
    /// </summary>
    public enum ScrollBars { None, Horizontal, Vertical, Both, Auto }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.PlaceHolder - not available in .NET 8.
    /// </summary>
    public class PlaceHolder : UI.Control { }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Literal - not available in .NET 8.
    /// </summary>
    public class Literal : UI.Control
    {
        public string? Text { get; set; }
        public LiteralMode Mode { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.LiteralMode - not available in .NET 8.
    /// </summary>
    public enum LiteralMode { Transform, PassThrough, Encode }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Table - not available in .NET 8.
    /// </summary>
    public class Table : WebControl
    {
        public TableRowCollection? Rows { get; protected set; }
        public string? Caption { get; set; }
        public string? CellPadding { get; set; }
        public string? CellSpacing { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.TableRowCollection - not available in .NET 8.
    /// </summary>
    public class TableRowCollection : Collections.Generic.List<TableRow> { }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.TableRow - not available in .NET 8.
    /// </summary>
    public class TableRow : WebControl
    {
        public TableCellCollection? Cells { get; protected set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.RequiredFieldValidator - not available in .NET 8.
    /// </summary>
    public class RequiredFieldValidator : BaseValidator
    {
        public string? InitialValue { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.RegularExpressionValidator - not available in .NET 8.
    /// </summary>
    public class RegularExpressionValidator : BaseValidator
    {
        public string? ValidationExpression { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.RangeValidator - not available in .NET 8.
    /// </summary>
    public class RangeValidator : BaseValidator
    {
        public string? MinimumValue { get; set; }
        public string? MaximumValue { get; set; }
        public ValidationDataType Type { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.CompareValidator - not available in .NET 8.
    /// </summary>
    public class CompareValidator : BaseValidator
    {
        public string? ControlToCompare { get; set; }
        public string? ValueToCompare { get; set; }
        public ValidationCompareOperator Operator { get; set; }
        public ValidationDataType Type { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.CustomValidator - not available in .NET 8.
    /// </summary>
    public class CustomValidator : BaseValidator
    {
        public string? ClientValidationFunction { get; set; }
        public event ServerValidateEventHandler? ServerValidate;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ValidationSummary - not available in .NET 8.
    /// </summary>
    public class ValidationSummary : WebControl
    {
        public string? ValidationGroup { get; set; }
        public bool ShowSummary { get; set; } = true;
        public bool ShowMessageBox { get; set; }
        public ValidationSummaryDisplayMode DisplayMode { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ValidationSummaryDisplayMode - not available in .NET 8.
    /// </summary>
    public enum ValidationSummaryDisplayMode { List, BulletList, SingleParagraph }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.BaseValidator - not available in .NET 8.
    /// </summary>
    public abstract class BaseValidator : Label
    {
        public string? ControlToValidate { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ValidationGroup { get; set; }
        public bool IsValid { get; set; } = true;
        public ValidatorDisplay Display { get; set; }
        public bool SetFocusOnError { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ValidatorDisplay - not available in .NET 8.
    /// </summary>
    public enum ValidatorDisplay { None, Static, Dynamic }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ValidationDataType - not available in .NET 8.
    /// </summary>
    public enum ValidationDataType { String, Integer, Double, Date, Currency }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.ValidationCompareOperator - not available in .NET 8.
    /// </summary>
    public enum ValidationCompareOperator { Equal, NotEqual, GreaterThan, GreaterThanEqual, LessThan, LessThanEqual, DataTypeCheck }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Calendar - not available in .NET 8.
    /// </summary>
    public class Calendar : WebControl
    {
        public DateTime SelectedDate { get; set; }
        public DateTime TodaysDate { get; set; } = DateTime.Today;
        public event EventHandler? SelectionChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.BulletedList - not available in .NET 8.
    /// </summary>
    public class BulletedList : ListControl
    {
        public BulletStyle BulletStyle { get; set; }
        public BulletedListDisplayMode DisplayMode { get; set; }
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.BulletStyle - not available in .NET 8.
    /// </summary>
    public enum BulletStyle { NotSet, Numbered, LowerAlpha, UpperAlpha, LowerRoman, UpperRoman, Disc, Circle, Square, CustomImage }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.BulletedListDisplayMode - not available in .NET 8.
    /// </summary>
    public enum BulletedListDisplayMode { Text, HyperLink, LinkButton }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.CheckBoxList - not available in .NET 8.
    /// </summary>
    public class CheckBoxList : ListControl
    {
        public event EventHandler? SelectedIndexChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.RadioButtonList - not available in .NET 8.
    /// </summary>
    public class RadioButtonList : ListControl
    {
        public event EventHandler? SelectedIndexChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.MultiView - not available in .NET 8.
    /// </summary>
    public class MultiView : UI.Control
    {
        public int ActiveViewIndex { get; set; } = -1;
        public event EventHandler? ActiveViewChanged;
    }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.View - not available in .NET 8.
    /// </summary>
    public class View : UI.Control { }

    /// <summary>
    /// Stub for System.Web.UI.WebControls.Wizard - not available in .NET 8.
    /// </summary>
    public class Wizard : UI.Control
    {
        public int ActiveStepIndex { get; set; }
        public event WizardNavigationEventHandler? NextButtonClick;
        public event WizardNavigationEventHandler? PreviousButtonClick;
        public event EventHandler? FinishButtonClick;
    }

    // Event handler delegates
    public delegate void CommandEventHandler(object sender, CommandEventArgs e);
    public delegate void ImageClickEventHandler(object sender, ImageClickEventArgs e);
    public delegate void GridViewRowEventHandler(object sender, GridViewRowEventArgs e);
    public delegate void GridViewEditEventHandler(object sender, GridViewEditEventArgs e);
    public delegate void GridViewDeleteEventHandler(object sender, GridViewDeleteEventArgs e);
    public delegate void GridViewUpdateEventHandler(object sender, GridViewUpdateEventArgs e);
    public delegate void GridViewCancelEditEventHandler(object sender, GridViewCancelEditEventArgs e);
    public delegate void GridViewPageEventHandler(object sender, GridViewPageEventArgs e);
    public delegate void RepeaterItemEventHandler(object sender, RepeaterItemEventArgs e);
    public delegate void RepeaterCommandEventHandler(object sender, RepeaterCommandEventArgs e);
    public delegate void DataListItemEventHandler(object sender, DataListItemEventArgs e);
    public delegate void DataListCommandEventHandler(object sender, DataListCommandEventArgs e);
    public delegate void ServerValidateEventHandler(object sender, ServerValidateEventArgs e);
    public delegate void WizardNavigationEventHandler(object sender, WizardNavigationEventArgs e);

    // Event args stubs
    public class CommandEventArgs : EventArgs
    {
        public string? CommandName { get; set; }
        public object? CommandArgument { get; set; }
    }
    public class ImageClickEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class GridViewRowEventArgs : EventArgs { public GridViewRow? Row { get; set; } }
    public class GridViewEditEventArgs : EventArgs { public int NewEditIndex { get; set; } }
    public class GridViewDeleteEventArgs : EventArgs { public int RowIndex { get; set; } }
    public class GridViewUpdateEventArgs : EventArgs { public int RowIndex { get; set; } }
    public class GridViewCancelEditEventArgs : EventArgs { public int RowIndex { get; set; } }
    public class GridViewPageEventArgs : EventArgs { public int NewPageIndex { get; set; } }
    public class RepeaterItemEventArgs : EventArgs { public RepeaterItem? Item { get; set; } }
    public class RepeaterCommandEventArgs : EventArgs { public RepeaterItem? Item { get; set; } public string? CommandName { get; set; } }
    public class DataListItemEventArgs : EventArgs { public DataListItem? Item { get; set; } }
    public class DataListCommandEventArgs : EventArgs { public DataListItem? Item { get; set; } public string? CommandName { get; set; } }
    public class ServerValidateEventArgs : EventArgs { public string? Value { get; set; } public bool IsValid { get; set; } }
    public class WizardNavigationEventArgs : EventArgs { public int CurrentStepIndex { get; set; } public int NextStepIndex { get; set; } }

    // Additional stub types
    public class RepeaterItem : UI.Control { public object? DataItem { get; set; } }
    public class DataListItem : UI.Control { public object? DataItem { get; set; } }
}

namespace System.Web

{
    /// <summary>
    /// Stub for System.Web.HttpPostedFile - not available in .NET 8.
    /// </summary>
    public class HttpPostedFile
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public int ContentLength { get; set; }
        public IO.Stream? InputStream { get; set; }
        public void SaveAs(string filename) { }
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8625
