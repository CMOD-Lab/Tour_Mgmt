# ASP.NET Web Forms to .NET 8 Migration Analysis Report
## Tour Management Application

**Analysis Date:** 2025-01-30  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Module:** Tour_Management  

---

## Executive Summary

The Tour Management application is a classic ASP.NET Web Forms application targeting .NET Framework 4.7.2. It consists of **11 Web Forms pages** with code-behind files, uses **raw ADO.NET** for all data access, and has **no master pages or user controls**. The application has **no authentication framework** (only a hardcoded admin credential check), stores **plain-text passwords**, and uses **System.Web** throughout.

| Severity | Count |
|----------|-------|
| Critical | 12    |
| High     | 10    |
| Medium   | 8     |
| Low      | 4     |
| **Total**| **34**|

**Migration Complexity:** Complex  
**Estimated Effort:** 80–120 hours  
**Compatibility Score:** 15/100  

---

## Web Forms Component Inventory

| File | Type | Complexity | Description |
|------|------|------------|-------------|
| AddTour.aspx / .cs | Web Forms Page | Medium | Add new tour with file upload |
| AdminLogin2.aspx / .cs | Web Forms Page | Medium | Admin login (hardcoded credentials) |
| AdminProfile.aspx / .cs | Web Forms Page | Simple | Admin dashboard/home |
| allbooking.aspx / .cs | Web Forms Page | Medium | View all bookings (GridView + SqlDataSource) |
| DisplayTours.aspx / .cs | Web Forms Page | Medium | Display tours (GridView + SqlDataSource) |
| MainProfilePage.aspx / .cs | Web Forms Page | Simple | User home page |
| mybooking.aspx / .cs | Web Forms Page | Medium | User's own bookings (GridView + SqlDataSource) |
| Order.aspx / .cs | Web Forms Page | Medium | Book a tour |
| SignUpForm.aspx / .cs | Web Forms Page | Medium | User registration |
| TourCrud.aspx / .cs | Web Forms Page | Complex | Tour CRUD (GridView + SqlDataSource) |
| userlogin.aspx / .cs | Web Forms Page | Medium | User login (SQL injection vulnerability) |
| usercrud.aspx / .cs | Web Forms Page | Medium | User management (GridView + SqlDataSource) |

**No master pages found.**  
**No user controls (.ascx) found.**  
**No Global.asax found.**  

---

## Detailed Issue Findings

### CRITICAL Issues

#### ISSUE-001: System.Web Dependency — All Code-Behind Files
- **Severity:** Critical  
- **Category:** webforms-migration / deprecated-api  
- **Breaking Change:** Yes  
- **Files:** All 11 `.aspx.cs` files  
- **Description:** Every code-behind file imports `System.Web`, `System.Web.UI`, and `System.Web.UI.WebControls`. These namespaces do not exist in .NET 8. The entire Web Forms page lifecycle (`System.Web.UI.Page`, `Page_Load`, `IsPostBack`, `Response`, `Request`, `Server`, `Session`) is unavailable.  
- **Code Snippet:**
```csharp
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AddTour : System.Web.UI.Page { ... }
```
- **Recommendation:** Replace all Web Forms pages with ASP.NET Core Razor Pages. Each `.aspx` page becomes a `.cshtml` Razor Page with a `PageModel` class. Remove all `System.Web.*` using statements.  
- **Effort:** High  

---

#### ISSUE-002: Web Forms Project Type GUID — Tour_Management.csproj
- **Severity:** Critical  
- **Category:** webforms-migration / breaking-change  
- **Breaking Change:** Yes  
- **File:** `Tour_Management.csproj` (line 8)  
- **Description:** The project uses the legacy Web Application project type GUID `{349c5851-65df-11da-9384-00065b846f21}` and the old MSBuild format. This project format is incompatible with .NET 8 SDK-style projects.  
- **Code Snippet:**
```xml
<ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>
<TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
```
- **Recommendation:** Replace the entire `.csproj` with an SDK-style project file using `<Project Sdk="Microsoft.NET.Sdk.Web">` and `<TargetFramework>net8.0</TargetFramework>`.  
- **Effort:** High  

---

#### ISSUE-003: Web.config Configuration System — Web.config
- **Severity:** Critical  
- **Category:** webforms-migration / breaking-change  
- **Breaking Change:** Yes  
- **File:** `Web.config` (entire file)  
- **Description:** The application uses `Web.config` for all configuration including connection strings, compilation settings, and HTTP handlers. `Web.config` is not used in ASP.NET Core / .NET 8 applications.  
- **Code Snippet:**
```xml
<system.web>
  <compilation debug="true" targetFramework="4.7.2">
  <httpRuntime targetFramework="4.7.2"/>
</system.web>
<connectionStrings>
  <add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;..."/>
</connectionStrings>
```
- **Recommendation:** Migrate all settings to `appsettings.json`. Connection strings go under `"ConnectionStrings"` key. Configure the application in `Program.cs`.  
- **Effort:** Medium  

