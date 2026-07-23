# Tour_Management – ASP.NET Web Forms to .NET 8 Migration Analysis

**Analysis Date:** 2025-01-30  
**Module:** Tour_Management  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Migration Complexity:** Complex  
**Overall Compatibility Score:** 12 / 100

---

## Executive Summary

The Tour_Management application is a classic ASP.NET Web Forms project targeting .NET Framework 4.7.2.
It contains **11 Web Forms pages** (.aspx + code-behind), uses raw ADO.NET with `SqlConnection`/`SqlCommand`
for all data access, relies heavily on `System.Web` APIs, and stores plain-text passwords in the database.
None of these patterns are directly portable to .NET 8.

| Severity | Count |
|----------|-------|
| Critical | 12    |
| High     | 8     |
| Medium   | 6     |
| Low      | 4     |
| **Total**| **30**|

**Estimated Remediation Effort:** 80–120 hours  
**Deprecated APIs Found:** 18  
**Breaking Changes:** 20  

---

## 1. Web Forms Components Inventory

| File | Type | Complexity |
|------|------|------------|
| AddTour.aspx / .cs | Web Forms Page | Medium |
| AdminLogin2.aspx / .cs | Web Forms Page | Medium |
| AdminProfile.aspx / .cs | Web Forms Page | Simple |
| allbooking.aspx / .cs | Web Forms Page | Medium |
| DisplayTours.aspx / .cs | Web Forms Page | Medium |
| MainProfilePage.aspx / .cs | Web Forms Page | Simple |
| mybooking.aspx / .cs | Web Forms Page | Medium |
| Order.aspx / .cs | Web Forms Page | Medium |
| SignUpForm.aspx / .cs | Web Forms Page | Medium |
| TourCrud.aspx / .cs | Web Forms Page | Complex |
| usercrud.aspx / .cs | Web Forms Page | Complex |
| userlogin.aspx / .cs | Web Forms Page | Medium |

No master pages (.master), no user controls (.ascx), no Global.asax found.

---

## 2. Detailed Issue Findings

### ISSUE-001 [CRITICAL] – System.Web.UI.Page Inheritance (All Code-Behind Files)
All code-behind classes inherit from `System.Web.UI.Page`, which does not exist in .NET 8.

**Affected files:**
- AddTour.aspx.cs (line 12)
- AdminLogin2.aspx.cs (line 12)
- AdminProfile.aspx.cs (line 12)
- allbooking.aspx.cs (line 12)
- DisplayTours.aspx.cs (line 12)
- MainProfilePage.aspx.cs (line 12)
- mybooking.aspx.cs (line 12)
- Order.aspx.cs (line 12)
- SignUpForm.aspx.cs (line 12)
- TourCrud.aspx.cs (line 12)
- usercrud.aspx.cs (line 12)
- userlogin.aspx.cs (line 12)

**Code snippet:**
```csharp
public partial class AddTour : System.Web.UI.Page
```

**Remediation:** Replace each Web Forms page with a Razor Page (`.cshtml` + `PageModel`) or MVC Controller/View.

---

### ISSUE-002 [CRITICAL] – System.Web Namespace References (All Code-Behind Files)
`using System.Web;`, `using System.Web.UI;`, `using System.Web.UI.WebControls;` are present in every
code-behind file. `System.Web` is a .NET Framework-only assembly and is not available in .NET 8.

**Affected files:** All 12 code-behind files (lines 5–8 in each).

**Remediation:** Remove all `System.Web.*` using statements. Replace with ASP.NET Core equivalents
(`Microsoft.AspNetCore.Mvc`, `Microsoft.AspNetCore.Http`, etc.).

---

### ISSUE-003 [CRITICAL] – Raw ADO.NET SqlConnection / SqlCommand (Data Access)
Direct `SqlConnection` / `SqlCommand` usage without any ORM or abstraction layer.

**Affected files:**
- AddTour.aspx.cs (lines 21–38)
- Order.aspx.cs (lines 18–33)
- SignUpForm.aspx.cs (lines 18–36)
- userlogin.aspx.cs (lines 24–35)
- TourCrud.aspx.cs (lines 18–28)
- DisplayTours.aspx.cs (lines 1–14)

