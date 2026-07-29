# ASP.NET Web Forms to .NET 8 Migration Analysis Report
## Tour Management Application

**Analysis Date:** 2025-01-30  
**Module:** Tour_Management  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Migration Complexity:** Complex  

---

## Executive Summary

The Tour Management application is a classic ASP.NET Web Forms application targeting .NET Framework 4.7.2. The application manages tour packages, user registrations, bookings, and admin operations. The codebase contains **11 Web Forms pages** with code-behind files, uses **raw ADO.NET** for all data access, has **no master pages or user controls**, and relies heavily on **System.Web** APIs throughout.

A total of **38 migration issues** were identified:
- **Critical:** 10
- **High:** 14
- **Medium:** 9
- **Low:** 5

**Estimated Remediation Effort:** 60–90 hours  
**Compatibility Score:** 18/100

---

## Project Inventory

| Component Type | Count | Files |
|---|---|---|
| Web Forms Pages (.aspx) | 11 | userlogin, SignUpForm, AdminLogin2, AdminProfile, MainProfilePage, AddTour, DisplayTours, TourCrud, Order, allbooking, mybooking, usercrud |
| Code-Behind Files (.aspx.cs) | 11 | All pages have code-behind |
| Designer Files (.aspx.designer.cs) | 11 | Auto-generated |
| Master Pages (.master) | 0 | None |
| User Controls (.ascx) | 0 | None |
| Global.asax | 0 | Not present |
| Web.config | 1 | Root configuration |
| packages.config | 1 | NuGet packages |
| .csproj | 1 | Legacy non-SDK style |

---

## Detailed Issue Findings

### CRITICAL Issues

#### Issue 1: System.Web Namespace Dependency (All Code-Behind Files)
- **Severity:** Critical
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files Affected:** All 11 .aspx.cs files
- **Code Snippet:**
```csharp
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
```
- **Description:** All code-behind files import `System.Web`, `System.Web.UI`, and `System.Web.UI.WebControls`. These namespaces are part of the .NET Framework only and do not exist in .NET 8. The entire Web Forms page lifecycle (`System.Web.UI.Page`) is unavailable in .NET 8.
- **Recommendation:** Migrate all pages to ASP.NET Core Razor Pages. Replace `System.Web.UI.Page` base class with Razor Page models (`PageModel`). Replace all server controls with HTML Tag Helpers or Razor syntax.
- **Effort:** High

#### Issue 2: Legacy Non-SDK Project File Format
- **Severity:** Critical
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `Tour_Management.csproj`
- **Line:** 1
- **Code Snippet:**
```xml
<Project ToolsVersion="15.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>
  <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
```
- **Description:** The project file uses the legacy non-SDK MSBuild format with Web Application project type GUIDs. This format is incompatible with .NET 8. The `TargetFrameworkVersion` must be replaced with `TargetFramework` in SDK-style format.
- **Recommendation:** Replace with SDK-style project file: `<Project Sdk="Microsoft.NET.Sdk.Web">` with `<TargetFramework>net8.0</TargetFramework>`.
- **Effort:** Medium

#### Issue 3: SQL Injection Vulnerability in userlogin.aspx.cs
- **Severity:** Critical
- **Category:** security
- **Breaking Change:** No
- **File:** `userlogin.aspx.cs`
- **Line:** 24
- **Code Snippet:**
```csharp
string checkPasswordQuery = "select password from Userinfo where password='" + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
```
- **Description:** Raw string concatenation is used to build SQL queries, creating a critical SQL injection vulnerability. This must be fixed during migration.
- **Recommendation:** Use parameterized queries (EF Core handles this automatically) or at minimum use `SqlParameter` objects. During migration to EF Core, use LINQ queries which are inherently parameterized.
- **Effort:** Low

#### Issue 4: Web.config Configuration System
- **Severity:** Critical
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `Web.config`
- **Line:** 1
- **Code Snippet:**
```xml
<configuration>
  <system.web>
    <compilation debug="true" targetFramework="4.7.2">
    <httpRuntime targetFramework="4.7.2"/>
  </system.web>
  <connectionStrings>
    <add name="dbconnection" connectionString="..."/>
  </connectionStrings>
```
- **Description:** The entire `Web.config` configuration system is .NET Framework-specific. `system.web`, `system.webServer`, `httpHandlers`, `httpRuntime`, and `compilation` sections do not exist in .NET 8.
- **Recommendation:** Migrate to `appsettings.json`. Move connection strings to `appsettings.json` under `ConnectionStrings` section. Configure middleware in `Program.cs`.
- **Effort:** Medium

