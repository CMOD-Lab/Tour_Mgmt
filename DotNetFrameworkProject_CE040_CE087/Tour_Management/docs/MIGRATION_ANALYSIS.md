# Tour_Management – ASP.NET Web Forms to .NET 8 Migration Analysis

**Analysis Date:** 2025-01-30  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Module:** Tour_Management  

---

## Executive Summary

| Severity | Count |
|----------|-------|
| Critical | 12    |
| High     | 10    |
| Medium   | 8     |
| Low      | 4     |
| **Total**| **34**|

- **Migration Complexity:** Complex  
- **Estimated Remediation Effort:** 80–120 hours  
- **Compatibility Score:** 18 / 100  
- **Deprecated APIs Found:** 18  
- **Breaking Changes:** 22  

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
| usercrud.aspx / .cs | Web Forms Page | Medium |
| userlogin.aspx / .cs | Web Forms Page | Medium |
| Web.config | Configuration | N/A |
| Tour_Management.csproj | Project File | N/A |
| packages.config | Package Config | N/A |

**No master pages (.master) found.**  
**No user controls (.ascx) found.**  
**No Global.asax found.**

---

## 2. Detailed Issue Findings

### CRITICAL Issues

#### ISSUE-001: System.Web.UI.Page Inheritance (All Code-Behind Files)
All 12 code-behind files inherit from `System.Web.UI.Page`, which does not exist in .NET 8.

**Affected Files:**
- AddTour.aspx.cs (line 12)
- AdminLogin2.aspx.cs (line 11)
- AdminProfile.aspx.cs (line 11)
- allbooking.aspx.cs (line 11)
- DisplayTours.aspx.cs (line 12)
- MainProfilePage.aspx.cs (line 11)
- mybooking.aspx.cs (line 11)
- Order.aspx.cs (line 12)
- SignUpForm.aspx.cs (line 12)
- TourCrud.aspx.cs (line 12)
- usercrud.aspx.cs (line 11)
- userlogin.aspx.cs (line 12)

**Code Snippet:**
```csharp
public partial class AddTour : System.Web.UI.Page
```

**Remediation:** Replace with Razor Pages (`PageModel`) or MVC Controllers. Each `.aspx` page maps to a `.cshtml` Razor Page with a corresponding `PageModel` class.

---

#### ISSUE-002: System.Web Namespace References (All Code-Behind Files)
All code-behind files import `System.Web`, `System.Web.UI`, and `System.Web.UI.WebControls` which are not available in .NET 8.

**Affected Files:** All 12 .aspx.cs files  
**Code Snippet:**
```csharp
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
```

**Remediation:** Remove all `System.Web.*` using statements. Use `Microsoft.AspNetCore.*` equivalents.

---

#### ISSUE-003: ADO.NET SqlConnection with ConfigurationManager (Multiple Files)
Direct ADO.NET usage with `ConfigurationManager.ConnectionStrings` is a .NET Framework pattern. `ConfigurationManager` is not available by default in .NET 8 web projects.

**Affected Files:**
- AddTour.aspx.cs (lines 20–22)
- Order.aspx.cs (lines 18–20)
- SignUpForm.aspx.cs (lines 20–22)
- TourCrud.aspx.cs (lines 22–24)
- userlogin.aspx.cs (lines 22–24)
- DisplayTours.aspx.cs (lines 10–11)

**Code Snippet:**
```csharp
SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
```

**Remediation:** Replace with EF Core 8 `DbContext` and inject `IConfiguration` for connection strings from `appsettings.json`.

---

#### ISSUE-004: Web.config Configuration File
`Web.config` is not supported in .NET 8. All configuration must be migrated to `appsettings.json`.

**File:** Web.config  
**Code Snippet:**
```xml
<connectionStrings>
  <add name="dbconnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;..." />
</connectionStrings>
<compilation debug="true" targetFramework="4.7.2" />
<httpRuntime targetFramework="4.7.2"/>
```

**Remediation:** Create `appsettings.json` with connection strings and app settings. Configure in `Program.cs`.

---

#### ISSUE-005: System.Web.DataVisualization (allbooking.aspx)
The `System.Web.DataVisualization` assembly (Chart controls) is a .NET Framework-only component.