**Code snippet (AddTour.aspx.cs):**
```csharp
SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
conn.Open();
string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) values(...)";
SqlCommand com = new SqlCommand(insertQuery, conn);
```

**Remediation:** Replace with Entity Framework Core 8.0.0 (`DbContext`, `DbSet<T>`) and repository pattern.

---

### ISSUE-004 [CRITICAL] – SQL Injection Vulnerability (userlogin.aspx.cs)
Password check query uses string concatenation, creating a SQL injection vulnerability.

**File:** userlogin.aspx.cs, line 28

**Code snippet:**
```csharp
string checkPasswordQuery = "select password from Userinfo where password='" + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
```

**Remediation:** Use parameterized queries (EF Core handles this automatically) or at minimum
`SqlCommand.Parameters.AddWithValue()`.

---

### ISSUE-005 [CRITICAL] – Plain-Text Password Storage (SignUpForm.aspx.cs)
Passwords are stored in plain text in the database.

**File:** SignUpForm.aspx.cs, line 28

**Code snippet:**
```csharp
com.Parameters.AddWithValue("@Password", password1.Text);
```

**Remediation:** Use ASP.NET Core Identity with `IPasswordHasher<T>` for secure password hashing.

---

### ISSUE-006 [CRITICAL] – ConfigurationManager Usage (Multiple Files)
`System.Configuration.ConfigurationManager` is a .NET Framework API not available in .NET 8.

**Affected files:**
- AddTour.aspx.cs (line 21)
- Order.aspx.cs (line 18)
- SignUpForm.aspx.cs (line 18)
- userlogin.aspx.cs (line 24)
- TourCrud.aspx.cs (line 18)

**Code snippet:**
```csharp
ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString
```

**Remediation:** Use `IConfiguration` injected via DI, reading from `appsettings.json`.

---

### ISSUE-007 [CRITICAL] – Web.config Configuration File
`Web.config` is a .NET Framework configuration mechanism not supported in .NET 8.

**File:** Web.config

**Issues found:**
- `<system.web>` section (not supported)
- `<compilation debug="true" targetFramework="4.7.2">` (not supported)
- `<httpRuntime targetFramework="4.7.2"/>` (not supported)
- `<connectionStrings>` (must move to appsettings.json)
- `<appSettings>` (must move to appsettings.json)
- `<system.codedom>` (not supported)
- `<system.webServer>` handlers (not supported)

**Remediation:** Replace with `appsettings.json` and configure middleware in `Program.cs`.

---

### ISSUE-008 [CRITICAL] – Legacy .csproj Format (Non-SDK Style)
The project file uses the old MSBuild format with explicit file listings and Web Application project GUIDs.

**File:** Tour_Management.csproj (lines 1–130)

**Code snippet:**
```xml
<Project ToolsVersion="15.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
<ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>
<TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
```

**Remediation:** Replace with SDK-style project file:
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
</Project>
```

---

### ISSUE-009 [CRITICAL] – Server.MapPath Usage (AddTour.aspx.cs)
`Server.MapPath()` is a `System.Web.HttpServerUtility` method not available in .NET 8.

**File:** AddTour.aspx.cs, line 33

**Code snippet:**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
```

**Remediation:** Use `IWebHostEnvironment.WebRootPath` or `IWebHostEnvironment.ContentRootPath` injected via DI.

---

### ISSUE-010 [CRITICAL] – Response.Write Usage (Multiple Files)
`Response.Write()` is a `System.Web.HttpResponse` method not available in .NET 8.

**Affected files:**
- AddTour.aspx.cs (line 37)
- Order.aspx.cs (line 28)
- SignUpForm.aspx.cs (line 33)
- userlogin.aspx.cs (lines 37, 44)

**Code snippet:**
```csharp
Response.Write("ADD  Successful");
```

**Remediation:** Use `TempData`, `ModelState`, or return appropriate `IActionResult` responses.

---