#### Issue 5: System.Web.DataVisualization Chart Control
- **Severity:** Critical
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **File:** `allbooking.aspx`, `Web.config`, `Tour_Management.csproj`
- **Line:** 1 (allbooking.aspx), 7 (Web.config)
- **Code Snippet:**
```aspx
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
```
```xml
<Reference Include="System.Web.DataVisualization" />
<add assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"/>
```
- **Description:** `System.Web.DataVisualization` is a .NET Framework-only assembly. It is registered in Web.config, referenced in the .csproj, and used in allbooking.aspx. There is no direct equivalent in .NET 8.
- **Recommendation:** Replace with a modern charting library such as `Chart.js` (client-side JavaScript) or `LiveCharts2` for server-side rendering. The chart control is registered but not visibly used in the current allbooking.aspx markup.
- **Effort:** High

#### Issue 6: ConfigurationManager.ConnectionStrings Usage
- **Severity:** Critical
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **Files:** `userlogin.aspx.cs` (line 23), `SignUpForm.aspx.cs` (line 14), `AddTour.aspx.cs` (line 20), `TourCrud.aspx.cs` (line 18), `Order.aspx.cs` (line 16)
- **Code Snippet:**
```csharp
using System.Configuration;
SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
```
- **Description:** `System.Configuration.ConfigurationManager` reads from `Web.config` which does not exist in .NET 8. While `ConfigurationManager` is available via NuGet in .NET 8, the recommended approach is to use `IConfiguration` with `appsettings.json`.
- **Recommendation:** Inject `IConfiguration` via dependency injection and use `configuration.GetConnectionString("dbconnection")`. Better yet, migrate to EF Core with `DbContext` and configure the connection string in `Program.cs`.
- **Effort:** Medium

#### Issue 7: Raw ADO.NET Data Access Pattern
- **Severity:** Critical
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files:** `userlogin.aspx.cs`, `SignUpForm.aspx.cs`, `AddTour.aspx.cs`, `TourCrud.aspx.cs`, `Order.aspx.cs`
- **Code Snippet:**
```csharp
SqlConnection conn = new SqlConnection(...);
conn.Open();
SqlCommand com = new SqlCommand(insertQuery, conn);
com.Parameters.AddWithValue("@Email", email.Text);
com.ExecuteNonQuery();
conn.Close();
```
- **Description:** All data access uses raw ADO.NET with `SqlConnection`, `SqlCommand`, and `SqlDataAdapter`. While ADO.NET works in .NET 8, the pattern is tightly coupled to the UI layer (code-behind), violating separation of concerns. Connection management is also poor (no `using` statements, no async operations).
- **Recommendation:** Migrate to Entity Framework Core 8.0.0 with a proper repository pattern. Create a `TourManagementDbContext`, define entities (`Tour`, `UserInfo`, `Booking`), and implement repository interfaces. Use async/await for all database operations.
- **Effort:** High

#### Issue 8: Server.MapPath Usage
- **Severity:** Critical
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **File:** `AddTour.aspx.cs`
- **Line:** 30
- **Code Snippet:**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
```
- **Description:** `Server.MapPath()` is a `System.Web.HttpServerUtility` method that does not exist in .NET 8. It maps virtual paths to physical file system paths.
- **Recommendation:** Replace with `IWebHostEnvironment.WebRootPath` or `IWebHostEnvironment.ContentRootPath`. In Razor Pages: `Path.Combine(_webHostEnvironment.WebRootPath, "Tour_pics", fileName)`.
- **Effort:** Low

#### Issue 9: SqlDataSource Server Control
- **Severity:** Critical
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files:** `DisplayTours.aspx` (line 10), `TourCrud.aspx` (line 35), `allbooking.aspx` (line 20), `mybooking.aspx` (line 14), `usercrud.aspx` (line 18)
- **Code Snippet:**
```aspx
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:dbconnection %>" 
    SelectCommand="SELECT * FROM [Tour]"
    UpdateCommand="UPDATE [Tour] Set [TOUR_NAME]=@TOUR_NAME..."
    DeleteCommand="Delete from [Tour] Where [TOUR_ID]=@TOUR_ID">
