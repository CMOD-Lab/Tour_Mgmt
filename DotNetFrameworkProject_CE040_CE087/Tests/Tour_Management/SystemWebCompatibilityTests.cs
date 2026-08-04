using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Xunit;

namespace Tour_Management.Tests
{
    public class SystemWebCompatibilityTests
    {
        // ─── HttpContext ───────────────────────────────────────────────────────────

        [Fact]
        public void HttpContext_Current_DefaultsToNull()
        {
            // Arrange & Act
            var ctx = HttpContext.Current;
            // Assert
            Assert.Null(ctx);
        }

        [Fact]
        public void HttpContext_CanSetAndGetCurrent()
        {
            // Arrange
            var ctx = new HttpContext();
            // Act
            HttpContext.Current = ctx;
            // Assert
            Assert.NotNull(HttpContext.Current);
            // Cleanup
            HttpContext.Current = null;
        }

        [Fact]
        public void HttpContext_PropertiesAreNullByDefault()
        {
            // Arrange
            var ctx = new HttpContext();
            // Assert
            Assert.Null(ctx.Request);
            Assert.Null(ctx.Response);
            Assert.Null(ctx.Server);
            Assert.Null(ctx.Items);
        }

        // ─── HttpRequest ──────────────────────────────────────────────────────────

        [Fact]
        public void HttpRequest_IndexerReturnsNull()
        {
            // Arrange
            var req = new HttpRequest();
            // Act
            var val = req["anykey"];
            // Assert
            Assert.Null(val);
        }

        [Fact]
        public void HttpRequest_QueryStringIsNotNull()
        {
            // Arrange
            var req = new HttpRequest();
            // Assert
            Assert.NotNull(req.QueryString);
        }

        [Fact]
        public void HttpRequest_FormIsNotNull()
        {
            // Arrange
            var req = new HttpRequest();
            // Assert
            Assert.NotNull(req.Form);
        }

        // ─── HttpResponse ─────────────────────────────────────────────────────────

        [Fact]
        public void HttpResponse_Write_DoesNotThrow()
        {
            // Arrange
            var resp = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => resp.Write("hello"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_Redirect_DoesNotThrow()
        {
            // Arrange
            var resp = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => resp.Redirect("http://example.com"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_RedirectWithEndResponse_DoesNotThrow()
        {
            // Arrange
            var resp = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => resp.Redirect("http://example.com", false));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpResponse_End_DoesNotThrow()
        {
            // Arrange
            var resp = new HttpResponse();
            // Act & Assert
            var ex = Record.Exception(() => resp.End());
            Assert.Null(ex);
        }

        // ─── HttpServerUtility ────────────────────────────────────────────────────

        [Fact]
        public void HttpServerUtility_MapPath_ReturnsSamePath()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act
            var result = srv.MapPath("~/somepath");
            // Assert
            Assert.Equal("~/somepath", result);
        }

        [Fact]
        public void HttpServerUtility_HtmlEncode_ReturnsSameString()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act
            var result = srv.HtmlEncode("<b>test</b>");
            // Assert
            Assert.Equal("<b>test</b>", result);
        }

        [Fact]
        public void HttpServerUtility_HtmlDecode_ReturnsSameString()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act
            var result = srv.HtmlDecode("&lt;b&gt;");
            // Assert
            Assert.Equal("&lt;b&gt;", result);
        }

        [Fact]
        public void HttpServerUtility_UrlEncode_ReturnsSameString()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act
            var result = srv.UrlEncode("hello world");
            // Assert
            Assert.Equal("hello world", result);
        }

        [Fact]
        public void HttpServerUtility_UrlDecode_ReturnsSameString()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act
            var result = srv.UrlDecode("hello+world");
            // Assert
            Assert.Equal("hello+world", result);
        }

        [Fact]
        public void HttpServerUtility_Transfer_DoesNotThrow()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act & Assert
            var ex = Record.Exception(() => srv.Transfer("page.aspx"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpServerUtility_TransferWithPreserveForm_DoesNotThrow()
        {
            // Arrange
            var srv = new HttpServerUtility();
            // Act & Assert
            var ex = Record.Exception(() => srv.Transfer("page.aspx", true));
            Assert.Null(ex);
        }

        // ─── HttpSessionState ─────────────────────────────────────────────────────

        [Fact]
        public void HttpSessionState_IndexerGetReturnsNull()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act
            var val = session["key"];
            // Assert
            Assert.Null(val);
        }

        [Fact]
        public void HttpSessionState_IndexerSet_DoesNotThrow()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act & Assert
            var ex = Record.Exception(() => session["key"] = "value");
            Assert.Null(ex);
        }

