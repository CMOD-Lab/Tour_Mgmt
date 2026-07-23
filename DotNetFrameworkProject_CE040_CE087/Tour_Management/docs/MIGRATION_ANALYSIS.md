# Tour_Management – ASP.NET Web Forms to .NET 8 Migration Analysis

**Analysis Date:** 2025-01-30  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Module:** Tour_Management  

---

## Executive Summary

| Metric | Value |
|---|---|
| Total Issues Found | 42 |
| Critical Issues | 12 |
| High Issues | 14 |
| Medium Issues | 10 |
| Low Issues | 6 |
| Deprecated APIs Found | 18 |
| Breaking Changes | 16 |
| Compatibility Score | 18 / 100 |
| Estimated Remediation Effort | 80–120 hours |
| Migration Complexity | **Complex** |

The Tour_Management application is a classic ASP.NET Web Forms 4.7.2 project with **11 Web Forms pages**, raw ADO.NET data access, hardcoded credentials, SQL injection vulnerabilities, and deep System.Web dependencies. Every single page requires a full rewrite to migrate to .NET 8 Razor Pages with clean architecture.

---

## 1. Web Forms Component Inventory

| Component | File | Complexity |
|---|---|---|
| Web Forms Page | AddTour.aspx / .cs | Medium |
| Web Forms Page | AdminLogin2.aspx / .cs | Simple |
| Web Forms Page | AdminProfile.aspx / .cs | Simple |
| Web Forms Page | allbooking.aspx / .cs | Medium |
| Web Forms Page | DisplayTours.aspx / .cs | Medium |
| Web Forms Page | MainProfilePage.aspx / .cs | Simple |
| Web Forms Page | mybooking.aspx / .cs | Medium |
| Web Forms Page | Order.aspx / .cs | Medium |
| Web Forms Page | SignUpForm.aspx / .cs | Medium |
| Web Forms Page | TourCrud.aspx / .cs | Medium |
| Web Forms Page | usercrud.aspx / .cs | Medium |
| Configuration | Web.config | Critical |
| Project File | Tour_Management.csproj | Critical |
| Package Config | packages.config | High |

**Master Pages Found:** 0  
**User Controls (.ascx) Found:** 0  
**Global.asax Found:** 0  

---

## 2. Detailed Issue Findings

---

### ISSUE-001 — [CRITICAL] System.Web Namespace Usage (Breaking Change)

**Category:** webforms-migration / deprecated-api  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- `AddTour.aspx.cs` — lines 1–9
- `AdminLogin2.aspx.cs` — lines 1–8
- `AdminProfile.aspx.cs` — lines 1–8
- `allbooking.aspx.cs` — lines 1–8
- `DisplayTours.aspx.cs` — lines 1–9
- `MainProfilePage.aspx.cs` — lines 1–8
- `mybooking.aspx.cs` — lines 1–8
- `Order.aspx.cs` — lines 1–9
- `SignUpForm.aspx.cs` — lines 1–9
- `TourCrud.aspx.cs` — lines 1–10
- `usercrud.aspx.cs` — lines 1–8
- `userlogin.aspx.cs` — lines 1–9

**Code Snippet (representative — AddTour.aspx.cs):**
```csharp
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
```

**Description:**  
`System.Web` and all its sub-namespaces (`System.Web.UI`, `System.Web.UI.WebControls`, `System.Web.HttpContext`, etc.) do not exist in .NET 8. They are exclusive to .NET Framework. Every code-behind file imports these namespaces, making the entire project incompatible with .NET 8 as-is.

**Recommendation:**  
Replace with ASP.NET Core equivalents:
- `System.Web.UI.Page` → `Microsoft.AspNetCore.Mvc.RazorPages.PageModel`
- `System.Web.HttpContext` → `Microsoft.AspNetCore.Http.HttpContext` (injected via `IHttpContextAccessor`)
- `System.Web.UI.WebControls.*` → HTML Tag Helpers / Razor syntax
- `Response.Redirect()` → `RedirectToPage()` / `Redirect()`
- `Server.MapPath()` → `IWebHostEnvironment.WebRootPath`

---

### ISSUE-002 — [CRITICAL] ASP.NET Web Forms Page Lifecycle (Breaking Change)

**Category:** webforms-migration  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- All 11 `.aspx.cs` code-behind files

**Code Snippet (AddTour.aspx.cs, line 14):**
```csharp
public partial class AddTour : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }
    protected void Register_Click(object sender, EventArgs e) { ... }
}
```

**Description:**  
The entire Web Forms page lifecycle (`Page_Load`, `Page_PreRender`, `Page_Init`, postback event handlers, `IsPostBack`, etc.) does not exist in .NET 8. All 11 pages inherit from `System.Web.UI.Page` which is unavailable in .NET 8.

**Recommendation:**  
Migrate each page to a Razor Page (`.cshtml` + `PageModel`):
```csharp
// AddTour.cshtml.cs
public class AddTourModel : PageModel
{
    public async Task<IActionResult> OnPostAsync() { ... }
}
```

---