---

#### ISSUE-004: SQL Injection Vulnerability — userlogin.aspx.cs
- **Severity:** Critical  
- **Category:** security  
- **Breaking Change:** No  
- **File:** `userlogin.aspx.cs` (line 28)  
- **Description:** The login query concatenates user input directly into the SQL string, creating a critical SQL injection vulnerability.  
- **Code Snippet:**
```csharp
string checkPasswordQuery = "select password from Userinfo where password='" 
    + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
```
- **Recommendation:** Replace with parameterized queries or EF Core. In .NET 8, use `DbContext` with LINQ or `FromSqlInterpolated`. Never concatenate user input into SQL strings.  
- **Effort:** Medium  

---

#### ISSUE-005: Plain-Text Password Storage — SignUpForm.aspx.cs
- **Severity:** Critical  
- **Category:** security  
- **Breaking Change:** No  
- **File:** `SignUpForm.aspx.cs` (line 22)  
- **Description:** User passwords are stored in plain text in the database. This is a critical security vulnerability.  
- **Code Snippet:**
```csharp
com.Parameters.AddWithValue("@Password", password1.Text);
```
- **Recommendation:** Implement ASP.NET Core Identity with `PasswordHasher<T>` for password hashing. Never store plain-text passwords. Migrate existing passwords by forcing a password reset.  
- **Effort:** High  

---

#### ISSUE-006: Hardcoded Admin Credentials — AdminLogin2.aspx.cs
- **Severity:** Critical  
- **Category:** security  
- **Breaking Change:** No  
- **File:** `AdminLogin2.aspx.cs` (lines 14–18)  
- **Description:** Admin authentication uses hardcoded credentials checked in `Page_Load`. This is a critical security vulnerability and the logic runs on every page load, not just on form submission.  
- **Code Snippet:**
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (password.Text == "admin" && name.Text == "admin@gmail.com")
    {
        Response.Redirect("AdminProfile.aspx");
        Server.Transfer("AdminProfile.aspx");
    }
}
```
- **Recommendation:** Implement role-based authentication using ASP.NET Core Identity. Use `[Authorize(Roles = "Admin")]` attribute on admin pages. Remove all hardcoded credentials.  
- **Effort:** High  

---

#### ISSUE-007: SqlDataSource Server Controls — DisplayTours.aspx, TourCrud.aspx, allbooking.aspx, mybooking.aspx, usercrud.aspx
- **Severity:** Critical  
- **Category:** webforms-migration  
- **Breaking Change:** Yes  
- **Files:** `DisplayTours.aspx`, `TourCrud.aspx`, `allbooking.aspx`, `mybooking.aspx`, `usercrud.aspx`  
- **Description:** `<asp:SqlDataSource>` is a Web Forms server control that does not exist in .NET 8. It embeds SQL queries directly in markup and relies on the Web Forms data binding pipeline.  
- **Code Snippet:**
```xml
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:dbconnection %>" 
    SelectCommand="SELECT * FROM [Tour]"
    UpdateCommand="UPDATE [Tour] Set [TOUR_NAME]=@TOUR_NAME..."
    DeleteCommand="Delete from [Tour] Where [TOUR_ID]=@TOUR_ID">
</asp:SqlDataSource>
```
- **Recommendation:** Replace with EF Core repositories and Razor Page handlers. Use `OnGetAsync()` to load data and `OnPostAsync()` for mutations. Render data using Razor `@foreach` loops with HTML tables.  
- **Effort:** High  

---

#### ISSUE-008: GridView Server Controls — Multiple Pages
- **Severity:** Critical  
- **Category:** webforms-migration  
- **Breaking Change:** Yes  
- **Files:** `DisplayTours.aspx`, `TourCrud.aspx`, `allbooking.aspx`, `mybooking.aspx`, `usercrud.aspx`  
- **Description:** `<asp:GridView>` is a Web Forms server control that does not exist in .NET 8. It relies on ViewState, postback, and the Web Forms rendering pipeline.  
- **Code Snippet:**
```xml
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" 
    DataKeyNames="TOUR_ID" DataSourceID="SqlDataSource1" ...>