        [Fact]
        public void HttpSessionState_Remove_DoesNotThrow()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act & Assert
            var ex = Record.Exception(() => session.Remove("key"));
            Assert.Null(ex);
        }

        [Fact]
        public void HttpSessionState_Clear_DoesNotThrow()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act & Assert
            var ex = Record.Exception(() => session.Clear());
            Assert.Null(ex);
        }

        [Fact]
        public void HttpSessionState_Abandon_DoesNotThrow()
        {
            // Arrange
            var session = new HttpSessionState();
            // Act & Assert
            var ex = Record.Exception(() => session.Abandon());
            Assert.Null(ex);
        }

        // ─── ClientScriptManager ──────────────────────────────────────────────────

        [Fact]
        public void ClientScriptManager_RegisterStartupScript_DoesNotThrow()
        {
            // Arrange
            var csm = new ClientScriptManager();
            // Act & Assert
            var ex = Record.Exception(() => csm.RegisterStartupScript(typeof(object), "key", "alert('hi');"));
            Assert.Null(ex);
        }

        [Fact]
        public void ClientScriptManager_RegisterStartupScriptWithTag_DoesNotThrow()
        {
            // Arrange
            var csm = new ClientScriptManager();
            // Act & Assert
            var ex = Record.Exception(() => csm.RegisterStartupScript(typeof(object), "key", "alert('hi');", true));
            Assert.Null(ex);
        }

        // ─── WebControls: Label ───────────────────────────────────────────────────

        [Fact]
        public void Label_TextProperty_CanBeSetAndGet()
        {
            // Arrange
            var lbl = new Label();
            // Act
            lbl.Text = "Hello";
            // Assert
            Assert.Equal("Hello", lbl.Text);
        }

        [Fact]
        public void Label_DefaultTextIsNull()
        {
            // Arrange
            var lbl = new Label();
            // Assert
            Assert.Null(lbl.Text);
        }

        // ─── WebControls: TextBox ─────────────────────────────────────────────────

        [Fact]
        public void TextBox_DefaultTextIsEmpty()
        {
            // Arrange
            var tb = new TextBox();
            // Assert
            Assert.Equal(string.Empty, tb.Text);
        }

        [Fact]
        public void TextBox_TextProperty_CanBeSetAndGet()
        {
            // Arrange
            var tb = new TextBox();
            // Act
            tb.Text = "test value";
            // Assert
            Assert.Equal("test value", tb.Text);
        }

        [Fact]
        public void TextBox_ReadOnly_DefaultIsFalse()
        {
            // Arrange
            var tb = new TextBox();
            // Assert
            Assert.False(tb.ReadOnly);
        }

        // ─── WebControls: Button ──────────────────────────────────────────────────

        [Fact]
        public void Button_TextProperty_CanBeSetAndGet()
        {
            // Arrange
            var btn = new Button();
            // Act
            btn.Text = "Submit";
            // Assert
            Assert.Equal("Submit", btn.Text);
        }

        [Fact]
        public void Button_UseSubmitBehavior_DefaultIsTrue()
        {
            // Arrange
            var btn = new Button();
            // Assert
            Assert.True(btn.UseSubmitBehavior);
        }

        // ─── WebControls: DropDownList ────────────────────────────────────────────

        [Fact]
        public void DropDownList_Items_IsNotNull()
        {
            // Arrange
            var ddl = new DropDownList();
            // Assert
            Assert.NotNull(ddl.Items);
        }

        [Fact]
        public void DropDownList_SelectedIndex_DefaultIsMinusOne()
        {
            // Arrange
            var ddl = new DropDownList();
            // Assert
            Assert.Equal(-1, ddl.SelectedIndex);
        }

        [Fact]
        public void DropDownList_AddItem_IncreasesCount()
        {
            // Arrange
            var ddl = new DropDownList();
            // Act
            ddl.Items.Add("Option1");
            // Assert
            Assert.Equal(1, ddl.Items.Count);
        }

        // ─── WebControls: ListItem ────────────────────────────────────────────────

        [Fact]
        public void ListItem_DefaultConstructor_EmptyTextAndValue()
        {
            // Arrange & Act
            var item = new ListItem();
            // Assert
            Assert.Equal(string.Empty, item.Text);
            Assert.Equal(string.Empty, item.Value);
        }

        [Fact]
        public void ListItem_TextConstructor_SetsTextAndValue()
        {
            // Arrange & Act
            var item = new ListItem("MyText");
            // Assert
            Assert.Equal("MyText", item.Text);
            Assert.Equal("MyText", item.Value);
        }

        [Fact]
        public void ListItem_TextValueConstructor_SetsBoth()
        {
            // Arrange & Act
            var item = new ListItem("Display", "val1");
            // Assert
            Assert.Equal("Display", item.Text);
            Assert.Equal("val1", item.Value);
        }

        [Fact]
        public void ListItem_FullConstructor_SetsAllProperties()
        {
            // Arrange & Act
            var item = new ListItem("Display", "val1", false);
            // Assert
            Assert.Equal("Display", item.Text);
            Assert.Equal("val1", item.Value);
            Assert.False(item.Enabled);
        }

        // ─── WebControls: ListItemCollection ─────────────────────────────────────

        [Fact]
        public void ListItemCollection_FindByText_ReturnsCorrectItem()
        {
            // Arrange
            var col = new ListItemCollection();
            col.Add(new ListItem("Alpha", "a"));
            col.Add(new ListItem("Beta", "b"));
            // Act
            var found = col.FindByText("Alpha");
            // Assert
            Assert.NotNull(found);
            Assert.Equal("a", found!.Value);
        }

        [Fact]
        public void ListItemCollection_FindByValue_ReturnsCorrectItem()
        {
            // Arrange
            var col = new ListItemCollection();
            col.Add(new ListItem("Alpha", "a"));
            col.Add(new ListItem("Beta", "b"));
            // Act
            var found = col.FindByValue("b");
            // Assert
            Assert.NotNull(found);
            Assert.Equal("Beta", found!.Text);
        }

        [Fact]
        public void ListItemCollection_FindByText_ReturnsNullWhenNotFound()
        {
            // Arrange
            var col = new ListItemCollection();
            col.Add(new ListItem("Alpha", "a"));
            // Act
            var found = col.FindByText("Gamma");
            // Assert
            Assert.Null(found);
        }

        // ─── WebControls: GridView ────────────────────────────────────────────────

        [Fact]
        public void GridView_DefaultPageSize_IsTen()
        {
            // Arrange
            var gv = new GridView();
            // Assert
            Assert.Equal(10, gv.PageSize);
        }

        [Fact]
        public void GridView_AutoGenerateColumns_DefaultIsTrue()
        {
            // Arrange
            var gv = new GridView();
            // Assert
            Assert.True(gv.AutoGenerateColumns);
        }

        [Fact]
        public void GridView_DataBind_DoesNotThrow()
        {
            // Arrange
            var gv = new GridView();
            // Act & Assert
            var ex = Record.Exception(() => gv.DataBind());
            Assert.Null(ex);
        }

        // ─── WebControls: CheckBox ────────────────────────────────────────────────

        [Fact]
        public void CheckBox_Checked_DefaultIsFalse()
        {
            // Arrange
            var cb = new CheckBox();
            // Assert
            Assert.False(cb.Checked);
        }

        [Fact]
        public void CheckBox_Checked_CanBeSetToTrue()
        {
            // Arrange
            var cb = new CheckBox();
            // Act
            cb.Checked = true;
            // Assert
            Assert.True(cb.Checked);
        }

        // ─── WebControls: FileUpload ──────────────────────────────────────────────

        [Fact]
        public void FileUpload_HasFile_DefaultIsFalse()
        {
            // Arrange
            var fu = new FileUpload();
            // Assert
            Assert.False(fu.HasFile);
        }

        [Fact]
        public void FileUpload_FileName_DefaultIsEmpty()
        {
            // Arrange
            var fu = new FileUpload();
            // Assert
            Assert.Equal(string.Empty, fu.FileName);
        }

        [Fact]
        public void FileUpload_SaveAs_DoesNotThrow()
        {
            // Arrange
            var fu = new FileUpload();
            // Act & Assert
            var ex = Record.Exception(() => fu.SaveAs("/tmp/test.jpg"));
            Assert.Null(ex);
        }

        // ─── WebControls: Unit ────────────────────────────────────────────────────

        [Fact]
        public void Unit_Pixel_CreatesPixelUnit()
        {
            // Arrange & Act
            var u = Unit.Pixel(100);
            // Assert
            Assert.Equal(100, u.Value);
            Assert.Equal(UnitType.Pixel, u.Type);
        }

        [Fact]
        public void Unit_Percentage_CreatesPercentageUnit()
        {
            // Arrange & Act
            var u = Unit.Percentage(50);
            // Assert
            Assert.Equal(50, u.Value);
            Assert.Equal(UnitType.Percentage, u.Type);
        }

        [Fact]
        public void Unit_ToString_ReturnsPxSuffix()
        {
            // Arrange
            var u = new Unit(200);
            // Act
            var str = u.ToString();
            // Assert
            Assert.Contains("200", str);
        }

        [Fact]
        public void Unit_Parse_DoesNotThrow()
        {
            // Arrange & Act
            var ex = Record.Exception(() => Unit.Parse("100px"));
            // Assert
            Assert.Null(ex);
        }

        // ─── HtmlControls: HtmlGenericControl ────────────────────────────────────

        [Fact]
        public void HtmlGenericControl_DefaultConstructor_DoesNotThrow()
        {
            // Arrange & Act
            var ex = Record.Exception(() => new HtmlGenericControl());
            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void HtmlGenericControl_TagConstructor_DoesNotThrow()
        {
            // Arrange & Act
            var ex = Record.Exception(() => new HtmlGenericControl("div"));
            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void HtmlGenericControl_InnerHtml_CanBeSetAndGet()
        {
            // Arrange
            var ctrl = new HtmlGenericControl("div");
            // Act
            ctrl.InnerHtml = "<b>bold</b>";
            // Assert
            Assert.Equal("<b>bold</b>", ctrl.InnerHtml);
        }

        // ─── WebControls: Panel ───────────────────────────────────────────────────

        [Fact]
        public void Panel_DefaultScrollBars_IsNone()
        {
            // Arrange
            var panel = new Panel();
            // Assert
            Assert.Equal(ScrollBars.None, panel.ScrollBars);
        }

        [Fact]
        public void Panel_GroupingText_CanBeSetAndGet()
        {
            // Arrange
            var panel = new Panel();
            // Act
            panel.GroupingText = "My Group";
            // Assert
            Assert.Equal("My Group", panel.GroupingText);
        }

        // ─── WebControls: Literal ─────────────────────────────────────────────────

        [Fact]
        public void Literal_Text_CanBeSetAndGet()
        {
            // Arrange
            var lit = new Literal();
            // Act
            lit.Text = "Hello World";
            // Assert
            Assert.Equal("Hello World", lit.Text);
        }

        // ─── WebControls: HyperLink ───────────────────────────────────────────────

        [Fact]
        public void HyperLink_NavigateUrl_CanBeSetAndGet()
        {
            // Arrange
            var hl = new HyperLink();
            // Act
            hl.NavigateUrl = "http://example.com";
            // Assert
            Assert.Equal("http://example.com", hl.NavigateUrl);
        }

        // ─── WebControls: Image ───────────────────────────────────────────────────

        [Fact]
        public void Image_ImageUrl_CanBeSetAndGet()
        {
            // Arrange
            var img = new Image();
            // Act
            img.ImageUrl = "~/images/test.png";
            // Assert
            Assert.Equal("~/images/test.png", img.ImageUrl);
        }

        // ─── WebControls: RadioButton ─────────────────────────────────────────────

        [Fact]
        public void RadioButton_GroupName_CanBeSetAndGet()
        {
            // Arrange
            var rb = new RadioButton();
            // Act
            rb.GroupName = "group1";
            // Assert
            Assert.Equal("group1", rb.GroupName);
        }

        // ─── WebControls: Calendar ────────────────────────────────────────────────

        [Fact]
        public void Calendar_TodaysDate_DefaultIsToday()
        {
            // Arrange
            var cal = new Calendar();
            // Assert
            Assert.Equal(DateTime.Today, cal.TodaysDate);
        }

        // ─── WebControls: Repeater ────────────────────────────────────────────────

        [Fact]
        public void Repeater_DataBind_DoesNotThrow()
        {
            // Arrange
            var rep = new Repeater();
            // Act & Assert
            var ex = Record.Exception(() => rep.DataBind());
            Assert.Null(ex);
        }

        // ─── WebControls: DataList ────────────────────────────────────────────────

        [Fact]
        public void DataList_DataBind_DoesNotThrow()
        {
            // Arrange
            var dl = new DataList();
            // Act & Assert
            var ex = Record.Exception(() => dl.DataBind());
            Assert.Null(ex);
        }

        // ─── WebControls: MultiView ───────────────────────────────────────────────

        [Fact]
        public void MultiView_ActiveViewIndex_DefaultIsMinusOne()
        {
            // Arrange
            var mv = new MultiView();
            // Assert
            Assert.Equal(-1, mv.ActiveViewIndex);
        }

        // ─── WebControls: BaseValidator ───────────────────────────────────────────

        [Fact]
        public void RequiredFieldValidator_IsValid_DefaultIsTrue()
        {
            // Arrange
            var rfv = new RequiredFieldValidator();
            // Assert
            Assert.True(rfv.IsValid);
        }

        [Fact]
        public void RequiredFieldValidator_ErrorMessage_CanBeSetAndGet()
        {
            // Arrange
            var rfv = new RequiredFieldValidator();
            // Act
            rfv.ErrorMessage = "Field is required";
            // Assert
            Assert.Equal("Field is required", rfv.ErrorMessage);
        }

        [Fact]
        public void RegularExpressionValidator_ValidationExpression_CanBeSetAndGet()
        {
            // Arrange
            var rev = new RegularExpressionValidator();
            // Act
            rev.ValidationExpression = @"\d+";
            // Assert
            Assert.Equal(@"\d+", rev.ValidationExpression);
        }

        [Fact]
        public void RangeValidator_MinMaxValues_CanBeSetAndGet()
        {
            // Arrange
            var rv = new RangeValidator();
            // Act
            rv.MinimumValue = "0";
            rv.MaximumValue = "100";
            // Assert
            Assert.Equal("0", rv.MinimumValue);
            Assert.Equal("100", rv.MaximumValue);
        }

        // ─── WebControls: LinkButton ──────────────────────────────────────────────

        [Fact]
        public void LinkButton_Text_CanBeSetAndGet()
        {
            // Arrange
            var lb = new LinkButton();
            // Act
            lb.Text = "Click Me";
            // Assert
            Assert.Equal("Click Me", lb.Text);
        }

        // ─── WebControls: CheckBoxList ────────────────────────────────────────────

        [Fact]
        public void CheckBoxList_Items_IsNotNull()
        {
            // Arrange
            var cbl = new CheckBoxList();
            // Assert
            Assert.NotNull(cbl.Items);
        }

        // ─── WebControls: RadioButtonList ─────────────────────────────────────────

        [Fact]
        public void RadioButtonList_Items_IsNotNull()
        {
            // Arrange
            var rbl = new RadioButtonList();
            // Assert
            Assert.NotNull(rbl.Items);
        }

        // ─── WebControls: ValidationSummary ──────────────────────────────────────

        [Fact]
        public void ValidationSummary_ShowSummary_DefaultIsTrue()
        {
            // Arrange
            var vs = new ValidationSummary();
            // Assert
            Assert.True(vs.ShowSummary);
        }

        // ─── WebControls: BulletedList ────────────────────────────────────────────

        [Fact]
        public void BulletedList_Items_IsNotNull()
        {
            // Arrange
            var bl = new BulletedList();
            // Assert
            Assert.NotNull(bl.Items);
        }

        // ─── WebControls: ListBox ─────────────────────────────────────────────────

        [Fact]
        public void ListBox_SelectionMode_DefaultIsSingle()
        {
            // Arrange
            var lb = new ListBox();
            // Assert
            Assert.Equal(ListSelectionMode.Single, lb.SelectionMode);
        }

        // ─── WebControls: ImageButton ─────────────────────────────────────────────

        [Fact]
        public void ImageButton_ImageUrl_CanBeSetAndGet()
        {
            // Arrange
            var ib = new ImageButton();
            // Act
            ib.ImageUrl = "~/images/btn.png";
            // Assert
            Assert.Equal("~/images/btn.png", ib.ImageUrl);
        }

        // ─── HtmlControls: HtmlAnchor ─────────────────────────────────────────────

        [Fact]
        public void HtmlAnchor_HRef_CanBeSetAndGet()
        {
            // Arrange
            var anchor = new HtmlAnchor();
            // Act
            anchor.HRef = "http://example.com";
            // Assert
            Assert.Equal("http://example.com", anchor.HRef);
        }

        // ─── HtmlControls: HtmlImage ──────────────────────────────────────────────

        [Fact]
        public void HtmlImage_Src_CanBeSetAndGet()
        {
            // Arrange
            var img = new HtmlImage();
            // Act
            img.Src = "~/images/photo.jpg";
            // Assert
            Assert.Equal("~/images/photo.jpg", img.Src);
        }

        // ─── HtmlControls: HtmlInputText ──────────────────────────────────────────

        [Fact]
        public void HtmlInputText_Value_CanBeSetAndGet()
        {
            // Arrange
            var input = new HtmlInputText();
            // Act
            input.Value = "test";
            // Assert
            Assert.Equal("test", input.Value);
        }

        // ─── HtmlControls: HtmlInputPassword ─────────────────────────────────────

        [Fact]
        public void HtmlInputPassword_Value_CanBeSetAndGet()
        {
            // Arrange
            var input = new HtmlInputPassword();
            // Act
            input.Value = "secret";
            // Assert
            Assert.Equal("secret", input.Value);
        }

        // ─── WebControls: WebControl base ────────────────────────────────────────

        [Fact]
        public void WebControl_Enabled_DefaultIsTrue()
        {
            // Arrange
            var btn = new Button();
            // Assert
            Assert.True(btn.Enabled);
        }

        [Fact]
        public void WebControl_Visible_DefaultIsTrue()
        {
            // Arrange
            var btn = new Button();
            // Assert
            Assert.True(btn.Visible);
        }

        [Fact]
        public void WebControl_CssClass_CanBeSetAndGet()
        {
            // Arrange
            var btn = new Button();
            // Act
            btn.CssClass = "btn-primary";
            // Assert
            Assert.Equal("btn-primary", btn.CssClass);
        }

        [Fact]
        public void WebControl_Attributes_IsNotNull()
        {
            // Arrange
            var btn = new Button();
            // Assert
            Assert.NotNull(btn.Attributes);
        }

        // ─── HttpPostedFile ───────────────────────────────────────────────────────

        [Fact]
        public void HttpPostedFile_FileName_DefaultIsEmpty()
        {
            // Arrange
            var file = new HttpPostedFile();
            // Assert
            Assert.Equal(string.Empty, file.FileName);
        }

        [Fact]
        public void HttpPostedFile_SaveAs_DoesNotThrow()
        {
            // Arrange
            var file = new HttpPostedFile();
            // Act & Assert
            var ex = Record.Exception(() => file.SaveAs("/tmp/upload.jpg"));
            Assert.Null(ex);
        }

        // ─── Enum coverage ────────────────────────────────────────────────────────

        [Fact]
        public void TextBoxMode_Enum_HasExpectedValues()
        {
            Assert.Equal(0, (int)TextBoxMode.SingleLine);
            Assert.Equal(1, (int)TextBoxMode.MultiLine);
            Assert.Equal(2, (int)TextBoxMode.Password);
        }

        [Fact]
        public void DataControlRowType_Enum_HasExpectedValues()
        {
            Assert.Equal(0, (int)DataControlRowType.Header);
            Assert.Equal(2, (int)DataControlRowType.DataRow);
        }

        [Fact]
        public void ValidationDataType_Enum_HasExpectedValues()
        {
            Assert.Equal(0, (int)ValidationDataType.String);
            Assert.Equal(1, (int)ValidationDataType.Integer);
        }

        [Fact]
        public void ScrollBars_Enum_HasExpectedValues()
        {
            Assert.Equal(0, (int)ScrollBars.None);
            Assert.Equal(4, (int)ScrollBars.Auto);
        }
    }
}