**File:** allbooking.aspx (line 3), Web.config (lines 4–18), Tour_Management.csproj  
**Code Snippet:**
```aspx
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, ..." %>
```

**Remediation:** Replace with a modern charting library such as Chart.js (client-side) or a .NET 8 compatible NuGet package.

---

#### ISSUE-006: Non-SDK-Style Project File
The `.csproj` uses the old MSBuild format with `ProjectTypeGuids` for Web Application projects, which is incompatible with .NET 8.

**File:** Tour_Management.csproj (lines 1–5)  
**Code Snippet:**
```xml
<Project ToolsVersion="15.0" DefaultTargets="Build" xmlns="...">
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

#### ISSUE-007: packages.config NuGet Format
`packages.config` is the legacy NuGet format and is not supported in SDK-style .NET 8 projects.

**File:** packages.config  
**Code Snippet:**
```xml
<package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />
```

**Remediation:** Migrate to `<PackageReference>` elements in the `.csproj` file.

---

#### ISSUE-008: Hardcoded Credentials in Source Code (AdminLogin2.aspx.cs)
Admin credentials are hardcoded directly in the source code — a critical security vulnerability.

**File:** AdminLogin2.aspx.cs (lines 14–17)  
**Code Snippet:**
```csharp
if (password.Text == "admin" && name.Text == "admin@gmail.com")
{
    Response.Redirect("AdminProfile.aspx");
}
```

**Remediation:** Implement ASP.NET Core Identity with proper authentication. Store credentials securely using hashed passwords.

---

#### ISSUE-009: SQL Injection Vulnerability (userlogin.aspx.cs)
Raw string concatenation is used to build SQL queries, creating a critical SQL injection vulnerability.

**File:** userlogin.aspx.cs (lines 26–27)  
**Code Snippet:**
```csharp
string checkPasswordQuery = "select password from Userinfo where password='" + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
```

**Remediation:** Use parameterized queries (EF Core handles this automatically) or at minimum use `SqlParameter`.

---

#### ISSUE-010: Plaintext Password Storage (SignUpForm.aspx.cs)
User passwords are stored in plaintext in the database — a critical security vulnerability.

**File:** SignUpForm.aspx.cs (line 29)  
**Code Snippet:**
```csharp
com.Parameters.AddWithValue("@Password", password1.Text);
```

**Remediation:** Use ASP.NET Core Identity's `IPasswordHasher<T>` to hash passwords before storage.

---

#### ISSUE-011: Server.MapPath Usage (AddTour.aspx.cs)
`Server.MapPath()` is a `System.Web` API not available in .NET 8.

**File:** AddTour.aspx.cs (line 33)  
**Code Snippet:**
```csharp
FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
```

**Remediation:** Use `IWebHostEnvironment.WebRootPath` or `IWebHostEnvironment.ContentRootPath` injected via DI.

---

#### ISSUE-012: Response.Write for User Feedback (Multiple Files)
`Response.Write()` is a `System.Web` API not available in .NET 8 and is an anti-pattern for user feedback.

**Affected Files:**
- AddTour.aspx.cs (line 37)
- Order.aspx.cs (line 28)
- SignUpForm.aspx.cs (line 35)
- userlogin.aspx.cs (line 37)

**Code Snippet:**
```csharp
Response.Write("Registration Successful");
```

**Remediation:** Use `TempData` for flash messages or `ModelState` for validation errors in Razor Pages.

---

### HIGH Issues

#### ISSUE-013: Response.Redirect + Server.Transfer Anti-Pattern (Multiple Files)
Both `Response.Redirect()` and `Server.Transfer()` are called sequentially, which is incorrect. `Server.Transfer()` is also not available in .NET 8.

**Affected Files:**
- AdminLogin2.aspx.cs (lines 15–16)
- Order.aspx.cs (lines 29–30)
- SignUpForm.aspx.cs (lines 36–37)
- userlogin.aspx.cs (lines 38–39, 47–48)

**Code Snippet:**
```csharp
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");  // Dead code - never reached
```

**Remediation:** Use `RedirectToPage()` in Razor Pages or `Redirect()` in MVC controllers.

---

#### ISSUE-014: SqlDataSource Server Controls (DisplayTours.aspx, TourCrud.aspx, allbooking.aspx, mybooking.aspx, usercrud.aspx)
`asp:SqlDataSource` is a Web Forms-only server control that does not exist in .NET 8.

**Affected Files:**
- DisplayTours.aspx (line 12)
- TourCrud.aspx (lines 42–46)
- allbooking.aspx (line 22)
- mybooking.aspx (line 22)
- usercrud.aspx (lines 18–22)

**Code Snippet:**
```aspx
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbconnection %>" SelectCommand="SELECT * FROM [Tour]" />
```

**Remediation:** Replace with EF Core repository pattern. Bind data in Razor Page `OnGet()` handlers.

---

#### ISSUE-015: GridView Server Controls (Multiple Pages)
`asp:GridView` is a Web Forms-only server control not available in .NET 8.

**Affected Files:**
- DisplayTours.aspx (line 13)
- TourCrud.aspx (line 9)
- allbooking.aspx (line 9)
- mybooking.aspx (line 9)
- usercrud.aspx (line 9)

**Remediation:** Replace with HTML `<table>` with Razor `@foreach` loops or use a modern component library.

---

#### ISSUE-016: FileUpload Server Control (AddTour.aspx)
`asp:FileUpload` is a Web Forms-only server control.

**File:** AddTour.aspx (line 55)  
**Code Snippet:**
```aspx
<asp:FileUpload ID="FileUpload1" runat="server"/>
```

**Remediation:** Use `<input type="file">` with `IFormFile` in Razor Pages.

---

#### ISSUE-017: RegularExpressionValidator Server Control (AddTour.aspx)
`asp:RegularExpressionValidator` is a Web Forms-only server control.

**File:** AddTour.aspx (line 72)  
**Code Snippet:**
```aspx
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tour_info" ValidationExpression="^[\s\S]{0,250}$" runat="server" />
```

**Remediation:** Use Data Annotations (`[MaxLength(250)]`) and FluentValidation in .NET 8.

---

#### ISSUE-018: HyperLink Server Control (DisplayTours.aspx)
`asp:HyperLink` is a Web Forms-only server control.

**File:** DisplayTours.aspx (line 28)  
**Code Snippet:**
```aspx
<asp:HyperLink ID="HyperLink1" href="Order.aspx" runat="server">Book Now</asp:HyperLink>
```

**Remediation:** Replace with standard HTML `<a>` tag with Razor `asp-page` tag helper.

---

#### ISSUE-019: Page_Load Event Handler Pattern (All Code-Behind Files)
The `Page_Load` event handler is a Web Forms lifecycle event that does not exist in .NET 8.

**Affected Files:** All 12 .aspx.cs files  
**Code Snippet:**
```csharp
protected void Page_Load(object sender, EventArgs e) { }
```

**Remediation:** Replace with `OnGet()` / `OnPost()` methods in Razor Page `PageModel` classes.

---

#### ISSUE-020: Button Click Event Handlers (Multiple Files)
Server-side button click event handlers (`OnClick="Register_Click"`) are Web Forms postback patterns.

**Affected Files:**
- AddTour.aspx / .cs
- Order.aspx / .cs
- SignUpForm.aspx / .cs
- userlogin.aspx / .cs

**Code Snippet:**
```aspx
<asp:Button ID="Register" runat="server" OnClick="Register_Click" />
```

**Remediation:** Replace with HTML `<button type="submit">` and handle in `OnPost()` Razor Page handler.

---

#### ISSUE-021: asp:Label, asp:TextBox, asp:Button Server Controls (All Pages)
All Web Forms server controls (`asp:Label`, `asp:TextBox`, `asp:Button`, `asp:DropDownList`) are not available in .NET 8.

**Affected Files:** All .aspx files  
**Remediation:** Replace with standard HTML elements and Razor Tag Helpers (`<label asp-for>`, `<input asp-for>`, `<select asp-for>`).

---

#### ISSUE-022: DropDownList Server Control (SignUpForm.aspx)
`asp:DropDownList` is a Web Forms-only server control.

**File:** SignUpForm.aspx (lines 44–49)  
**Code Snippet:**
```aspx
<asp:DropDownList ID="gender" runat="server">
    <asp:ListItem Text="Male"></asp:ListItem>
    <asp:ListItem Text="Female"></asp:ListItem>