```
- **Recommendation:** Replace with HTML `<table>` elements rendered via Razor `@foreach` loops. For edit/delete functionality, use form posts or AJAX calls to Razor Page handlers.  
- **Effort:** High  

---

#### ISSUE-009: Server.MapPath Usage — AddTour.aspx.cs
- **Severity:** Critical  
- **Category:** deprecated-api  
- **Breaking Change:** Yes  
- **File:** `AddTour.aspx.cs` (line 26)  
- **Description:** `Server.MapPath()` is a `System.Web.HttpServerUtility` method that does not exist in .NET 8.  
- **Code Snippet:**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
```
- **Recommendation:** Use `IWebHostEnvironment.WebRootPath` injected via DI. In Razor Pages: `Path.Combine(_env.WebRootPath, "Tour_pics", fileName)`.  
- **Effort:** Medium  

---

#### ISSUE-010: Response.Write Usage — AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs
- **Severity:** Critical  
- **Category:** deprecated-api  
- **Breaking Change:** Yes  
- **Files:** `AddTour.aspx.cs` (line 30), `Order.aspx.cs` (line 22), `SignUpForm.aspx.cs` (line 27), `userlogin.aspx.cs` (line 37)  
- **Description:** `Response.Write()` is a `System.Web.HttpResponse` method. While `HttpResponse` exists in ASP.NET Core, `Response.Write()` is not the correct pattern for Razor Pages. The intent here is to show success/error messages.  
- **Code Snippet:**
```csharp
Response.Write("ADD Successful");
Response.Write("Registration Successful");
```
- **Recommendation:** Use `TempData["Message"]` to pass messages between redirects, or use `ModelState` for validation errors. Display messages in the Razor view using `@TempData["Message"]`.  
- **Effort:** Low  

---

#### ISSUE-011: Response.Redirect + Server.Transfer Pattern — Multiple Files
- **Severity:** Critical  
- **Category:** deprecated-api  
- **Breaking Change:** Yes  
- **Files:** `Order.aspx.cs` (lines 23–24), `SignUpForm.aspx.cs` (lines 28–29), `userlogin.aspx.cs` (lines 38–39, 47–48), `AdminLogin2.aspx.cs` (lines 15–16)  
- **Description:** Multiple files call both `Response.Redirect()` AND `Server.Transfer()` for the same destination. `Server.Transfer()` does not exist in ASP.NET Core. Calling both is also logically incorrect — `Response.Redirect` sends a 302 response and the subsequent `Server.Transfer` would never execute.  
- **Code Snippet:**
```csharp
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");  // Dead code - never reached
conn.Close();  // Also dead code
```
- **Recommendation:** Use `return RedirectToPage("/MyBooking")` in Razor Page handlers. Remove all `Server.Transfer()` calls. Ensure database connections are closed before redirecting (use `using` statements).  
- **Effort:** Medium  

---

#### ISSUE-012: System.Web.DataVisualization Chart Control — allbooking.aspx, Web.config
- **Severity:** Critical  
- **Category:** webforms-migration / deprecated-api  
- **Breaking Change:** Yes  
- **Files:** `allbooking.aspx` (line 2), `Web.config` (lines 4–8, 12–15, 20–22)  
- **Description:** The application registers `System.Web.UI.DataVisualization.Charting.ChartHttpHandler` and the `System.Web.DataVisualization` assembly. This is a .NET Framework-only component with no equivalent in .NET 8.  
- **Code Snippet:**
```xml
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, ..." %>
<add name="ChartImageHandler" ... type="System.Web.UI.DataVisualization.Charting.ChartHttpHandler, System.Web.DataVisualization..." />
```
- **Recommendation:** Replace with a modern charting library such as Chart.js (client-side JavaScript) or use the `LiveCharts2` NuGet package for server-side chart generation. Remove all `System.Web.DataVisualization` references.  
- **Effort:** High  

---

### HIGH Issues

#### ISSUE-013: Raw ADO.NET Data Access — AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs, TourCrud.aspx.cs
- **Severity:** High  
- **Category:** deprecated-api / data-access  
- **Breaking Change:** No (ADO.NET works in .NET 8, but pattern is incompatible with clean architecture)  
- **Files:** `AddTour.aspx.cs`, `Order.aspx.cs`, `SignUpForm.aspx.cs`, `userlogin.aspx.cs`, `TourCrud.aspx.cs`  
- **Description:** All data access uses raw `SqlConnection`, `SqlCommand`, and `ConfigurationManager.ConnectionStrings`. While `System.Data.SqlClient` is available in .NET 8 via `Microsoft.Data.SqlClient`, the pattern of opening connections in page event handlers violates separation of concerns and makes testing impossible.  
- **Code Snippet:**
```csharp
SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
conn.Open();
SqlCommand com = new SqlCommand(insertQuery, conn);
com.ExecuteNonQuery();
conn.Close();
```
- **Recommendation:** Replace with Entity Framework Core 8.0.0. Create a `TourManagementDbContext` with `DbSet<Tour>`, `DbSet<UserInfo>`, and `DbSet<Booking>`. Use repository pattern with async operations.  
- **Effort:** High  