</asp:SqlDataSource>
```
- **Description:** `asp:SqlDataSource` is a Web Forms data source control that does not exist in .NET 8. It provides declarative data binding directly in markup, which is a Web Forms-only pattern.
- **Recommendation:** Replace with EF Core repository calls in Razor Page handlers (`OnGetAsync`, `OnPostAsync`). Bind data to Razor Page model properties and render using `@foreach` loops in Razor markup.
- **Effort:** High

#### Issue 10: GridView Server Control with AutoGenerateEditButton/DeleteButton
- **Severity:** Critical
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files:** `DisplayTours.aspx`, `TourCrud.aspx`, `allbooking.aspx`, `mybooking.aspx`, `usercrud.aspx`
- **Code Snippet:**
```aspx
<asp:GridView ID="GridView1" runat="server" 
    AutoGenerateColumns="False" 
    AutoGenerateDeleteButton="True" 
    AutoGenerateEditButton="True" 
    DataKeyNames="TOUR_ID" 
    DataSourceID="SqlDataSource1">
```
- **Description:** `asp:GridView` is a Web Forms server control that does not exist in .NET 8. The auto-generated edit/delete buttons rely on the Web Forms postback mechanism and ViewState.
- **Recommendation:** Replace with HTML `<table>` elements rendered via Razor `@foreach` loops. Implement edit/delete as separate Razor Pages or use AJAX calls. Consider using a modern data table library like DataTables.js.
- **Effort:** High

---

### HIGH Issues

#### Issue 11: Page Lifecycle Events (Page_Load, IsPostBack)
- **Severity:** High
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files:** All .aspx.cs files
- **Code Snippet:**
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!Page.IsPostBack)
    {
        refreshdata();
    }
}
```
- **Description:** The Web Forms page lifecycle (`Page_Load`, `Page_PreRender`, `IsPostBack`) does not exist in .NET 8. Razor Pages use `OnGet`/`OnPost` handler methods instead.
- **Recommendation:** Replace `Page_Load` with `OnGet()` or `OnGetAsync()`. Replace postback checks with separate `OnPost()` handlers. The `IsPostBack` pattern is replaced by the HTTP verb-based routing in Razor Pages.
- **Effort:** Medium

#### Issue 12: Button Click Event Handlers (Postback Pattern)
- **Severity:** High
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files:** `userlogin.aspx.cs` (Btn_Submit, Btn_reg), `SignUpForm.aspx.cs` (Register_Click), `AddTour.aspx.cs` (Register_Click), `Order.aspx.cs` (btn_click)
- **Code Snippet:**
```csharp
protected void Btn_Submit(object sender, EventArgs e) { ... }
protected void Register_Click(object sender, EventArgs e) { ... }
```
```aspx
<asp:Button OnClick="Btn_Submit" runat="server" Text="Login" />
```
- **Description:** Web Forms server-side event handlers wired via `OnClick` attributes use the postback mechanism which does not exist in .NET 8. The entire event-driven programming model is Web Forms-specific.
- **Recommendation:** Replace with Razor Page `OnPost()` handlers. Use `<form method="post">` with `asp-page-handler` tag helpers for named handlers. Replace `asp:Button` with `<button type="submit">`.
- **Effort:** Medium

#### Issue 13: Response.Redirect and Server.Transfer
- **Severity:** High
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **Files:** `userlogin.aspx.cs` (lines 33, 35), `SignUpForm.aspx.cs` (lines 30, 31), `Order.aspx.cs` (lines 22, 23)
- **Code Snippet:**
```csharp
Response.Redirect("MainProfilePage.aspx");
Server.Transfer("MainProfilePage.aspx");
```
- **Description:** `Response.Redirect` is available in ASP.NET Core but `Server.Transfer` is not. Additionally, using both `Response.Redirect` and `Server.Transfer` consecutively is a logic error (unreachable code). The `.aspx` extension-based routing does not apply in .NET 8.
- **Recommendation:** Use `return RedirectToPage("/MainProfilePage")` in Razor Pages. Remove `Server.Transfer` calls entirely. Fix the unreachable code issue.
- **Effort:** Low

#### Issue 14: Response.Write for User Feedback
- **Severity:** High
- **Category:** deprecated-api
- **Breaking Change:** No (available but not recommended)
- **Files:** `userlogin.aspx.cs` (lines 31, 38), `SignUpForm.aspx.cs` (line 29), `AddTour.aspx.cs` (line 35), `Order.aspx.cs` (line 21)
- **Code Snippet:**
```csharp
Response.Write("Password is correct");
Response.Write("Registration Successful");
```
- **Description:** `Response.Write` is used to display user feedback messages directly in the HTTP response. This is a poor UX pattern and not appropriate for .NET 8 Razor Pages.
- **Recommendation:** Use `TempData` for success/error messages that persist across redirects. Display messages in the Razor Page view using `@TempData["Message"]` or use model validation with `ModelState`.
- **Effort:** Low