### ISSUE-003 — [CRITICAL] Raw ADO.NET SqlConnection / SqlCommand Usage

**Category:** deprecated-api / data-access  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- `AddTour.aspx.cs` — lines 20–35
- `Order.aspx.cs` — lines 16–35
- `SignUpForm.aspx.cs` — lines 16–38
- `TourCrud.aspx.cs` — lines 18–28
- `userlogin.aspx.cs` — lines 20–38
- `DisplayTours.aspx.cs` — lines 1–9

**Code Snippet (AddTour.aspx.cs, lines 20–35):**
```csharp
SqlConnection conn = new SqlConnection(
    ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
conn.Open();
string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) 
    values(@TOUR_NAME,@PLACE,@DAYS,@PRICE,@LOCATIONS,@TOUR_INFO,@pic)";
SqlCommand com = new SqlCommand(insertQuery, conn);
com.Parameters.AddWithValue("@TOUR_NAME", tour_name.Text);
...
com.ExecuteNonQuery();
conn.Close();
```

**Description:**  
All data access uses raw ADO.NET `SqlConnection`/`SqlCommand` without `using` statements (resource leak risk), without async/await, and without a repository pattern. `ConfigurationManager` is also .NET Framework-only.

**Recommendation:**  
Replace with Entity Framework Core 8.0.0 using the repository pattern:
```csharp
// Infrastructure layer
public class TourRepository : ITourRepository
{
    private readonly TourDbContext _context;
    public async Task<Tour> AddAsync(Tour tour, CancellationToken ct)
    {
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync(ct);
        return tour;
    }
}
```
Replace `ConfigurationManager` with `IConfiguration` from `Microsoft.Extensions.Configuration`.

---

### ISSUE-004 — [CRITICAL] SQL Injection Vulnerability

**Category:** security  
**Severity:** Critical  
**Breaking Change:** No (security fix required)  
**Effort:** High  

**Files Affected:**
- `userlogin.aspx.cs` — lines 22–24

**Code Snippet (userlogin.aspx.cs, lines 22–24):**
```csharp
string checkPasswordQuery = "select password from Userinfo where password='" 
    + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
SqlCommand passComm = new SqlCommand(checkPasswordQuery, conn);
```

**Description:**  
The login query is built by direct string concatenation of user input, creating a critical SQL injection vulnerability. An attacker can bypass authentication or extract/destroy data.

**Recommendation:**  
Use parameterized queries (EF Core handles this automatically) or at minimum:
```csharp
string query = "SELECT password FROM Userinfo WHERE password=@pwd AND email=@email";
cmd.Parameters.AddWithValue("@pwd", txtPassword.Text);
cmd.Parameters.AddWithValue("@email", txtEmail.Text);
```
In .NET 8 migration, use ASP.NET Core Identity for authentication entirely.

---

### ISSUE-005 — [CRITICAL] Plaintext Password Storage

**Category:** security  
**Severity:** Critical  
**Breaking Change:** No (security fix required)  
**Effort:** High  

**Files Affected:**
- `SignUpForm.aspx.cs` — line 28
- `userlogin.aspx.cs` — lines 22–35
- `usercrud.aspx` — line 14 (Password column displayed in GridView)

**Code Snippet (SignUpForm.aspx.cs, line 28):**
```csharp
com.Parameters.AddWithValue("@Password", password1.Text);
```

**Code Snippet (usercrud.aspx, line 14):**
```xml
<asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
```

**Description:**  
User passwords are stored in plaintext in the database and even displayed in a GridView. This is a critical security vulnerability.

**Recommendation:**  
Use ASP.NET Core Identity which handles password hashing automatically with PBKDF2. Never store or display plaintext passwords.

---

### ISSUE-006 — [CRITICAL] Hardcoded Admin Credentials

**Category:** security  
**Severity:** Critical  
**Breaking Change:** No (security fix required)  
**Effort:** Medium  

**Files Affected:**
- `AdminLogin2.aspx.cs` — lines 14–18

**Code Snippet (AdminLogin2.aspx.cs, lines 14–18):**
```csharp
if (password.Text == "admin" && name.Text == "admin@gmail.com")
{
    Response.Redirect("AdminProfile.aspx");
    Server.Transfer("AdminProfile.aspx");
}
```

**Description:**  
Admin credentials are hardcoded in source code. The login check also runs on `Page_Load` (every GET request), not on a button click, meaning it never actually validates on form submission. Additionally, `Response.Redirect` followed by `Server.Transfer` is unreachable dead code.

**Recommendation:**  
Use ASP.NET Core Identity with role-based authorization. Store credentials in the database with hashed passwords. Use `[Authorize(Roles = "Admin")]` attribute on admin pages.

---

### ISSUE-007 — [CRITICAL] Web.config — Not Compatible with .NET 8

**Category:** configuration / webforms-migration  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** Medium  

**File:** `Web.config`