---

#### ISSUE-014: ConfigurationManager Usage — AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs, TourCrud.aspx.cs
- **Severity:** High  
- **Category:** deprecated-api  
- **Breaking Change:** Yes  
- **Files:** All files using `ConfigurationManager`  
- **Description:** `System.Configuration.ConfigurationManager` is a .NET Framework API. While a compatibility NuGet package exists, it is not the recommended approach for .NET 8.  
- **Code Snippet:**
```csharp
using System.Configuration;
ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString
```
- **Recommendation:** Use `IConfiguration` injected via DI. Register connection strings in `appsettings.json` and access via `builder.Configuration.GetConnectionString("dbconnection")` in `Program.cs`.  
- **Effort:** Medium  

---

#### ISSUE-015: FileUpload Server Control — AddTour.aspx
- **Severity:** High  
- **Category:** webforms-migration  
- **Breaking Change:** Yes  
- **File:** `AddTour.aspx` (line 52), `AddTour.aspx.cs` (lines 25–27)  
- **Description:** `<asp:FileUpload>` is a Web Forms server control. In .NET 8 Razor Pages, file uploads are handled via `IFormFile`.  
- **Code Snippet:**
```xml
<asp:FileUpload ID="FileUpload1" runat="server"/>
```
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
com.Parameters.AddWithValue("@pic", FileUpload1.FileName);
```
- **Recommendation:** Use `<input type="file" asp-for="UploadedFile" />` in Razor Pages with `IFormFile UploadedFile` property on the PageModel. Save using `IWebHostEnvironment.WebRootPath`.  
- **Effort:** Medium  

---

#### ISSUE-016: RegularExpressionValidator Server Control — AddTour.aspx
- **Severity:** High  
- **Category:** webforms-migration  
- **Breaking Change:** Yes  
- **File:** `AddTour.aspx` (line 72)  
- **Description:** `<asp:RegularExpressionValidator>` is a Web Forms server control that does not exist in .NET 8.  
- **Code Snippet:**
```xml
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
    ControlToValidate="tour_info" ValidationExpression="^[\s\S]{0,250}$" 
    runat="server" ErrorMessage="Characters less than 250">