</asp:DropDownList>
```

**Remediation:** Replace with `<select asp-for="Gender">` with `<option>` elements.

---

### MEDIUM Issues

#### ISSUE-023: Web.config Connection String with Absolute Path
The connection string contains an absolute local file path that is environment-specific.

**File:** Web.config (line 28)  
**Code Snippet:**
```xml
AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf
```

**Remediation:** Use a proper SQL Server connection string in `appsettings.json` with environment-specific overrides.

---

#### ISSUE-024: System.Web.DataVisualization Chart Handler in Web.config
The Chart HTTP handler configuration is .NET Framework-specific.

**File:** Web.config (lines 4–18)  
**Remediation:** Remove entirely. Use client-side charting (Chart.js) in .NET 8.

---

#### ISSUE-025: Microsoft.CodeDom.Providers.DotNetCompilerPlatform Package
This package is for Roslyn compiler support in .NET Framework and is not needed in .NET 8.

**File:** packages.config (line 2), Tour_Management.csproj  
**Remediation:** Remove this package reference entirely. .NET 8 uses Roslyn natively.

---

#### ISSUE-026: Page.IsPostBack Pattern (TourCrud.aspx.cs)
`Page.IsPostBack` is a Web Forms-specific property.

**File:** TourCrud.aspx.cs (line 14)  
**Code Snippet:**
```csharp
if (!Page.IsPostBack) { refreshdata(); }
```

**Remediation:** In Razor Pages, `OnGet()` is only called on GET requests by default. No equivalent check needed.

---

#### ISSUE-027: Inline Data Binding Syntax in ASPX (DisplayTours.aspx, TourCrud.aspx)
Web Forms data binding syntax `<%#Eval("pic") %>` is not available in .NET 8.