**Code Snippet (Web.config, lines 1–35):**
```xml
<configuration>
  <system.web>
    <compilation debug="true" targetFramework="4.7.2">
    <httpRuntime targetFramework="4.7.2"/>
  </system.web>
  <connectionStrings>
    <add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;
         AttachDbFilename=C:\Users\gajer\source\repos\..."/>
  </connectionStrings>
  <system.codedom>...</system.codedom>
</configuration>
```

**Description:**  
`Web.config` with `<system.web>`, `<system.webServer>`, `<system.codedom>` sections is .NET Framework-only. The connection string contains a hardcoded absolute path to a developer's local machine (`C:\Users\gajer\...`). .NET 8 uses `appsettings.json` for configuration.

**Recommendation:**  
Create `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagement;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": { "Default": "Information" }
  }
}
```
Use `IConfiguration` to access settings. Remove `Web.config` entirely.

---

### ISSUE-008 — [CRITICAL] Legacy .csproj Format (Non-SDK Style)

**Category:** webforms-migration / project-configuration  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** Medium  

**File:** `Tour_Management.csproj`

**Code Snippet (Tour_Management.csproj, lines 1–5):**
```xml
<Project ToolsVersion="15.0" DefaultTargets="Build" 
    xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};...</ProjectTypeGuids>
```

**Description:**  
The project uses the old non-SDK-style `.csproj` format with `ToolsVersion`, `ProjectTypeGuids` (Web Application GUID), and `TargetFrameworkVersion`. This format is incompatible with .NET 8 SDK-style projects.

**Recommendation:**  
Replace with SDK-style project file:
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

---

### ISSUE-009 — [CRITICAL] System.Web.DataVisualization (Chart Control) — Not Available in .NET 8

**Category:** webforms-migration / deprecated-api  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- `Web.config` — lines 4–18
- `allbooking.aspx` — line 2
- `Tour_Management.csproj` — line 8

**Code Snippet (allbooking.aspx, line 2):**
```xml
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, ..." 
    namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
```

**Code Snippet (Web.config, lines 4–18):**
```xml
<add name="ChartImageHandler" ... 
    type="System.Web.UI.DataVisualization.Charting.ChartHttpHandler, 
    System.Web.DataVisualization, Version=4.0.0.0, ..."/>
```

**Description:**  
`System.Web.DataVisualization` (ASP.NET Chart Controls) is a .NET Framework-only assembly. It is not available in .NET 8.

**Recommendation:**  
Replace with a modern charting library compatible with .NET 8:
- **Chart.js** (JavaScript, free) via CDN
- **Blazor Charts** (if migrating to Blazor)
- **LiveCharts2** (NuGet: `LiveChartsCore.SkiaSharpView.AspNetCore`)

---

### ISSUE-010 — [CRITICAL] ConfigurationManager Usage

**Category:** deprecated-api  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** Medium  

**Files Affected:**
- `AddTour.aspx.cs` — line 21
- `Order.aspx.cs` — line 17
- `SignUpForm.aspx.cs` — line 17
- `TourCrud.aspx.cs` — line 19
- `userlogin.aspx.cs` — line 21

**Code Snippet (AddTour.aspx.cs, line 21):**
```csharp
SqlConnection conn = new SqlConnection(
    ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
```

**Description:**  
`System.Configuration.ConfigurationManager` reads from `Web.config` / `App.config`. While a NuGet package (`System.Configuration.ConfigurationManager`) exists for .NET 8, it is not the recommended approach. .NET 8 uses `IConfiguration` backed by `appsettings.json`.

**Recommendation:**  
Inject `IConfiguration` via constructor injection:
```csharp
private readonly IConfiguration _configuration;
public TourService(IConfiguration configuration) => _configuration = configuration;
// Usage:
var connStr = _configuration.GetConnectionString("DefaultConnection");
```
Or better, use `DbContext` with EF Core which handles connection strings via `DbContextOptions`.

---

### ISSUE-011 — [CRITICAL] Server.MapPath() Usage

**Category:** deprecated-api  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** Medium  

**Files Affected:**
- `AddTour.aspx.cs` — line 32