</asp:RegularExpressionValidator>
```
- **Recommendation:** Use Data Annotations (`[MaxLength(250)]`) on the ViewModel/DTO and FluentValidation for complex rules. Display validation errors using `<span asp-validation-for="TourInfo"></span>` Tag Helpers.  
- **Effort:** Low  

---

#### ISSUE-017: Label / TextBox / Button / DropDownList Server Controls — All .aspx Files
- **Severity:** High  
- **Category:** webforms-migration  
- **Breaking Change:** Yes  
- **Files:** All `.aspx` files  
- **Description:** All Web Forms server controls (`<asp:Label>`, `<asp:TextBox>`, `<asp:Button>`, `<asp:DropDownList>`, `<asp:HyperLink>`) do not exist in .NET 8.  
- **Code Snippet:**
```xml
<asp:Label ID="Label1" runat="server" Text="Email"/>
<asp:TextBox ID="email" TextMode="Email" runat="server" class="form-control"/>
<asp:Button ID="Register" runat="server" Text="Register" OnClick="Register_Click"/>
<asp:DropDownList ID="gender" runat="server" Width="361px">
```
- **Recommendation:** Replace with standard HTML elements and ASP.NET Core Tag Helpers: `<label asp-for="Email">`, `<input asp-for="Email" class="form-control" />`, `<button type="submit">`, `<select asp-for="Gender" asp-items="...">`.  
- **Effort:** High  

---

#### ISSUE-018: No Authentication/Authorization Framework
- **Severity:** High  
- **Category:** security / webforms-migration  
- **Breaking Change:** No  
- **Files:** All pages  
- **Description:** The application has no authentication framework. There is no session management after login, no authorization checks on protected pages, and no logout mechanism. Any user can navigate directly to admin pages.  
- **Recommendation:** Implement ASP.NET Core Identity with cookie authentication. Add `[Authorize]` and `[Authorize(Roles = "Admin")]` attributes to protected pages. Implement proper login/logout flows.  
- **Effort:** High  

---

#### ISSUE-019: Database Connection Not Closed on Error — Multiple Files
- **Severity:** High  
- **Category:** data-access / reliability  
- **Breaking Change:** No  
- **Files:** `AddTour.aspx.cs`, `Order.aspx.cs`, `SignUpForm.aspx.cs`, `userlogin.aspx.cs`  
- **Description:** Database connections are opened but `conn.Close()` is placed after `Response.Redirect()` or `Response.Write()`, meaning the connection is never closed if an exception occurs or after a redirect.  
- **Code Snippet:**
```csharp
conn.Open();
// ... operations ...
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");
conn.Close();  // Never reached after redirect
```
- **Recommendation:** Use `using` statements for all `SqlConnection` and `SqlCommand` objects. In EF Core, the `DbContext` lifetime is managed by DI (Scoped).  
- **Effort:** Medium  

---

#### ISSUE-020: Microsoft.CodeDom.Providers.DotNetCompilerPlatform Package — packages.config
- **Severity:** High  
- **Category:** package-compatibility  
- **Breaking Change:** Yes  
- **File:** `packages.config` (line 3)  
- **Description:** `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` is a .NET Framework-only package used for Roslyn compiler support in Web Forms. It is not needed and not compatible with .NET 8.  
- **Code Snippet:**
```xml
<package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />
```
- **Recommendation:** Remove this package entirely. .NET 8 uses the Roslyn compiler natively. Delete `packages.config` and use `<PackageReference>` in the SDK-style `.csproj`.  
- **Effort:** Low  

---

#### ISSUE-021: Legacy packages.config Package Management
- **Severity:** High  
- **Category:** package-compatibility  
- **Breaking Change:** Yes  
- **File:** `packages.config`  
- **Description:** The project uses the legacy `packages.config` format for NuGet package management. SDK-style .NET 8 projects use `<PackageReference>` elements in the `.csproj` file.  
- **Recommendation:** Delete `packages.config`. Add all required packages as `<PackageReference>` elements in the new SDK-style `.csproj` file.  
- **Effort:** Low  

---

#### ISSUE-022: HyperLink Server Control with Static href — DisplayTours.aspx
- **Severity:** High  
- **Category:** webforms-migration  
- **Breaking Change:** Yes  
- **File:** `DisplayTours.aspx` (line 26)  
- **Description:** `<asp:HyperLink>` with a static `href` attribute does not pass the tour ID to the Order page, making it impossible to know which tour is being booked.  
- **Code Snippet:**
```xml
<asp:HyperLink ID="HyperLink1" href="Order.aspx" runat="server">Book Now</asp:HyperLink>
```
- **Recommendation:** Replace with `<a asp-page="/Order" asp-route-tourId="@item.TourId">Book Now</a>` in Razor Pages. Pass the tour ID as a route parameter and pre-populate the order form.  
- **Effort:** Medium  

---

### MEDIUM Issues

#### ISSUE-023: No Session State After Login — userlogin.aspx.cs
- **Severity:** Medium  
- **Category:** webforms-migration / security  
- **Breaking Change:** No  
- **File:** `userlogin.aspx.cs` (line 36, commented out)  
- **Description:** The session assignment is commented out (`//Session["New"] = txtEmail.Text;`), meaning there is no way to identify the logged-in user on subsequent pages.  
- **Code Snippet:**
```csharp
//Session["New"] = txtEmail.Text;
Response.Write("Password is correct");
Response.Redirect("MainProfilePage.aspx");
```
- **Recommendation:** Implement ASP.NET Core Identity with cookie authentication. After successful login, call `await _signInManager.SignInAsync(user, isPersistent: false)`. Access the current user via `User.Identity.Name` or `User.FindFirstValue(ClaimTypes.Email)`.  
- **Effort:** High  

---

#### ISSUE-024: Hardcoded Local File Path in Connection String — Web.config
- **Severity:** Medium  
- **Category:** configuration  
- **Breaking Change:** No  
- **File:** `Web.config` (line 30)  
- **Description:** The connection string contains a hardcoded local file path (`C:\Users\gajer\source\repos\...`) that is specific to the developer's machine.  
- **Code Snippet:**
```xml
<add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf;Integrated Security=True"/>
```
- **Recommendation:** Use environment-specific `appsettings.json` files (`appsettings.Development.json`, `appsettings.Production.json`). Use `|DataDirectory|` substitution or a proper SQL Server connection string.  
- **Effort:** Low  

---