### ISSUE-011 [CRITICAL] – Response.Redirect + Server.Transfer Pattern (Multiple Files)
Both `Response.Redirect()` and `Server.Transfer()` are called sequentially, which is incorrect
(Response.Redirect throws ThreadAbortException; Server.Transfer is not available in .NET 8).

**Affected files:**
- AdminLogin2.aspx.cs (lines 16–17)
- Order.aspx.cs (lines 29–30)
- SignUpForm.aspx.cs (lines 34–35)
- userlogin.aspx.cs (lines 38–39, 48–49)

**Code snippet:**
```csharp
Response.Redirect("AdminProfile.aspx");
Server.Transfer("AdminProfile.aspx");  // unreachable and unavailable in .NET 8
```

**Remediation:** Use `return RedirectToPage("/AdminProfile")` in Razor Pages or `return Redirect(url)` in MVC.

---

### ISSUE-012 [CRITICAL] – System.Web.DataVisualization Chart Control (allbooking.aspx)
The page registers `System.Web.UI.DataVisualization.Charting` which is a .NET Framework-only assembly.

**File:** allbooking.aspx (line 2), Web.config (lines 4–8, 12–15, 19–22)

**Code snippet:**
```aspx
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, ..." %>
```

**Remediation:** Replace with a JavaScript charting library (Chart.js, ApexCharts) or use
`Microsoft.AspNetCore.Components` with a compatible charting component.

---

### ISSUE-013 [HIGH] – Page Lifecycle Events (Page_Load, IsPostBack)
Web Forms page lifecycle events (`Page_Load`, `Page_PreRender`, `IsPostBack`) do not exist in .NET 8.

**Affected files:**
- TourCrud.aspx.cs (lines 12–17): `Page_Load` + `Page.IsPostBack`
- All other code-behind files: `Page_Load` event handlers