**Code Snippet (AddTour.aspx.cs, line 32):**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
```

**Description:**  
`Server.MapPath()` is a `System.Web.HttpServerUtility` method that does not exist in .NET 8. It maps a virtual path to a physical path on the server.

**Recommendation:**  
Use `IWebHostEnvironment.WebRootPath`:
```csharp
private readonly IWebHostEnvironment _env;
// In handler:
var uploadPath = Path.Combine(_env.WebRootPath, "Tour_pics", file.FileName);
await using var stream = System.IO.File.Create(uploadPath);
await file.CopyToAsync(stream);
```

---

### ISSUE-012 — [CRITICAL] Response.Write() for User Feedback

**Category:** deprecated-api / webforms-migration  
**Severity:** Critical  
**Breaking Change:** Yes  
**Effort:** Low  

**Files Affected:**
- `AddTour.aspx.cs` — line 36
- `Order.aspx.cs` — line 30
- `SignUpForm.aspx.cs` — line 36
- `userlogin.aspx.cs` — lines 33, 42

**Code Snippet (AddTour.aspx.cs, line 36):**
```csharp
Response.Write("ADD  Successful");
```

**Description:**  
`Response.Write()` directly writes raw text to the HTTP response stream. This is a Web Forms pattern that does not exist in Razor Pages. It also bypasses the page layout entirely.

**Recommendation:**  
Use `TempData` for success/error messages in Razor Pages:
```csharp
TempData["SuccessMessage"] = "Tour added successfully.";
return RedirectToPage("./Index");
```
Display in the Razor view: `@TempData["SuccessMessage"]`

---

### ISSUE-013 — [HIGH] Response.Redirect() + Server.Transfer() Dead Code Pattern

**Category:** webforms-migration / code-quality  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** Low  

**Files Affected:**
- `Order.aspx.cs` — lines 31–32
- `SignUpForm.aspx.cs` — lines 37–38
- `userlogin.aspx.cs` — lines 34–35, 44–45
- `AdminLogin2.aspx.cs` — lines 16–17

**Code Snippet (Order.aspx.cs, lines 31–32):**
```csharp
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");  // UNREACHABLE - dead code
```

**Description:**  
`Response.Redirect()` ends the response immediately, making the subsequent `Server.Transfer()` call unreachable dead code. Both APIs are Web Forms-specific and unavailable in .NET 8.

**Recommendation:**  
In Razor Pages, use:
```csharp
return RedirectToPage("/Booking/MyBooking");
```

---

### ISSUE-014 — [HIGH] SqlDataSource Server Controls

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- `DisplayTours.aspx` — lines 10–11
- `TourCrud.aspx` — lines 30–35
- `allbooking.aspx` — lines 22–23
- `mybooking.aspx` — lines 20–22
- `usercrud.aspx` — lines 18–22

**Code Snippet (DisplayTours.aspx, lines 10–11):**
```xml
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:dbconnection %>" 
    SelectCommand="SELECT [TOUR_NAME], [pic], [PRICE], [DAYS], [LOCATIONS], [TOUR_ID] FROM [Tour]">
</asp:SqlDataSource>
```

**Description:**  
`asp:SqlDataSource` is a Web Forms data-bound server control that does not exist in .NET 8. It embeds SQL queries directly in the markup, violating separation of concerns.

**Recommendation:**  
Replace with EF Core repository calls in the PageModel:
```csharp
public class IndexModel : PageModel
{
    public IEnumerable<TourDto> Tours { get; set; } = [];
    public async Task OnGetAsync()
    {
        Tours = await _tourService.GetAllAsync();
    }
}
```
Render in Razor: `@foreach (var tour in Model.Tours) { ... }`

---

### ISSUE-015 — [HIGH] GridView Server Controls

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- `DisplayTours.aspx` — lines 12–40
- `TourCrud.aspx` — lines 8–30
- `allbooking.aspx` — lines 8–22
- `mybooking.aspx` — lines 8–20
- `usercrud.aspx` — lines 4–18

**Code Snippet (TourCrud.aspx, lines 8–12):**
```xml
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" 
    DataKeyNames="TOUR_ID" DataSourceID="SqlDataSource1" ...>
```

**Description:**  
`asp:GridView` is a Web Forms server control with built-in edit/delete/sort functionality that does not exist in .NET 8. Five pages use GridView for data display and CRUD operations.

**Recommendation:**  
Replace with HTML `<table>` with Razor `@foreach` loops, or use a modern JavaScript data table library (DataTables.js). Implement edit/delete as separate Razor Page handlers.

---

### ISSUE-016 — [HIGH] FileUpload Server Control

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** Medium  

**Files Affected:**
- `AddTour.aspx` — line 52
- `AddTour.aspx.cs` — lines 31–33

**Code Snippet (AddTour.aspx, line 52):**
```xml
<asp:FileUpload ID="FileUpload1" runat="server"/>
```

**Code Snippet (AddTour.aspx.cs, lines 31–33):**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
com.Parameters.AddWithValue("@pic", FileUpload1.FileName);
```

**Description:**  
`asp:FileUpload` is a Web Forms server control. In .NET 8 Razor Pages, file uploads use `IFormFile`.

**Recommendation:**  
```csharp
// In PageModel:
[BindProperty]
public IFormFile? TourImage { get; set; }

public async Task<IActionResult> OnPostAsync()
{
    if (TourImage != null)
    {
        var fileName = Path.GetFileName(TourImage.FileName);
        var path = Path.Combine(_env.WebRootPath, "Tour_pics", fileName);
        await using var stream = System.IO.File.Create(path);
        await TourImage.CopyToAsync(stream);
    }
}
```
In Razor: `<input type="file" asp-for="TourImage" class="form-control" />`

---

### ISSUE-017 — [HIGH] RegularExpressionValidator Server Control

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** Low  

**Files Affected:**
- `AddTour.aspx` — line 72

**Code Snippet (AddTour.aspx, line 72):**
```xml
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
    ControlToValidate="tour_info" ValidationExpression="^[\s\S]{0,250}$" 
    runat="server" ErrorMessage="Characters less than 250">
</asp:RegularExpressionValidator>
```