**Affected Files:**
- DisplayTours.aspx (line 22)
- TourCrud.aspx (line 22)

**Code Snippet:**
```aspx
<img src="Tour_pics/<%#Eval("pic") %>" style="width:200px;height:200px" />
```

**Remediation:** Use Razor syntax `@Model.Pic` or `@item.Pic` in a `@foreach` loop.

---

#### ISSUE-028: ConnectionStrings Expression Syntax in ASPX
`<%$ ConnectionStrings:dbconnection %>` is a Web Forms expression syntax not available in .NET 8.

**Affected Files:**
- DisplayTours.aspx (line 12)
- TourCrud.aspx (line 43)
- allbooking.aspx (line 22)
- mybooking.aspx (line 22)
- usercrud.aspx (line 18)

**Remediation:** Remove `SqlDataSource` controls entirely. Use EF Core with injected `DbContext`.

---

#### ISSUE-029: runat="server" Attribute on HTML Elements
The `runat="server"` attribute is a Web Forms-specific mechanism for server-side control.

**Affected Files:** All .aspx files  
**Remediation:** Remove `runat="server"` from all HTML elements. Use Razor syntax for server-side rendering.

---

#### ISSUE-030: No Authentication/Authorization Mechanism
The application has no proper authentication. Admin login uses hardcoded credentials; user login has SQL injection vulnerability. No session management exists.

**Affected Files:** AdminLogin2.aspx.cs, userlogin.aspx.cs  
**Remediation:** Implement ASP.NET Core Identity with cookie authentication. Add `[Authorize]` attributes to protected pages.

---

### LOW Issues

#### ISSUE-031: Dead Code After Response.Redirect (Multiple Files)
Code after `Response.Redirect()` is unreachable because `Response.Redirect()` throws `ThreadAbortException` in .NET Framework (or terminates response in .NET Core).

**Affected Files:**
- Order.aspx.cs (line 30)
- SignUpForm.aspx.cs (line 37)
- userlogin.aspx.cs (lines 39, 48)

**Code Snippet:**
```csharp
Response.Redirect("mybooking.aspx");
Server.Transfer("mybooking.aspx");  // Dead code
conn.Close();                        // Dead code
```