#### Issue 15: FileUpload Server Control
- **Severity:** High
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `AddTour.aspx` (line 38), `AddTour.aspx.cs` (line 30)
- **Code Snippet:**
```aspx
<asp:FileUpload ID="FileUpload1" runat="server"/>
```
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
com.Parameters.AddWithValue("@pic", FileUpload1.FileName);
```
- **Description:** `asp:FileUpload` is a Web Forms server control. In .NET 8, file uploads are handled via `IFormFile` in Razor Pages.
- **Recommendation:** Replace with `<input type="file" asp-for="TourImage" />`. In the page model, use `IFormFile TourImage` property. Save using `IWebHostEnvironment.WebRootPath`.
- **Effort:** Medium

#### Issue 16: asp:Label, asp:TextBox, asp:Button, asp:DropDownList Server Controls
- **Severity:** High
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **Files:** All .aspx files
- **Code Snippet:**
```aspx
<asp:Label ID="Label1" runat="server" Text="Email" />
<asp:TextBox ID="txtEmail" TextMode="Email" runat="server" ForeColor="Black" class="form-control" />
<asp:Button ID="Register" runat="server" Text="Login" OnClick="Btn_Submit" />
<asp:DropDownList ID="gender" runat="server" Width="361px">
```
- **Description:** All ASP.NET server controls (`asp:Label`, `asp:TextBox`, `asp:Button`, `asp:DropDownList`, `asp:HyperLink`, `asp:RegularExpressionValidator`) are Web Forms-specific and do not exist in .NET 8.
- **Recommendation:** Replace with standard HTML elements using Razor Tag Helpers: `<label asp-for="Email">`, `<input asp-for="Email" class="form-control" />`, `<button type="submit">`, `<select asp-for="Gender" asp-items="...">`. Use Data Annotations for validation.
- **Effort:** High

#### Issue 17: Hardcoded Admin Credentials
- **Severity:** High
- **Category:** security
- **Breaking Change:** No
- **File:** `AdminLogin2.aspx.cs`
- **Line:** 14
- **Code Snippet:**
```csharp
if (password.Text == "admin" && name.Text == "admin@gmail.com")
{
    Response.Redirect("AdminProfile.aspx");
}
```
- **Description:** Admin credentials are hardcoded in the source code. This is a critical security vulnerability. Additionally, the login check is in `Page_Load` which runs on every page load, not just on form submission.
- **Recommendation:** Implement ASP.NET Core Identity for authentication. Store credentials securely (hashed passwords). Use role-based authorization with `[Authorize(Roles = "Admin")]` attribute.
- **Effort:** High

#### Issue 18: Plaintext Password Storage
- **Severity:** High
- **Category:** security
- **Breaking Change:** No
- **Files:** `SignUpForm.aspx.cs` (line 22), `usercrud.aspx` (Password column displayed)
- **Code Snippet:**
```csharp
com.Parameters.AddWithValue("@Password", password1.Text);
```
```aspx
<asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
```
- **Description:** User passwords are stored in plaintext in the database and even displayed in the user management grid. This is a severe security vulnerability.
- **Recommendation:** Use ASP.NET Core Identity which handles password hashing automatically (PBKDF2). Never store or display plaintext passwords. Remove the Password column from the user management grid.
- **Effort:** High

#### Issue 19: No Session Management / Authentication State
- **Severity:** High
- **Category:** security
- **Breaking Change:** No
- **Files:** `userlogin.aspx.cs` (line 30 - commented out session code)
- **Code Snippet:**
```csharp
//Session["New"] = txtEmail.Text;
Response.Redirect("MainProfilePage.aspx");
```
- **Description:** Session management is commented out. There is no authentication state management - any user can navigate directly to protected pages without logging in. No authorization checks exist on any page.
- **Recommendation:** Implement ASP.NET Core Identity with cookie authentication. Add `[Authorize]` attributes to protected Razor Pages. Configure authentication middleware in `Program.cs`.
- **Effort:** High

#### Issue 20: asp:SqlDataSource with Inline SQL (Security)
- **Severity:** High
- **Category:** security
- **Breaking Change:** No
- **Files:** `usercrud.aspx` (line 18)
- **Code Snippet:**
```aspx
SelectCommand="Select top (select COUNT(*) from UserInfo) * From UserInfo
EXCEPT
Select top ((select COUNT(*) from UserInfo)-(1)) * From UserInfo"
```
- **Description:** Complex inline SQL in markup is difficult to maintain and potentially vulnerable. The query logic (getting the last row) is convoluted and should be replaced with proper parameterized queries.
- **Recommendation:** Replace with EF Core LINQ queries in the page model. Use `dbContext.UserInfo.OrderByDescending(u => u.Email).FirstOrDefault()` or appropriate query.
- **Effort:** Medium

#### Issue 21: asp:RegularExpressionValidator Server Control
- **Severity:** High
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `AddTour.aspx`
- **Line:** 52
- **Code Snippet:**
```aspx
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
    ControlToValidate="tour_info" 
    ValidationExpression="^[\s\S]{0,250}$" 
    runat="server" 
    ErrorMessage="Characters less than 250">