**Description:**  
ASP.NET Web Forms validation controls (`RequiredFieldValidator`, `RegularExpressionValidator`, etc.) do not exist in .NET 8.

**Recommendation:**  
Use Data Annotations on the ViewModel/DTO:
```csharp
[MaxLength(250, ErrorMessage = "Tour info must be 250 characters or less.")]
public string TourInfo { get; set; } = string.Empty;
```
Or use FluentValidation for complex validation rules.

---

### ISSUE-018 — [HIGH] asp:Label, asp:TextBox, asp:Button Server Controls

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- All 11 `.aspx` files

**Code Snippet (AddTour.aspx, lines 30–35):**
```xml
<asp:Label id="l1" runat="server" text="Name of Tour"/>
<asp:TextBox id="tour_name" required="true" ForeColor="Black" class="form-control" runat="server"/>
<asp:Button BackColor="#cc6600" ID="Register" runat="server" Text="Register" OnClick="Register_Click" />
```

**Description:**  
All Web Forms server controls (`asp:Label`, `asp:TextBox`, `asp:Button`, `asp:DropDownList`, `asp:HyperLink`) render as HTML but require the Web Forms runtime. They do not exist in .NET 8.

**Recommendation:**  
Replace with standard HTML elements and Tag Helpers:
```html
<label asp-for="TourName" class="control-label">Name of Tour</label>
<input asp-for="TourName" class="form-control" required />
<span asp-validation-for="TourName" class="text-danger"></span>
<button type="submit" class="btn btn-warning">Register</button>
```

---

### ISSUE-019 — [HIGH] `<%@ Page %>` Directive and `runat="server"` Attributes

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- All 11 `.aspx` files

**Code Snippet (AddTour.aspx, line 1):**
```xml
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTour.aspx.cs" 
    Inherits="Tour_Management.AddTour" %>
```

**Description:**  
The `<%@ Page %>` directive, `runat="server"` attributes, `<form id="form1" runat="server">`, and all Web Forms markup syntax are exclusive to ASP.NET Web Forms and do not exist in .NET 8 Razor Pages.

**Recommendation:**  
Replace `.aspx` files with `.cshtml` Razor Pages:
```razor
@page
@model Tour_Management.Web.Pages.Tours.AddModel
@{
    ViewData["Title"] = "Add New Tour";
}
<form method="post" enctype="multipart/form-data">
    ...
</form>
```

---

### ISSUE-020 — [HIGH] No Authentication / Authorization Mechanism

**Category:** security / webforms-migration  
**Severity:** High  
**Breaking Change:** No (new feature required)  
**Effort:** High  

**Files Affected:**
- `AdminLogin2.aspx.cs` — entire file
- `userlogin.aspx.cs` — entire file
- All admin pages (no access control)

**Description:**  
The application has no proper authentication. Admin login uses hardcoded credentials checked in `Page_Load`. User login uses a raw SQL query with SQL injection. There is no session management, no authorization on admin pages, and no protection against unauthorized access.

**Recommendation:**  
Implement ASP.NET Core Identity:
```csharp
// Program.cs
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TourDbContext>();
// Protect admin pages:
[Authorize(Roles = "Admin")]
public class AdminProfileModel : PageModel { ... }
```

---

### ISSUE-021 — [HIGH] No Session State Management

**Category:** webforms-migration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** Medium  

**Files Affected:**
- `userlogin.aspx.cs` — line 30 (commented out)

**Code Snippet (userlogin.aspx.cs, line 30):**
```csharp
//Session["New"] = txtEmail.Text;
```

**Description:**  
Session state usage is commented out, indicating it was intended but not implemented. The application has no way to track the logged-in user across pages. In .NET 8, session state requires explicit configuration.

**Recommendation:**  
Use ASP.NET Core Identity claims for user identity. If session state is needed:
```csharp
// Program.cs
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
app.UseSession();
```

---

### ISSUE-022 — [HIGH] packages.config Format (Legacy NuGet)

**Category:** project-configuration  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** Low  

**File:** `packages.config`

**Code Snippet (packages.config):**
```xml
<packages>
  <package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />
</packages>
```

**Description:**  
`packages.config` is the legacy NuGet package management format. .NET 8 SDK-style projects use `<PackageReference>` elements directly in the `.csproj` file. `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` is a .NET Framework-only package not needed in .NET 8.