#### ISSUE-025: No Input Validation on Server Side — Multiple Pages
- **Severity:** Medium  
- **Category:** security / data-quality  
- **Breaking Change:** No  
- **Files:** `AddTour.aspx.cs`, `Order.aspx.cs`, `SignUpForm.aspx.cs`  
- **Description:** Server-side validation is absent. The only validation is the `required` HTML attribute and one `RegularExpressionValidator` on the client side. All inputs are passed directly to SQL commands without validation.  
- **Recommendation:** Add Data Annotations to ViewModels/DTOs (`[Required]`, `[MaxLength]`, `[EmailAddress]`, `[Range]`). Use FluentValidation for complex rules. Check `ModelState.IsValid` in page handlers before processing.  
- **Effort:** Medium  

---

#### ISSUE-026: Dead Code After Redirect — Multiple Files
- **Severity:** Medium  
- **Category:** code-quality  
- **Breaking Change:** No  
- **Files:** `Order.aspx.cs` (lines 23–26), `SignUpForm.aspx.cs` (lines 28–31), `userlogin.aspx.cs` (lines 38–41, 47–48)  
- **Description:** Code after `Response.Redirect()` is unreachable. `conn.Close()` is never called, and `Server.Transfer()` is never reached.  
- **Code Snippet:**
```csharp
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");  // Dead code
conn.Close();                        // Dead code
```
- **Recommendation:** Remove dead code. Use `using` statements for connections. In Razor Pages, use `return RedirectToPage(...)` which immediately returns.  
- **Effort:** Low  

---

#### ISSUE-027: Inline SQL in ASPX Markup — DisplayTours.aspx, TourCrud.aspx, allbooking.aspx, mybooking.aspx, usercrud.aspx
- **Severity:** Medium  
- **Category:** data-access / code-quality  
- **Breaking Change:** Yes  
- **Files:** Multiple `.aspx` files  
- **Description:** SQL queries are embedded directly in ASPX markup via `<asp:SqlDataSource>`. This violates separation of concerns and makes the queries impossible to test or reuse.  
- **Code Snippet:**
```xml
SelectCommand="SELECT * FROM [Tour]"
UpdateCommand="UPDATE [Tour] Set [TOUR_NAME]=@TOUR_NAME,[PLACE]=@PLACE..."
DeleteCommand="Delete from [Tour] Where [TOUR_ID]=@TOUR_ID"
```
- **Recommendation:** Move all data access to repository classes. Use EF Core with LINQ queries. Expose data through service interfaces.  
- **Effort:** High  

---

#### ISSUE-028: Commented-Out Code in TourCrud.aspx.cs
- **Severity:** Medium  
- **Category:** code-quality  
- **Breaking Change:** No  
- **File:** `TourCrud.aspx.cs` (lines 18–30)  
- **Description:** Large blocks of commented-out code exist in `TourCrud.aspx.cs`. The `refreshdata()` method opens a connection but does nothing with it.  
- **Code Snippet:**
```csharp
public void refreshdata()
{
    SqlConnection conn = new SqlConnection(...);
    conn.Open();
    string insertQuery = "select * from Tour";
    SqlCommand com = new SqlCommand(insertQuery, conn);
    // GridView1.DataSource = insertQuery;
    // GridView1.DataBind();
    // ... more commented code ...
}
```
- **Recommendation:** Remove all commented-out code. The connection is opened but never closed — this is a resource leak. Replace with EF Core repository.  
- **Effort:** Low  

---

#### ISSUE-029: No Error Handling — All Code-Behind Files
- **Severity:** Medium  
- **Category:** reliability  
- **Breaking Change:** No  
- **Files:** All `.aspx.cs` files  
- **Description:** No try-catch blocks exist in any code-behind file. Database errors, file system errors, and other exceptions will result in unhandled exceptions shown to users.  
- **Recommendation:** Add try-catch blocks in all service methods. Use global exception handling middleware in `Program.cs` (`app.UseExceptionHandler("/Error")`). Log exceptions using `ILogger<T>`.  
- **Effort:** Medium  

---

#### ISSUE-030: No Logging Framework
- **Severity:** Medium  
- **Category:** observability  
- **Breaking Change:** No  
- **Files:** All files  
- **Description:** The application has no logging framework. There is no way to diagnose issues in production.  
- **Recommendation:** Use `Microsoft.Extensions.Logging` with `ILogger<T>` injected via DI. Configure Serilog.AspNetCore 8.0.0 for structured logging with file and console sinks.  
- **Effort:** Medium  

---

### LOW Issues

#### ISSUE-031: No Master Page / Consistent Layout
- **Severity:** Low  
- **Category:** webforms-migration / ui  
- **Breaking Change:** No  
- **Files:** All `.aspx` files  
- **Description:** The application has no master page. Navigation is duplicated across pages (AdminProfile.aspx and MainProfilePage.aspx have inline `<ul>` navigation). This leads to inconsistent UI.  
- **Recommendation:** Create a shared `_Layout.cshtml` in Razor Pages. Move navigation to the layout. Use `_ViewStart.cshtml` to apply the layout to all pages.  
- **Effort:** Medium  