</asp:RegularExpressionValidator>
```
- **Description:** Web Forms validation controls (`asp:RegularExpressionValidator`, `asp:RequiredFieldValidator`, etc.) do not exist in .NET 8.
- **Recommendation:** Use Data Annotations on the page model: `[MaxLength(250, ErrorMessage = "Characters less than 250")]`. Use `<span asp-validation-for="TourInfo" class="text-danger"></span>` in the view. Add `@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers` to `_ViewImports.cshtml`.
- **Effort:** Low

#### Issue 22: packages.config NuGet Format
- **Severity:** High
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `packages.config`
- **Code Snippet:**
```xml
<packages>
  <package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />
</packages>
```
- **Description:** The `packages.config` format is the legacy NuGet package management format. .NET 8 SDK-style projects use `<PackageReference>` elements directly in the `.csproj` file.
- **Recommendation:** Remove `packages.config`. Add `<PackageReference>` elements to the new SDK-style `.csproj` file. The `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` package is not needed in .NET 8.
- **Effort:** Low

#### Issue 23: System.Web.DataVisualization HTTP Handler in Web.config
- **Severity:** High
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **File:** `Web.config`
- **Lines:** 5-14
- **Code Snippet:**
```xml
<system.webServer>
  <handlers>
    <add name="ChartImageHandler" verb="GET,HEAD,POST" path="ChartImg.axd" 
         type="System.Web.UI.DataVisualization.Charting.ChartHttpHandler, System.Web.DataVisualization..."/>
  </handlers>
</system.webServer>
<httpHandlers>
  <add path="ChartImg.axd" verb="GET,HEAD,POST" 
       type="System.Web.UI.DataVisualization.Charting.ChartHttpHandler..."/>