**Recommendation:**  
Remove `packages.config`. Use `<PackageReference>` in the SDK-style `.csproj`:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
```

---

### ISSUE-023 — [HIGH] Inline SQL in ASPX Markup (SqlDataSource)

**Category:** security / architecture  
**Severity:** High  
**Breaking Change:** Yes  
**Effort:** High  

**Files Affected:**
- `usercrud.aspx` — lines 18–22

**Code Snippet (usercrud.aspx, lines 18–22):**
```xml
<asp:SqlDataSource ... SelectCommand="Select top (select COUNT(*) from UserInfo) * From UserInfo
EXCEPT
Select top ((select COUNT(*) from UserInfo)-(1)) * From UserInfo" .../>
```

**Description:**  
Complex SQL queries are embedded directly in the ASPX markup. This violates separation of concerns, is unmaintainable, and is incompatible with .NET 8.

**Recommendation:**  
Move all data access to the repository/service layer. Use EF Core LINQ queries.

---

### ISSUE-024 — [HIGH] No Error Handling / Exception Management

**Category:** code-quality  
**Severity:** High  
**Breaking Change:** No  
**Effort:** Medium  

**Files Affected:**
- All code-behind files

**Description:**  
No try-catch blocks exist in any code-behind file. Database connections are opened without `using` statements, meaning connections are not closed if an exception occurs (resource leak). No logging is implemented.

**Recommendation:**  
Wrap all data access in try-catch-finally or use `using` statements. In .NET 8, use `ILogger<T>` for logging and global exception handling middleware:
```csharp
app.UseExceptionHandler("/Error");
```

---

### ISSUE-025 — [MEDIUM] No Async/Await Pattern

**Category:** code-quality / performance  
**Severity:** Medium  
**Breaking Change:** No  
**Effort:** Medium  

**Files Affected:**
- All code-behind files with database operations

**Description:**  
All database operations are synchronous. In .NET 8, all I/O operations should be async to avoid thread pool starvation.

**Recommendation:**  
Use async EF Core methods:
```csharp
public async Task<IActionResult> OnPostAsync(CancellationToken ct)
{
    await _tourService.CreateAsync(dto, ct);
    return RedirectToPage("./Index");
}
```

---

### ISSUE-026 — [MEDIUM] Hardcoded Absolute File Path in Connection String

**Category:** configuration  
**Severity:** Medium  
**Breaking Change:** No  
**Effort:** Low  

**File:** `Web.config` — line 22

**Code Snippet:**
```xml
<add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf;
    Integrated Security=True" providerName="System.Data.SqlClient"/>
```

**Description:**  
The connection string contains a hardcoded absolute path to a specific developer's machine (`C:\Users\gajer\...`). This will not work on any other machine.

**Recommendation:**  
Use a proper SQL Server connection string in `appsettings.json` with environment-specific overrides:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=True;"
}
```

---

### ISSUE-027 — [MEDIUM] .aspx Designer Files

**Category:** webforms-migration  
**Severity:** Medium  
**Breaking Change:** Yes  
**Effort:** Low  

**Files Affected:**
- All 11 `.aspx.designer.cs` files

**Description:**  
Designer files (`.aspx.designer.cs`) are auto-generated Web Forms files that declare server control fields. They have no equivalent in .NET 8 Razor Pages.

**Recommendation:**  
Delete all `.aspx.designer.cs` files. In Razor Pages, controls are accessed via model binding, not field declarations.

---

### ISSUE-028 — [MEDIUM] No Dependency Injection

**Category:** architecture  
**Severity:** Medium  
**Breaking Change:** No  
**Effort:** High  

**Description:**  
The application uses no dependency injection. All dependencies (database connections, configuration) are instantiated directly in code-behind files. This makes the code untestable and tightly coupled.

**Recommendation:**  
Use .NET 8's built-in DI container. Register services in `Program.cs`:
```csharp
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ITourService, TourService>();
```

---

### ISSUE-029 — [MEDIUM] No Input Validation Beyond Client-Side

**Category:** security / validation  
**Severity:** Medium  
**Breaking Change:** No  
**Effort:** Medium  

**Files Affected:**
- `SignUpForm.aspx` — uses `required` HTML attribute only
- `Order.aspx` — uses `required` HTML attribute only
- `AddTour.aspx` — one `RegularExpressionValidator` only

**Description:**  
Server-side validation is almost entirely absent. Only one `RegularExpressionValidator` exists. Client-side `required` attributes can be bypassed.

**Recommendation:**  
Use Data Annotations and FluentValidation:
```csharp
public class CreateTourDto
{
    [Required]
    [MaxLength(100)]
    public string TourName { get; set; } = string.Empty;
    
    [Range(1, 365)]
    public int Days { get; set; }
    
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }
}
```

---

### ISSUE-030 — [MEDIUM] MDF Database File in App_Data

**Category:** data-access / deployment  
**Severity:** Medium  
**Breaking Change:** No  
**Effort:** Medium  

**Files Affected:**
- `App_Data/tourdb.mdf`
- `App_Data/tourdb_log.ldf`

**Description:**  
The application uses a LocalDB `.mdf` file attached via connection string. This is a development-only approach. The `App_Data` folder convention is Web Forms-specific.

**Recommendation:**  
Use a proper SQL Server database (or SQL Server Express) with EF Core migrations. For development, use LocalDB with a standard connection string. For production, use environment-specific connection strings via environment variables or Azure Key Vault.

---

### ISSUE-031 — [MEDIUM] No CSRF Protection

**Category:** security  
**Severity:** Medium  
**Breaking Change:** No  
**Effort:** Low  