---

#### ISSUE-032: Inline CSS Styles — Multiple .aspx Files
- **Severity:** Low  
- **Category:** ui / code-quality  
- **Breaking Change:** No  
- **Files:** All `.aspx` files  
- **Description:** All CSS is defined inline within `<style>` tags in each page. Styles are duplicated across pages (e.g., the `.container` style appears in at least 5 pages).  
- **Recommendation:** Create a shared `site.css` in `wwwroot/css/`. Use Bootstrap 5 for consistent styling. Reference the stylesheet from `_Layout.cshtml`.  
- **Effort:** Low  

---

#### ISSUE-033: Static Image References with Relative Paths — Multiple Pages
- **Severity:** Low  
- **Category:** webforms-migration  
- **Breaking Change:** No  
- **Files:** `AdminProfile.aspx`, `MainProfilePage.aspx`  
- **Description:** Background images use relative paths like `url('../Pics/adminhp.jpg')`. In .NET 8, static files are served from `wwwroot/`.  
- **Code Snippet:**
```css
background-image: url('../Pics/adminhp.jpg');
background-image: url('../Pics/homepage.jpg');
```
- **Recommendation:** Move all images to `wwwroot/images/`. Reference them as `url('/images/adminhp.jpg')` or use `asp-append-version="true"` for cache busting.  
- **Effort:** Low  

---

#### ISSUE-034: Non-SDK Project File Format — Tour_Management.csproj
- **Severity:** Low  
- **Category:** project-configuration  
- **Breaking Change:** Yes  
- **File:** `Tour_Management.csproj`  
- **Description:** The project file uses the old MSBuild format with explicit file listings, `<Import>` statements for MSBuild targets, and `<ProjectExtensions>` for IIS settings. This format is not compatible with .NET 8.  
- **Recommendation:** Replace with SDK-style project file. The new format automatically includes all `.cs` files and does not require explicit file listings.  
- **Effort:** Low  

---

## Migration Roadmap

### Phase 1: Foundation (Week 1–2) — ~20 hours
1. Create new SDK-style solution with clean architecture layers
2. Set up `appsettings.json` with connection strings
3. Create EF Core `DbContext` with entity models (Tour, UserInfo, Booking)
4. Run EF Core migrations to create database schema
5. Implement repository interfaces and implementations

### Phase 2: Authentication & Security (Week 2–3) — ~20 hours
1. Implement ASP.NET Core Identity
2. Create user registration with password hashing
3. Implement login/logout with cookie authentication
4. Add role-based authorization (Admin/User roles)
5. Fix SQL injection vulnerability in login

### Phase 3: Core Pages Migration (Week 3–5) — ~40 hours
1. Migrate `userlogin.aspx` → `Pages/Account/Login.cshtml`
2. Migrate `SignUpForm.aspx` → `Pages/Account/Register.cshtml`
3. Migrate `MainProfilePage.aspx` → `Pages/Index.cshtml`
4. Migrate `DisplayTours.aspx` → `Pages/Tours/Index.cshtml`
5. Migrate `Order.aspx` → `Pages/Tours/Book.cshtml`
6. Migrate `mybooking.aspx` → `Pages/Bookings/MyBookings.cshtml`
7. Migrate `AdminProfile.aspx` → `Pages/Admin/Index.cshtml`
8. Migrate `AddTour.aspx` → `Pages/Admin/Tours/Create.cshtml`
9. Migrate `TourCrud.aspx` → `Pages/Admin/Tours/Index.cshtml`
10. Migrate `allbooking.aspx` → `Pages/Admin/Bookings/Index.cshtml`
11. Migrate `usercrud.aspx` → `Pages/Admin/Users/Index.cshtml`
12. Migrate `AdminLogin2.aspx` → Remove (use Identity login)

### Phase 4: UI & Static Files (Week 5–6) — ~15 hours
1. Create `_Layout.cshtml` with Bootstrap 5 navigation
2. Move images to `wwwroot/images/`
3. Create `wwwroot/css/site.css`
4. Replace charting with Chart.js
5. Add client-side validation with jQuery Validation

### Phase 5: Testing & Documentation (Week 6) — ~15 hours
1. Write unit tests for services
2. Write integration tests for repositories
3. Create `docs/MIGRATION_NOTES.md`
4. Create `docs/ARCHITECTURE.md`
5. Update `README.md`

---