</httpHandlers>
```
- **Description:** HTTP handlers (`.axd` endpoints) are a Web Forms/IIS concept. In .NET 8, HTTP handlers are replaced by middleware and minimal API endpoints.
- **Recommendation:** Remove the chart HTTP handler configuration. If charting is needed, use a client-side library (Chart.js) or a .NET 8 compatible server-side library.
- **Effort:** Low

#### Issue 24: Microsoft.CodeDom.Providers.DotNetCompilerPlatform
- **Severity:** High
- **Category:** package-compatibility
- **Breaking Change:** Yes
- **Files:** `packages.config`, `Tour_Management.csproj`, `Web.config`
- **Code Snippet:**
```xml
<package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />
```
- **Description:** This package provides Roslyn compiler support for .NET Framework Web Forms projects. It is completely unnecessary in .NET 8 which uses Roslyn by default.
- **Recommendation:** Remove this package entirely. .NET 8 SDK includes Roslyn compiler support natively.
- **Effort:** Low

---

### MEDIUM Issues

#### Issue 25: No Dependency Injection
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** No
- **Files:** All code-behind files
- **Description:** The application has no dependency injection. Database connections are created directly in code-behind files. .NET 8 has a built-in DI container that should be used for all services.
- **Recommendation:** Register services in `Program.cs` using `builder.Services`. Inject `DbContext`, repositories, and services via constructor injection in Razor Page models.
- **Effort:** Medium

#### Issue 26: No Async/Await Pattern
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** No
- **Files:** All code-behind files with database operations
- **Description:** All database operations are synchronous. .NET 8 strongly encourages async/await for all I/O operations to improve scalability.
- **Recommendation:** Use `async Task OnGetAsync()`, `async Task<IActionResult> OnPostAsync()`. Use EF Core async methods: `await dbContext.Tours.ToListAsync()`, `await dbContext.SaveChangesAsync()`.
- **Effort:** Medium

#### Issue 27: No Error Handling
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** No
- **Files:** All code-behind files
- **Description:** No try-catch blocks exist in any code-behind file. Database connections are not properly disposed (no `using` statements). Exceptions will cause unhandled errors.
- **Recommendation:** Wrap all database operations in try-catch blocks. Use `using` statements or `await using` for disposable resources. Implement global exception handling middleware in `Program.cs`.
- **Effort:** Medium

#### Issue 28: Hardcoded Database File Path in Connection String
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `Web.config`
- **Line:** 28
- **Code Snippet:**
```xml
<add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf;Integrated Security=True"/>
```
- **Description:** The connection string contains a hardcoded absolute path to a developer's local machine (`C:\Users\gajer\...`). This will not work in any other environment.
- **Recommendation:** Use `|DataDirectory|` substitution or environment-specific configuration. In .NET 8, use `appsettings.Development.json` for development connection strings and environment variables for production.
- **Effort:** Low

#### Issue 29: App_Data Folder with .mdf Database File
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** Yes
- **File:** `App_Data/tourdb.mdf`
- **Description:** The application uses a LocalDB `.mdf` file in the `App_Data` folder. While LocalDB can be used with .NET 8, the `App_Data` folder convention is Web Forms-specific. The database schema needs to be documented for EF Core migration.
- **Recommendation:** Create EF Core migrations to define the database schema. Use a proper SQL Server instance or SQL Server Express for development. Document the existing schema (Tour, UserInfo, booking tables).
- **Effort:** Medium

#### Issue 30: CodeFile vs CodeBehind Attribute Inconsistency
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** No
- **File:** `AdminLogin2.aspx`
- **Line:** 1
- **Code Snippet:**
```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLogin2.aspx.cs" Inherits="Tour_Management.AdminLogin2" %>
```
- **Description:** `AdminLogin2.aspx` uses `CodeFile` attribute while all other pages use `CodeBehind`. `CodeFile` is used for Web Sites (not Web Applications). This inconsistency indicates a copy-paste issue. Both are irrelevant in .NET 8.
- **Recommendation:** This is a minor issue that will be resolved during migration to Razor Pages. No action needed before migration.
- **Effort:** Low

#### Issue 31: Unreachable Code After Response.Redirect
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** No
- **Files:** `userlogin.aspx.cs` (lines 33-35), `SignUpForm.aspx.cs` (lines 30-31), `Order.aspx.cs` (lines 22-23)
- **Code Snippet:**
```csharp
Response.Redirect("MainProfilePage.aspx");
Server.Transfer("MainProfilePage.aspx"); // Unreachable code
```
- **Description:** `Server.Transfer` is called after `Response.Redirect` which ends the response. The `Server.Transfer` calls are unreachable dead code. Similarly, `conn.Close()` after `Response.Redirect` in `Order.aspx.cs` is unreachable.
- **Recommendation:** Remove all `Server.Transfer` calls. Ensure database connections are closed/disposed before redirecting using `using` statements.
- **Effort:** Low

#### Issue 32: Missing DOCTYPE and HTML Structure in userlogin.aspx
- **Severity:** Medium
- **Category:** webforms-migration
- **Breaking Change:** No
- **File:** `userlogin.aspx`
- **Line:** 1
- **Description:** `userlogin.aspx` is missing the `<!DOCTYPE html>` declaration and has malformed HTML (orphaned `</div>` tag). While this works in Web Forms, it should be fixed during migration.
- **Recommendation:** Ensure all Razor Pages have proper HTML structure with `<!DOCTYPE html>`. Use a shared `_Layout.cshtml` for consistent page structure.
- **Effort:** Low

#### Issue 33: ValidationSettings:UnobtrusiveValidationMode in Web.config
- **Severity:** Medium
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **File:** `Web.config`
- **Line:** 31
- **Code Snippet:**
```xml
<add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />
```
- **Description:** This setting disables unobtrusive validation in Web Forms. It is irrelevant in .NET 8 which uses a completely different validation approach.
- **Recommendation:** Remove this setting. In .NET 8, use Data Annotations and jQuery Unobtrusive Validation via `jquery.validate.unobtrusive.js`.
- **Effort:** Low

---

### LOW Issues

#### Issue 34: No Master Page / Consistent Layout
- **Severity:** Low
- **Category:** webforms-migration
- **Breaking Change:** No
- **Files:** All .aspx files
- **Description:** The application has no master page. Each page has its own inline CSS and navigation. This leads to code duplication and inconsistent styling.
- **Recommendation:** Create a shared `_Layout.cshtml` in Razor Pages. Move navigation and common CSS to the layout. Use Bootstrap 5 for consistent styling.
- **Effort:** Medium

#### Issue 35: Inline CSS Styles
- **Severity:** Low
- **Category:** webforms-migration
- **Breaking Change:** No
- **Files:** All .aspx files
- **Description:** All CSS is defined inline within each page's `<style>` tags. There are no external CSS files.
- **Recommendation:** Create a `wwwroot/css/site.css` file. Move common styles to the shared stylesheet. Reference Bootstrap 5 via CDN or local files.
- **Effort:** Low

#### Issue 36: No Input Validation Beyond Required Fields
- **Severity:** Low
- **Category:** security
- **Breaking Change:** No
- **Files:** All form pages
- **Description:** The only validation is HTML5 `required` attributes and one `RegularExpressionValidator`. There is no server-side validation.
- **Recommendation:** Add Data Annotations to page model properties: `[Required]`, `[EmailAddress]`, `[MaxLength]`, `[Range]`. Use `FluentValidation` for complex validation rules. Always validate on the server side.
- **Effort:** Medium

#### Issue 37: Password Confirmation Not Validated
- **Severity:** Low
- **Category:** security
- **Breaking Change:** No
- **File:** `SignUpForm.aspx.cs`
- **Description:** The registration form has two password fields (`password1`, `password2`) but the code-behind only uses `password1`. The password confirmation is never validated.
- **Recommendation:** Add a `[Compare("Password")]` Data Annotation to the confirm password property in the page model.
- **Effort:** Low

#### Issue 38: Legacy System.Web.DynamicData and System.Web.Entity References
- **Severity:** Low
- **Category:** deprecated-api
- **Breaking Change:** Yes
- **File:** `Tour_Management.csproj`
- **Lines:** 44-45
- **Code Snippet:**
```xml
<Reference Include="System.Web.DynamicData" />
<Reference Include="System.Web.Entity" />
```
- **Description:** References to `System.Web.DynamicData` and `System.Web.Entity` are included in the project but not used. These are .NET Framework-only assemblies.
- **Recommendation:** Remove these unused references. They will be automatically excluded when migrating to the SDK-style project format.
- **Effort:** Low

---

## Migration Roadmap

### Phase 1: Foundation Setup (Week 1-2)
1. Create new .NET 8 solution with clean architecture (Domain, Application, Infrastructure, Web layers)
2. Set up SDK-style project files with proper `<PackageReference>` entries
3. Create `appsettings.json` with connection strings
4. Set up `Program.cs` with middleware pipeline
5. Create EF Core `DbContext` and entity models (Tour, UserInfo, Booking)
6. Generate EF Core migrations from existing database schema

### Phase 2: Domain & Infrastructure Layer (Week 2-3)
1. Create domain entities: `Tour`, `UserInfo`, `Booking`
2. Define repository interfaces: `ITourRepository`, `IUserRepository`, `IBookingRepository`
3. Implement EF Core repositories with async operations
4. Create DTOs for all entities
5. Implement service layer with business logic

### Phase 3: Authentication & Security (Week 3-4)
1. Implement ASP.NET Core Identity
2. Create user registration with password hashing
3. Implement login with cookie authentication
4. Add role-based authorization (Admin, User)
5. Protect all pages with `[Authorize]` attributes

### Phase 4: UI Migration (Week 4-6)
1. Create `_Layout.cshtml` with navigation
2. Migrate each page to Razor Pages:
   - `userlogin.aspx` → `Pages/Account/Login.cshtml`
   - `SignUpForm.aspx` → `Pages/Account/Register.cshtml`
   - `AdminLogin2.aspx` → Merged with Identity login
   - `AdminProfile.aspx` → `Pages/Admin/Index.cshtml`
   - `MainProfilePage.aspx` → `Pages/Index.cshtml`
   - `AddTour.aspx` → `Pages/Tours/Create.cshtml`
   - `DisplayTours.aspx` → `Pages/Tours/Index.cshtml`
   - `TourCrud.aspx` → `Pages/Admin/Tours/Index.cshtml`
   - `Order.aspx` → `Pages/Bookings/Create.cshtml`
   - `allbooking.aspx` → `Pages/Admin/Bookings/Index.cshtml`
   - `mybooking.aspx` → `Pages/Bookings/Index.cshtml`
   - `usercrud.aspx` → `Pages/Admin/Users/Index.cshtml`
3. Replace all server controls with HTML Tag Helpers
4. Implement file upload with `IFormFile`

### Phase 5: Testing & Documentation (Week 6-7)
1. Write unit tests for service layer
2. Write integration tests for repositories
3. Create `docs/MIGRATION_NOTES.md`
4. Create `docs/ARCHITECTURE.md`
5. Update `README.md`

---

## Migration Mapping Table

| Web Forms Component | .NET 8 Equivalent |
|---|---|
| `System.Web.UI.Page` | `Microsoft.AspNetCore.Mvc.RazorPages.PageModel` |
| `Page_Load` | `OnGet()` / `OnGetAsync()` |
| `Button.OnClick` | `OnPost()` / `OnPostAsync()` |
| `asp:TextBox` | `<input asp-for="Property" />` |
| `asp:Label` | `<label asp-for="Property" />` |
| `asp:Button` | `<button type="submit">` |
| `asp:DropDownList` | `<select asp-for="Property" asp-items="...">` |
| `asp:GridView` | `@foreach` loop with `<table>` |
| `asp:SqlDataSource` | EF Core repository + page model property |
| `asp:FileUpload` | `<input type="file" asp-for="File" />` + `IFormFile` |
| `asp:RegularExpressionValidator` | `[RegularExpression]` Data Annotation |
| `Response.Redirect("page.aspx")` | `return RedirectToPage("/PageName")` |
| `Server.Transfer("page.aspx")` | Remove (not supported) |
| `Server.MapPath("~/folder/")` | `_env.WebRootPath + "/folder/"` |
| `Response.Write("message")` | `TempData["Message"] = "message"` |
| `ConfigurationManager.ConnectionStrings` | `IConfiguration.GetConnectionString()` |
| `Session["key"]` | `HttpContext.Session["key"]` or `TempData` |
| `Web.config` | `appsettings.json` |
| `Global.asax` | `Program.cs` middleware |
| `packages.config` | `<PackageReference>` in .csproj |
| `SqlConnection` + `SqlCommand` | EF Core `DbContext` |
| `SqlDataAdapter` + `DataTable` | EF Core LINQ queries returning `List<T>` |

---

## Required NuGet Packages for .NET 8

```xml
<!-- Infrastructure Layer -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />

<!-- Application Layer -->
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.9.0" />

<!-- Web Layer -->
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />

<!-- Testing -->
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
<PackageReference Include="xunit" Version="2.6.6" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
```

---

## Code Migration Examples

### Example 1: Login Page Migration

**Before (userlogin.aspx.cs):**
```csharp
protected void Btn_Submit(object sender, EventArgs e)
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
    conn.Open();
    string checkPasswordQuery = "select password from Userinfo where password='" + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
    SqlCommand passComm = new SqlCommand(checkPasswordQuery, conn);
    string password = passComm.ExecuteScalar()?.ToString() ?? "";
    if (password == txtPassword.Text)
    {
        Response.Redirect("MainProfilePage.aspx");
    }
}
```

**After (Pages/Account/Login.cshtml.cs):**
```csharp
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    [BindProperty]
    public LoginViewModel Input { get; set; } = new();
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        
        var result = await _signInManager.PasswordSignInAsync(
            Input.Email, Input.Password, false, lockoutOnFailure: true);
        
        if (result.Succeeded)
            return RedirectToPage("/Index");
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
}
```

### Example 2: Tour Data Access Migration

**Before (AddTour.aspx.cs):**
```csharp
SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
conn.Open();
string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) values(@TOUR_NAME,@PLACE,@DAYS,@PRICE,@LOCATIONS,@TOUR_INFO,@pic)";
SqlCommand com = new SqlCommand(insertQuery, conn);
com.Parameters.AddWithValue("@TOUR_NAME", tour_name.Text);
com.ExecuteNonQuery();
```

**After (Application/Services/TourService.cs):**
```csharp
public async Task<TourDto> CreateAsync(TourCreateDto dto, CancellationToken ct = default)
{
    var tour = new Tour
    {
        TourName = dto.TourName,
        Place = dto.Place,
        Days = dto.Days,
        Price = dto.Price,
        Locations = dto.Locations,
        TourInfo = dto.TourInfo,
        PicturePath = dto.PicturePath,
        CreatedDate = DateTime.UtcNow,
        IsActive = true
    };
    await _repository.AddAsync(tour, ct);
    return _mapper.Map<TourDto>(tour);
}
```

### Example 3: Program.cs Configuration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();
builder.Services.AddDbContext<TourManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbconnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TourManagementDbContext>();
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ITourService, TourService>();

// Configure logging
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
```

---

## Known Issues and Risks

1. **Database Schema Unknown**: The `.mdf` file is binary. The exact schema (column types, constraints, relationships) must be reverse-engineered before creating EF Core migrations.
2. **No Authentication State**: The current application has no working authentication. Users can access any page directly. This must be addressed as a priority.
3. **Image Storage**: Tour images are stored in the `Tour_pics` folder. During migration, decide whether to keep file system storage or move to blob storage.
4. **Admin Login Logic Bug**: The admin login check is in `Page_Load` which runs on every request, not just on form submission. This means the admin is never actually authenticated.
5. **Data Loss Risk**: The `mybooking.aspx` page allows deleting bookings without confirmation. Add confirmation dialogs in the migrated version.