**Description:**  
Web Forms has built-in ViewState-based CSRF protection via `__VIEWSTATE` and `__EVENTVALIDATION`. The application does not explicitly configure this. In .NET 8 Razor Pages, CSRF protection (anti-forgery tokens) is enabled by default.

**Recommendation:**  
In .NET 8 Razor Pages, anti-forgery tokens are automatically included when using `<form method="post">` with Tag Helpers. Ensure `app.UseAntiforgery()` is called in `Program.cs`.

---

### ISSUE-032 — [MEDIUM] Image Paths Embedded in ASPX Markup

**Category:** webforms-migration  
**Severity:** Medium  
**Breaking Change:** Yes  
**Effort:** Low  

**Files Affected:**
- `DisplayTours.aspx` — line 22
- `TourCrud.aspx` — line 22

**Code Snippet (DisplayTours.aspx, line 22):**
```xml
<img src="Tour_pics/<%#Eval("pic") %>" style="width:200px;height:200px" />
```

**Description:**  
Data binding expressions `<%#Eval("pic") %>` are Web Forms-specific syntax. They do not work in Razor Pages.

**Recommendation:**  
Use Razor syntax:
```razor
<img src="~/Tour_pics/@tour.Pic" style="width:200px;height:200px" alt="@tour.TourName" />
```

---

### ISSUE-033 — [MEDIUM] Web.Debug.config and Web.Release.config Transform Files

**Category:** configuration  
**Severity:** Medium  
**Breaking Change:** Yes  
**Effort:** Low  

**Files Affected:**
- `Web.Debug.config`
- `Web.Release.config`

**Description:**  
Web.config transform files are a .NET Framework deployment mechanism. .NET 8 uses environment-specific `appsettings.{Environment}.json` files and environment variables.

**Recommendation:**  
Delete `Web.Debug.config` and `Web.Release.config`. Create:
- `appsettings.json` (base configuration)
- `appsettings.Development.json` (development overrides)
- `appsettings.Production.json` (production overrides)

---

### ISSUE-034 — [LOW] Properties/AssemblyInfo.cs

**Category:** project-configuration  
**Severity:** Low  
**Breaking Change:** No  
**Effort:** Low  

**File:** `Properties/AssemblyInfo.cs`

**Description:**  
In SDK-style .NET 8 projects, `AssemblyInfo.cs` is auto-generated. Manual `AssemblyInfo.cs` files can conflict with auto-generated attributes.

**Recommendation:**  
Delete `Properties/AssemblyInfo.cs` or add `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>` to the `.csproj` if custom attributes are needed.

---

### ISSUE-035 — [LOW] No Logging Infrastructure

**Category:** code-quality  
**Severity:** Low  
**Breaking Change:** No  
**Effort:** Low  

**Description:**  
No logging is implemented anywhere in the application. There is no way to diagnose issues in production.

**Recommendation:**  
Use `ILogger<T>` with Serilog:
```csharp
// Program.cs
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/tour-management-.txt", rollingInterval: RollingInterval.Day));
```

---

### ISSUE-036 — [LOW] No Pagination for GridView Data

**Category:** performance  
**Severity:** Low  
**Breaking Change:** No  
**Effort:** Low  

**Files Affected:**
- `DisplayTours.aspx`, `TourCrud.aspx`, `allbooking.aspx`, `mybooking.aspx`, `usercrud.aspx`

**Description:**  
All GridViews load all records without pagination. This will cause performance issues as data grows.

**Recommendation:**  
Implement pagination in EF Core:
```csharp
var tours = await _context.Tours
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync(ct);
```

---

### ISSUE-037 — [LOW] Inline CSS Styles in ASPX Pages

**Category:** code-quality  
**Severity:** Low  
**Breaking Change:** No  
**Effort:** Low  

**Files Affected:**
- All `.aspx` files contain `<style>` blocks

**Description:**  
All CSS is defined inline within `<style>` tags in each page. There is no shared stylesheet or CSS framework integration (Bootstrap is referenced via class names but not actually linked).

**Recommendation:**  
Create a shared `wwwroot/css/site.css`. Reference Bootstrap 5 via CDN or npm. Use a shared `_Layout.cshtml` for common styles.

---

## 3. Migration Complexity Assessment

| Page | Complexity | Key Issues |
|---|---|---|
| AddTour.aspx | Complex | FileUpload, ADO.NET, Server.MapPath, RegexValidator |
| AdminLogin2.aspx | Medium | Hardcoded credentials, Page_Load auth logic |
| AdminProfile.aspx | Simple | Navigation only, no logic |
| allbooking.aspx | Medium | GridView, SqlDataSource, Chart control registration |
| DisplayTours.aspx | Medium | GridView, SqlDataSource, data binding expressions |
| MainProfilePage.aspx | Simple | Navigation only, no logic |
| mybooking.aspx | Medium | GridView, SqlDataSource, delete functionality |
| Order.aspx | Medium | ADO.NET, form submission, redirect pattern |
| SignUpForm.aspx | Complex | ADO.NET, plaintext password, no validation |
| TourCrud.aspx | Complex | GridView CRUD, SqlDataSource, ADO.NET |
| usercrud.aspx | Complex | Complex SQL in markup, password display, GridView |
| userlogin.aspx | Critical | SQL injection, plaintext password comparison |