**Remediation:** Remove dead code. Use `return RedirectToPage()` in Razor Pages.

---

#### ISSUE-032: No Connection Disposal / Using Statements (Multiple Files)
`SqlConnection` objects are not wrapped in `using` statements, risking connection leaks.

**Affected Files:**
- AddTour.aspx.cs
- Order.aspx.cs
- SignUpForm.aspx.cs
- userlogin.aspx.cs

**Code Snippet:**
```csharp
SqlConnection conn = new SqlConnection(...);
conn.Open();
// ... no using statement, conn.Close() may not be reached
```

**Remediation:** Use EF Core which manages connections automatically, or wrap in `using` statements.

---

#### ISSUE-033: Commented-Out Code (TourCrud.aspx.cs)
Large blocks of commented-out code exist in TourCrud.aspx.cs.

**File:** TourCrud.aspx.cs (lines 28–40)  
**Remediation:** Remove commented-out code during migration.

---

#### ISSUE-034: No Error Handling in Data Access Methods
No try-catch blocks exist in any data access code.

**Affected Files:** All code-behind files with database operations  
**Remediation:** Implement proper exception handling with logging using `ILogger<T>`.

---

## 3. Migration Roadmap

### Phase 1: Project Setup (8–12 hours)
1. Create new .NET 8 solution with clean architecture (Domain, Application, Infrastructure, Web)
2. Set up SDK-style `.csproj` files
3. Create `appsettings.json` with connection strings
4. Configure `Program.cs` with DI, middleware, and routing

### Phase 2: Domain & Infrastructure Layer (16–20 hours)
1. Create domain entities: `Tour`, `UserInfo`, `Booking`
2. Create EF Core `DbContext` with entity configurations
3. Implement repository interfaces and implementations
4. Set up database migrations

### Phase 3: Application Layer (12–16 hours)
1. Create DTOs for all entities
2. Implement service classes with business logic
3. Configure AutoMapper profiles
4. Add FluentValidation validators

### Phase 4: Web Layer – Authentication (8–12 hours)
1. Implement ASP.NET Core Identity
2. Create Login/Register Razor Pages
3. Add authorization policies
4. Secure admin pages with `[Authorize(Roles = "Admin")]`

### Phase 5: Web Layer – Feature Pages (24–32 hours)
1. Migrate all 12 .aspx pages to Razor Pages
2. Implement file upload with `IFormFile`
3. Replace GridView with HTML tables + Razor
4. Add TempData for success/error messages

### Phase 6: Testing & Documentation (12–16 hours)
1. Write unit tests for services
2. Write integration tests for repositories
3. Create migration documentation
4. Build verification

---

## 4. Architecture Mapping

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| AddTour.aspx | Pages/Tours/Create.cshtml |
| AdminLogin2.aspx | Pages/Admin/Login.cshtml |
| AdminProfile.aspx | Pages/Admin/Index.cshtml |
| allbooking.aspx | Pages/Admin/Bookings.cshtml |
| DisplayTours.aspx | Pages/Tours/Index.cshtml |
| MainProfilePage.aspx | Pages/Index.cshtml |
| mybooking.aspx | Pages/Bookings/MyBookings.cshtml |
| Order.aspx | Pages/Bookings/Create.cshtml |
| SignUpForm.aspx | Pages/Account/Register.cshtml |
| TourCrud.aspx | Pages/Admin/Tours.cshtml |
| usercrud.aspx | Pages/Admin/Users.cshtml |
| userlogin.aspx | Pages/Account/Login.cshtml |
| Web.config | appsettings.json |
| packages.config | PackageReference in .csproj |
| System.Web.UI.Page | Microsoft.AspNetCore.Mvc.RazorPages.PageModel |
| SqlDataSource | EF Core DbContext + Repository |
| GridView | HTML table + @foreach |
| asp:Button | `<button type="submit">` |
| asp:TextBox | `<input asp-for>` |
| asp:Label | `<label asp-for>` |
| Server.MapPath | IWebHostEnvironment.WebRootPath |
| ConfigurationManager | IConfiguration |
| Response.Redirect | RedirectToPage() |
| Page_Load | OnGet() |
| Button_Click | OnPost() |