## Migration Mapping Table

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| `System.Web.UI.Page` | `PageModel` (Razor Pages) |
| `Page_Load` | `OnGet()` / `OnGetAsync()` |
| `Button.OnClick` | `OnPost()` / `OnPostAsync()` |
| `<asp:GridView>` | HTML `<table>` + Razor `@foreach` |
| `<asp:SqlDataSource>` | EF Core `DbContext` + Repository |
| `<asp:TextBox>` | `<input asp-for="..." />` |
| `<asp:Label>` | `<label asp-for="..." />` |
| `<asp:Button>` | `<button type="submit">` |
| `<asp:DropDownList>` | `<select asp-for="..." asp-items="...">` |
| `<asp:FileUpload>` | `<input type="file" asp-for="..." />` + `IFormFile` |
| `<asp:RegularExpressionValidator>` | Data Annotations + `<span asp-validation-for="...">` |
| `<asp:HyperLink>` | `<a asp-page="..." asp-route-id="...">` |
| `Response.Redirect()` | `return RedirectToPage(...)` |
| `Server.Transfer()` | Remove (no equivalent needed) |
| `Server.MapPath()` | `IWebHostEnvironment.WebRootPath` |
| `Response.Write()` | `TempData["Message"]` |
| `ConfigurationManager` | `IConfiguration` |
| `SqlConnection` + `SqlCommand` | EF Core `DbContext` |
| `Web.config` | `appsettings.json` |
| `Global.asax` | `Program.cs` |
| Forms Authentication | ASP.NET Core Identity |
| `Session["key"]` | `HttpContext.Session` or Claims |
| `IsPostBack` | `HttpContext.Request.Method == "POST"` |
| `ViewState` | Hidden fields or TempData |

---

## Key Architecture for .NET 8 Target

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/
│   │   ├── Entities/
│   │   │   ├── Tour.cs
│   │   │   ├── UserInfo.cs
│   │   │   └── Booking.cs
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   │   ├── ITourRepository.cs
│   │   │   │   ├── IUserRepository.cs
│   │   │   │   └── IBookingRepository.cs
│   │   │   └── Services/
│   │   │       ├── ITourService.cs
│   │   │       ├── IUserService.cs
│   │   │       └── IBookingService.cs
│   │   └── Exceptions/
│   ├── TourManagement.Application/
│   │   ├── Services/
│   │   ├── DTOs/
│   │   ├── Mappings/
│   │   └── Validators/
│   ├── TourManagement.Infrastructure/
│   │   ├── Data/
│   │   │   ├── TourManagementDbContext.cs
│   │   │   ├── Configurations/
│   │   │   └── Migrations/
│   │   └── Repositories/
│   └── TourManagement.Web/
│       ├── Pages/
│       │   ├── Account/
│       │   │   ├── Login.cshtml
│       │   │   └── Register.cshtml
│       │   ├── Tours/
│       │   │   ├── Index.cshtml
│       │   │   └── Book.cshtml
│       │   ├── Bookings/
│       │   │   └── MyBookings.cshtml
│       │   └── Admin/
│       │       ├── Index.cshtml
│       │       ├── Tours/
│       │       ├── Bookings/
│       │       └── Users/
│       ├── wwwroot/
│       │   ├── css/site.css
│       │   ├── js/site.js
│       │   └── images/
│       ├── Program.cs
│       └── appsettings.json
├── tests/
│   ├── TourManagement.UnitTests/
│   └── TourManagement.IntegrationTests/
└── docs/
    ├── MIGRATION_NOTES.md
    ├── ARCHITECTURE.md
    └── BUILD_VERIFICATION.md
```

---

## Recommendations Summary

| Priority | Category | Recommendation | Effort |
|----------|----------|----------------|--------|
| High | Security | Fix SQL injection in userlogin.aspx.cs immediately | 2 hours |
| High | Security | Implement password hashing (replace plain-text storage) | 4 hours |
| High | Security | Remove hardcoded admin credentials | 2 hours |
| High | Architecture | Create SDK-style .NET 8 solution with clean architecture | 8 hours |
| High | Data Access | Replace ADO.NET with EF Core 8.0.0 | 16 hours |
| High | Authentication | Implement ASP.NET Core Identity | 12 hours |
| High | UI | Migrate all Web Forms pages to Razor Pages | 30 hours |
| Medium | Configuration | Migrate Web.config to appsettings.json | 4 hours |
| Medium | Reliability | Add error handling and logging throughout | 8 hours |
| Medium | UI | Create shared layout with Bootstrap 5 | 6 hours |
| Low | Code Quality | Remove dead code and commented-out code | 2 hours |
| Low | Static Files | Move images to wwwroot, update references | 2 hours |