**Code snippet (TourCrud.aspx.cs):**
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!Page.IsPostBack)
    {
        refreshdata();
    }
}
```

**Remediation:** Move initialization logic to `OnGet()` handler in Razor Pages or `[HttpGet]` action in MVC.

---

### ISSUE-014 [HIGH] – SqlDataSource Server Controls (DisplayTours, TourCrud, allbooking, mybooking, usercrud)
`<asp:SqlDataSource>` is a Web Forms data-bound control that does not exist in .NET 8.

**Affected files:**
- DisplayTours.aspx (line 8)
- TourCrud.aspx (lines 34–38)
- allbooking.aspx (line 22)
- mybooking.aspx (line 22)
- usercrud.aspx (lines 22–27)

**Code snippet:**
```aspx
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbconnection %>" SelectCommand="SELECT * FROM [Tour]" />
```

**Remediation:** Replace with EF Core repository calls in page model `OnGet()` methods, binding data to Razor view models.

---

### ISSUE-015 [HIGH] – GridView Server Controls (Multiple Pages)
`<asp:GridView>` is a Web Forms server control not available in .NET 8.

**Affected files:**
- DisplayTours.aspx (lines 9–35)
- TourCrud.aspx (lines 8–33)
- allbooking.aspx (lines 8–21)
- mybooking.aspx (lines 8–21)
- usercrud.aspx (lines 8–21)

**Remediation:** Replace with HTML `<table>` with Razor `@foreach` loops or use a modern component library.

---

### ISSUE-016 [HIGH] – ASP.NET Server Controls (TextBox, Button, Label, FileUpload, etc.)
All `<asp:TextBox>`, `<asp:Button>`, `<asp:Label>`, `<asp:FileUpload>`, `<asp:DropDownList>`,
`<asp:RegularExpressionValidator>`, `<asp:HyperLink>` controls are Web Forms-specific and do not exist in .NET 8.

**Affected files:** All 11 .aspx files.

**Remediation:** Replace with standard HTML elements and Tag Helpers (`<input asp-for="">`, `<label asp-for="">`, etc.).

---

### ISSUE-017 [HIGH] – Hardcoded Admin Credentials (AdminLogin2.aspx.cs)
Admin authentication uses hardcoded credentials compared in `Page_Load` (before form submission).

**File:** AdminLogin2.aspx.cs, lines 14–18

**Code snippet:**
```csharp
if (password.Text == "admin" && name.Text == "admin@gmail.com")
{
    Response.Redirect("AdminProfile.aspx");
```

**Remediation:** Implement ASP.NET Core Identity with role-based authorization (`[Authorize(Roles = "Admin")]`).

---

### ISSUE-018 [HIGH] – No Authentication / Session Management
No session management, no authentication cookies, no authorization checks exist anywhere in the application.
After login, there is no mechanism to maintain the authenticated state.

**Affected files:** userlogin.aspx.cs, AdminLogin2.aspx.cs (commented-out Session code visible).

**Code snippet (userlogin.aspx.cs):**
```csharp
//Session["New"] = txtEmail.Text;  // commented out
```

**Remediation:** Implement ASP.NET Core Identity with cookie authentication and `[Authorize]` attributes.

---

### ISSUE-019 [HIGH] – FileUpload Server Control (AddTour.aspx)
`<asp:FileUpload>` is a Web Forms server control not available in .NET 8.

**File:** AddTour.aspx (line 55), AddTour.aspx.cs (line 33)

**Code snippet:**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
```

**Remediation:** Use `IFormFile` in Razor Pages model binding with `IWebHostEnvironment` for path resolution.

---

### ISSUE-020 [HIGH] – packages.config NuGet Format
`packages.config` is the legacy NuGet package management format not used in .NET 8 SDK-style projects.

**File:** packages.config

**Remediation:** Remove `packages.config` and use `<PackageReference>` elements in the SDK-style `.csproj`.

---

### ISSUE-021 [MEDIUM] – Password Displayed in GridView (usercrud.aspx)
The `Password` column is exposed in the user management grid, creating a security vulnerability.

**File:** usercrud.aspx, line 16

**Code snippet:**
```aspx
<asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
```

**Remediation:** Remove password from display. Use hashed passwords with ASP.NET Core Identity.

---

### ISSUE-022 [MEDIUM] – No Connection Pooling / Resource Disposal
`SqlConnection` objects are opened but not wrapped in `using` statements, risking connection leaks.

**Affected files:** AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs, TourCrud.aspx.cs

**Code snippet:**
```csharp
SqlConnection conn = new SqlConnection(...);
conn.Open();
// ... no using statement, conn.Close() may not be reached on exception
```

**Remediation:** EF Core manages connection pooling automatically. Use `using` statements for any remaining ADO.NET code.

---

### ISSUE-023 [MEDIUM] – Unreachable Code After Response.Redirect (Multiple Files)
Code after `Response.Redirect()` (e.g., `conn.Close()`) is unreachable because `Response.Redirect` in
Web Forms throws `ThreadAbortException`.

**Affected files:** Order.aspx.cs (line 32), SignUpForm.aspx.cs (line 36)

**Code snippet:**
```csharp
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");
conn.Close();  // unreachable
```

**Remediation:** Restructure code flow; use `using` for connections; return redirect result directly.

---

### ISSUE-024 [MEDIUM] – Hardcoded Database File Path in Web.config
The connection string contains an absolute local file path.

**File:** Web.config, line 28

**Code snippet:**
```xml
AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf
```

**Remediation:** Use a proper SQL Server connection string in `appsettings.json` with environment-specific overrides.

---

### ISSUE-025 [MEDIUM] – No Input Validation Beyond RegularExpressionValidator
Only one `<asp:RegularExpressionValidator>` exists (AddTour.aspx). No server-side validation framework is used.

**File:** AddTour.aspx (line 68)

**Remediation:** Use FluentValidation 11.x or Data Annotations with `ModelState.IsValid` checks in .NET 8.

---

### ISSUE-026 [MEDIUM] – No Error Handling / Logging
No try-catch blocks, no logging framework, no global error handling exists anywhere in the application.

**Affected files:** All code-behind files.

**Remediation:** Add try-catch with `ILogger<T>` (Microsoft.Extensions.Logging / Serilog.AspNetCore 8.0.0).

---

### ISSUE-027 [LOW] – Designer Files (.aspx.designer.cs)
All 11 pages have `.aspx.designer.cs` files containing auto-generated Web Forms control declarations.
These are not needed in .NET 8.

**Affected files:** All 11 `.aspx.designer.cs` files.

**Remediation:** Delete all `.aspx.designer.cs` files; they have no equivalent in Razor Pages.

---

### ISSUE-028 [LOW] – AssemblyInfo.cs (Properties/AssemblyInfo.cs)
The legacy `AssemblyInfo.cs` file is not needed in SDK-style projects (attributes are auto-generated).

**File:** Properties/AssemblyInfo.cs

**Remediation:** Remove `AssemblyInfo.cs`; configure assembly metadata in the `.csproj` file if needed.

---

### ISSUE-029 [LOW] – Web.Debug.config / Web.Release.config Transform Files
Web.config transform files are not applicable in .NET 8.

**Files:** Web.Debug.config, Web.Release.config

**Remediation:** Use `appsettings.Development.json` and `appsettings.Production.json` for environment-specific configuration.

---

### ISSUE-030 [LOW] – No Async/Await Patterns
All data access operations are synchronous, blocking threads unnecessarily.

**Affected files:** All code-behind files with database operations.

**Remediation:** Use `async Task` methods with `await` for all I/O operations in .NET 8 (EF Core provides async APIs).

---

## 3. Migration Roadmap

### Phase 1 – Foundation (Week 1–2)
1. Create new SDK-style solution with clean architecture layers:
   - `Tour_Management.Domain`
   - `Tour_Management.Application`
   - `Tour_Management.Infrastructure`
   - `Tour_Management.Web`
2. Create `appsettings.json` with proper connection string
3. Set up EF Core 8.0.0 with `TourManagementDbContext`
4. Create domain entities: `Tour`, `UserInfo`, `Booking`
5. Create repository interfaces and implementations

### Phase 2 – Authentication (Week 2–3)
1. Implement ASP.NET Core Identity
2. Create user registration (replaces SignUpForm.aspx)
3. Create user login with cookie authentication (replaces userlogin.aspx)
4. Create admin login with role-based authorization (replaces AdminLogin2.aspx)
5. Add `[Authorize]` attributes to protected pages

### Phase 3 – Core Pages (Week 3–4)
1. Migrate DisplayTours.aspx → `Pages/Tours/Index.cshtml`
2. Migrate AddTour.aspx → `Pages/Tours/Create.cshtml`
3. Migrate TourCrud.aspx → `Pages/Tours/Manage.cshtml`
4. Migrate Order.aspx → `Pages/Bookings/Create.cshtml`
5. Migrate allbooking.aspx → `Pages/Bookings/Index.cshtml`
6. Migrate mybooking.aspx → `Pages/Bookings/MyBookings.cshtml`

### Phase 4 – Profile Pages (Week 4–5)
1. Migrate MainProfilePage.aspx → `Pages/Profile/Index.cshtml`
2. Migrate AdminProfile.aspx → `Pages/Admin/Index.cshtml`
3. Migrate usercrud.aspx → `Pages/Admin/Users.cshtml`
4. Implement file upload with `IFormFile`

### Phase 5 – Quality & Testing (Week 5–6)
1. Add FluentValidation for all input models
2. Add comprehensive error handling and logging (Serilog)
3. Write unit tests for services
4. Write integration tests for repositories
5. Security review (remove plain-text passwords, SQL injection fixes)

---

## 4. Target Architecture

```
Tour_Management/
├── src/
│   ├── Tour_Management.Domain/
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
│   ├── Tour_Management.Application/
│   │   ├── Services/
│   │   ├── DTOs/
│   │   ├── Mappings/
│   │   └── Validators/
│   ├── Tour_Management.Infrastructure/
│   │   ├── Data/
│   │   │   ├── TourManagementDbContext.cs
│   │   │   └── Configurations/
│   │   └── Repositories/
│   └── Tour_Management.Web/
│       ├── Pages/
│       │   ├── Tours/
│       │   ├── Bookings/
│       │   ├── Admin/
│       │   └── Profile/
│       ├── ViewModels/
│       ├── wwwroot/
│       └── Program.cs
└── tests/
    ├── Tour_Management.UnitTests/
    └── Tour_Management.IntegrationTests/
```

---

## 5. Key Code Migration Examples

### 5.1 AddTour.aspx.cs → Pages/Tours/Create.cshtml.cs

**Before (Web Forms):**
```csharp
protected void Register_Click(object sender, EventArgs e)
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
    conn.Open();
    string insertQuery = "insert into Tour(TOUR_NAME,...) values(@TOUR_NAME,...)";
    SqlCommand com = new SqlCommand(insertQuery, conn);
    com.Parameters.AddWithValue("@TOUR_NAME", tour_name.Text);
    FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
    com.ExecuteNonQuery();
    Response.Write("ADD Successful");
}
```

**After (.NET 8 Razor Pages):**
```csharp
public class CreateModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<CreateModel> _logger;

    [BindProperty] public TourCreateViewModel Input { get; set; } = new();

    public CreateModel(ITourService tourService, IWebHostEnvironment env, ILogger<CreateModel> logger)
    {
        _tourService = tourService;
        _env = env;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();
        try
        {
            string? picFileName = null;
            if (Input.PicFile != null)
            {
                var uploadsPath = Path.Combine(_env.WebRootPath, "Tour_pics");
                Directory.CreateDirectory(uploadsPath);
                picFileName = Path.GetFileName(Input.PicFile.FileName);
                using var stream = System.IO.File.Create(Path.Combine(uploadsPath, picFileName));
                await Input.PicFile.CopyToAsync(stream, ct);
            }
            var dto = new TourCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                Pic = picFileName
            };
            await _tourService.CreateAsync(dto, ct);
            TempData["Success"] = "Tour added successfully.";
            return RedirectToPage("/Tours/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            ModelState.AddModelError(string.Empty, "An error occurred while saving the tour.");
            return Page();
        }
    }
}
```

### 5.2 userlogin.aspx.cs → Pages/Account/Login.cshtml.cs

**Before (Web Forms – SQL Injection vulnerable):**
```csharp
string checkPasswordQuery = "select password from Userinfo where password='" + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
```

**After (.NET 8 with ASP.NET Core Identity):**
```csharp
public async Task<IActionResult> OnPostAsync(CancellationToken ct)
{
    if (!ModelState.IsValid) return Page();
    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: true);
    if (result.Succeeded)
        return RedirectToPage("/Profile/Index");
    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    return Page();
}
```

### 5.3 Web.config → appsettings.json

**Before:**
```xml
<connectionStrings>
  <add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gajer\..." />
</connectionStrings>
```

**After:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagement;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## 6. Package Migration Table

| Legacy Package | Version | Status | .NET 8 Replacement |
|----------------|---------|--------|--------------------|
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 2.0.1 | ❌ Remove | Not needed in SDK-style projects |
| System.Web (framework) | 4.7.2 | ❌ Remove | Microsoft.AspNetCore 8.0.0 |
| System.Data.SqlClient (framework) | 4.7.2 | ❌ Remove | Microsoft.EntityFrameworkCore.SqlServer 8.0.0 |
| System.Configuration (framework) | 4.7.2 | ❌ Remove | Microsoft.Extensions.Configuration 8.0.0 |
| System.Web.DataVisualization | 4.0.0 | ❌ Remove | Chart.js (JavaScript) |
| — | — | ✅ Add | Microsoft.EntityFrameworkCore 8.0.0 |
| — | — | ✅ Add | Microsoft.EntityFrameworkCore.SqlServer 8.0.0 |
| — | — | ✅ Add | Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.0 |
| — | — | ✅ Add | AutoMapper 12.0.1 |
| — | — | ✅ Add | FluentValidation 11.9.0 |
| — | — | ✅ Add | Serilog.AspNetCore 8.0.0 |

---

*Generated by ASP.NET Web Forms to .NET 8 Migration Analyzer*