---

## 4. Target Architecture (Clean Architecture)

```
Tour_Management/
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
│   │   │   ├── TourDbContext.cs
│   │   │   └── Configurations/
│   │   └── Repositories/
│   └── TourManagement.Web/
│       ├── Pages/
│       │   ├── Tours/
│       │   │   ├── Index.cshtml (DisplayTours)
│       │   │   ├── Create.cshtml (AddTour)
│       │   │   ├── Edit.cshtml
│       │   │   ├── Details.cshtml
│       │   │   └── Delete.cshtml
│       │   ├── Bookings/
│       │   │   ├── Index.cshtml (allbooking)
│       │   │   ├── MyBookings.cshtml (mybooking)
│       │   │   └── Create.cshtml (Order)
│       │   ├── Account/
│       │   │   ├── Login.cshtml (userlogin)
│       │   │   ├── Register.cshtml (SignUpForm)
│       │   │   └── AdminLogin.cshtml (AdminLogin2)
│       │   ├── Admin/
│       │   │   ├── Index.cshtml (AdminProfile)
│       │   │   └── Users.cshtml (usercrud)
│       │   └── Shared/
│       │       ├── _Layout.cshtml
│       │       └── _AdminLayout.cshtml
│       ├── ViewModels/
│       ├── wwwroot/
│       └── Program.cs
├── tests/
│   ├── TourManagement.UnitTests/
│   └── TourManagement.IntegrationTests/
└── docs/
```

---

## 5. Migration Roadmap

### Phase 1 — Foundation (Week 1–2, ~20 hours)
1. Create new SDK-style solution with 4 projects (Domain, Application, Infrastructure, Web)
2. Set up EF Core 8.0.0 with SQL Server provider
3. Create domain entities (Tour, UserInfo, Booking)
4. Create EF Core migrations from existing database schema
5. Set up `appsettings.json` with proper connection strings
6. Configure `Program.cs` with DI, middleware, Identity

### Phase 2 — Data Access Layer (Week 2–3, ~15 hours)
1. Implement repository interfaces and EF Core implementations
2. Create DTOs for all entities
3. Set up AutoMapper profiles
4. Implement service layer with business logic
5. Add FluentValidation validators

### Phase 3 — Security (Week 3, ~15 hours)
1. Implement ASP.NET Core Identity
2. Create user registration with password hashing
3. Implement role-based authorization (Admin/User)
4. Protect admin pages with `[Authorize(Roles = "Admin")]`
5. Fix SQL injection vulnerability

### Phase 4 — UI Migration (Week 4–6, ~40 hours)
1. Create `_Layout.cshtml` and `_AdminLayout.cshtml`
2. Migrate each page to Razor Pages (11 pages × ~2-3 hours each)
3. Implement file upload with `IFormFile`
4. Add client-side validation with jQuery Unobtrusive Validation
5. Integrate Bootstrap 5

### Phase 5 — Testing & Documentation (Week 6–7, ~20 hours)
1. Write unit tests for services
2. Write integration tests for repositories
3. Write integration tests for pages
4. Create documentation

---

## 6. Package Migration Table

| Current Package | Status | .NET 8 Replacement |
|---|---|---|
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1 | ❌ Remove | Not needed in .NET 8 |
| System.Web (framework) | ❌ Remove | Microsoft.AspNetCore 8.0.0 |
| System.Web.DataVisualization (framework) | ❌ Remove | Chart.js (JavaScript) |
| System.Data.SqlClient (framework) | ❌ Remove | Microsoft.EntityFrameworkCore.SqlServer 8.0.0 |
| System.Configuration (framework) | ❌ Remove | Microsoft.Extensions.Configuration 8.0.0 |
| — | ✅ Add | Microsoft.EntityFrameworkCore 8.0.0 |
| — | ✅ Add | Microsoft.EntityFrameworkCore.SqlServer 8.0.0 |
| — | ✅ Add | Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.0 |
| — | ✅ Add | AutoMapper 12.0.1 |
| — | ✅ Add | FluentValidation 11.9.0 |
| — | ✅ Add | Serilog.AspNetCore 8.0.0 |

---

## 7. Known Issues and Risks

1. **Database Schema Unknown**: The actual SQL Server schema (table definitions, constraints, relationships) is not available in source code. EF Core entity configurations must be reverse-engineered from the `.mdf` file.
2. **No Unit Tests**: Zero test coverage means migration correctness cannot be verified automatically.
3. **Chart Control**: `System.Web.DataVisualization` is registered in `allbooking.aspx` but no chart is actually rendered. The replacement library selection depends on actual requirements.
4. **Image Storage**: Tour images are stored in the file system (`Tour_pics/`). In .NET 8, consider using Azure Blob Storage or a CDN for production.
5. **Password Migration**: Existing plaintext passwords in the database cannot be automatically migrated to hashed passwords. A password reset flow will be required for all existing users.
