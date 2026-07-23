# ASP.NET Web Forms to .NET 8 Migration Analysis Report
## Module: Tour_Management
**Analysis Date:** 2025-01-30  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Migration Complexity:** Complex  

---

## Executive Summary

The **Tour_Management** application is a classic ASP.NET Web Forms project targeting .NET Framework 4.7.2. It consists of **10 Web Forms pages** with code-behind files, uses **raw ADO.NET** for all data access, and has **no master pages or user controls**. The application manages tour packages, user registration/login, and booking operations.

**Total Issues Found: 47**
- Critical: 12
- High: 16
- Medium: 12
- Low: 7

**Estimated Remediation Effort:** 60–80 hours  
**Compatibility Score:** 18/100 (Very Low — full rewrite required)

---

## Component Inventory

### Web Forms Pages (10 total)
| Page | Code-Behind | Complexity | Description |
|------|-------------|------------|-------------|
| AddTour.aspx | AddTour.aspx.cs | Medium | Admin form to add new tour with file upload |
| AdminLogin2.aspx | AdminLogin2.aspx.cs | Simple | Hardcoded admin login |
| AdminProfile.aspx | AdminProfile.aspx.cs | Simple | Admin dashboard/home page |
| DisplayTours.aspx | DisplayTours.aspx.cs | Medium | GridView with SqlDataSource for tours |
| TourCrud.aspx | TourCrud.aspx.cs | Medium | Admin GridView CRUD for tours |
| Order.aspx | Order.aspx.cs | Medium | Tour booking form |
| SignUpForm.aspx | SignUpForm.aspx.cs | Medium | User registration with ADO.NET |
| userlogin.aspx | userlogin.aspx.cs | Medium | User login with SQL injection vulnerability |
| usercrud.aspx | usercrud.aspx.cs | Medium | User management GridView |
| MainProfilePage.aspx | MainProfilePage.aspx.cs | Simple | User home/dashboard page |
| mybooking.aspx | mybooking.aspx.cs | Simple | User's current bookings |
| allbooking.aspx | allbooking.aspx.cs | Simple | Admin view of all bookings |

### User Controls: 0
### Master Pages: 0
### Global.asax: Not present
### packages.config: 1 package (Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1)

---

## Detailed Issue Findings

### CRITICAL Issues

#### ISSUE-001: System.Web Namespace — Not Available in .NET 8
- **Files:** All .aspx.cs code-behind files (10 files)
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** Every code-behind file imports `System.Web`, `System.Web.UI`, and `System.Web.UI.WebControls`. These namespaces do not exist in .NET 8. The entire Web Forms runtime (`System.Web.dll`) is a .NET Framework-only assembly and has no equivalent in .NET 8.
- **Code Snippet:**
  ```csharp
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  ```
- **Recommendation:** Replace with ASP.NET Core equivalents:
  - `System.Web.UI.Page` → Razor Page (`PageModel`)
  - `System.Web.UI.WebControls` → HTML Tag Helpers / Razor syntax
  - `System.Web.HttpResponse` → `Microsoft.AspNetCore.Http.HttpResponse`
  - `System.Web.HttpRequest` → `Microsoft.AspNetCore.Http.HttpRequest`
- **Effort:** High

#### ISSUE-002: Web Forms Page Lifecycle — Not Supported in .NET 8
- **Files:** All .aspx.cs files
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** All pages inherit from `System.Web.UI.Page` and use the Web Forms page lifecycle (`Page_Load`, `Page_PreRender`, etc.). This lifecycle does not exist in .NET 8.
- **Code Snippet (AddTour.aspx.cs, line 14):**
  ```csharp
  public partial class AddTour : System.Web.UI.Page
  {
      protected void Page_Load(object sender, EventArgs e) { }
  }
  ```
- **Recommendation:** Migrate each page to a Razor Page (`PageModel`) with `OnGet()` / `OnPost()` handlers.
- **Effort:** High

#### ISSUE-003: SQL Injection Vulnerability in userlogin.aspx.cs
- **File:** userlogin.aspx.cs
- **Line:** 28
- **Severity:** Critical
- **Breaking Change:** No (security issue)
- **Description:** The login query directly concatenates user input into the SQL string, creating a critical SQL injection vulnerability.
- **Code Snippet:**
  ```csharp
  string checkPasswordQuery = "select password from Userinfo where password='" 
      + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";
  ```
- **Recommendation:** Use parameterized queries (already done in other files) or migrate to EF Core with Identity. Never concatenate user input into SQL strings.
- **Effort:** Low

#### ISSUE-004: Plaintext Password Storage
- **File:** SignUpForm.aspx.cs, userlogin.aspx.cs, usercrud.aspx
- **Line:** SignUpForm.aspx.cs line 24; usercrud.aspx line 14
- **Severity:** Critical
- **Breaking Change:** No (security issue)
- **Description:** Passwords are stored in plaintext in the database (`@Password` parameter directly from `password1.Text`). The `usercrud.aspx` GridView even displays the `Password` column directly.
- **Code Snippet (SignUpForm.aspx.cs, line 24):**
  ```csharp
  com.Parameters.AddWithValue("@Password", password1.Text);
  ```
- **Recommendation:** Use ASP.NET Core Identity with `PasswordHasher<T>` or BCrypt. Never store plaintext passwords.
- **Effort:** High

#### ISSUE-005: Hardcoded Admin Credentials
- **File:** AdminLogin2.aspx.cs
- **Line:** 14–17
- **Severity:** Critical
- **Breaking Change:** No (security issue)
- **Description:** Admin authentication is performed by comparing hardcoded credentials in the code. No database lookup, no hashing.
- **Code Snippet:**
  ```csharp
  if (password.Text == "admin" && name.Text == "admin@gmail.com")
  {
      Response.Redirect("AdminProfile.aspx");
  }
  ```
- **Recommendation:** Implement proper role-based authentication using ASP.NET Core Identity with admin role claims.
- **Effort:** High

#### ISSUE-006: Response.Redirect + Server.Transfer Called Together
- **Files:** Order.aspx.cs (lines 22–23), userlogin.aspx.cs (lines 36–37), SignUpForm.aspx.cs (lines 28–29), Btn_reg handler
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** Multiple methods call both `Response.Redirect()` and `Server.Transfer()` on the same code path. `Response.Redirect()` throws a `ThreadAbortException` in .NET Framework (ending execution), so `Server.Transfer()` is never reached. Both APIs are unavailable in .NET 8.
- **Code Snippet (Order.aspx.cs, lines 22–23):**
  ```csharp
  Response.Redirect("mybooking.aspx");
  Server.Transfer("mybooking.aspx");  // Dead code — never executes
  ```
- **Recommendation:** Replace with `return RedirectToPage("/mybooking")` in Razor Pages. Remove all `Server.Transfer()` calls.
- **Effort:** Low

#### ISSUE-007: SqlDataSource Server Control — Not Available in .NET 8
- **Files:** DisplayTours.aspx, TourCrud.aspx, mybooking.aspx, allbooking.aspx, usercrud.aspx
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** `<asp:SqlDataSource>` is a Web Forms server control that does not exist in .NET 8. It provides declarative data binding directly in markup.
- **Code Snippet (DisplayTours.aspx, line 10):**
  ```xml
  <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
      ConnectionString="<%$ ConnectionStrings:dbconnection %>" 
      SelectCommand="SELECT [TOUR_NAME], [pic], [PRICE], [DAYS], [LOCATIONS], [TOUR_ID] FROM [Tour]">
  </asp:SqlDataSource>
  ```
- **Recommendation:** Replace with EF Core repository pattern. Bind data in `OnGet()` PageModel handler and use Razor `@foreach` loops or Tag Helpers for rendering.
- **Effort:** High

#### ISSUE-008: GridView Server Control — Not Available in .NET 8
- **Files:** DisplayTours.aspx, TourCrud.aspx, mybooking.aspx, allbooking.aspx, usercrud.aspx
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** `<asp:GridView>` with `AutoGenerateEditButton`, `AutoGenerateDeleteButton`, and `DataSourceID` binding is a Web Forms-only server control.
- **Code Snippet (TourCrud.aspx, line 9):**
  ```xml
  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
      AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" 
      DataKeyNames="TOUR_ID" DataSourceID="SqlDataSource1">
  ```
- **Recommendation:** Replace with Razor Pages table rendering using `@foreach` with Bootstrap table styling. Implement edit/delete as separate Razor Pages or AJAX calls.
- **Effort:** High

#### ISSUE-009: FileUpload Server Control + Server.MapPath — Not Available in .NET 8
- **File:** AddTour.aspx, AddTour.aspx.cs
- **Line:** AddTour.aspx.cs line 22
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** `<asp:FileUpload>` server control and `Server.MapPath()` are Web Forms-only APIs.
- **Code Snippet (AddTour.aspx.cs, line 22):**
  ```csharp
  FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
  ```
- **Recommendation:** Use `IFormFile` in Razor Pages with `IWebHostEnvironment.WebRootPath` for path resolution.
  ```csharp
  // In PageModel:
  public async Task<IActionResult> OnPostAsync(IFormFile file)
  {
      var path = Path.Combine(_env.WebRootPath, "Tour_pics", file.FileName);
      await using var stream = new FileStream(path, FileMode.Create);
      await file.CopyToAsync(stream);
  }
  ```
- **Effort:** Medium

#### ISSUE-010: System.Web.DataVisualization Charting — Not Available in .NET 8
- **Files:** Web.config, allbooking.aspx, Tour_Management.csproj
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** `System.Web.DataVisualization` (Chart controls) is a .NET Framework-only assembly. It is registered in Web.config and referenced in allbooking.aspx.
- **Code Snippet (Web.config, line 8):**
  ```xml
  <add name="ChartImageHandler" ... 
      type="System.Web.UI.DataVisualization.Charting.ChartHttpHandler, 
      System.Web.DataVisualization, Version=4.0.0.0, ..."/>
  ```
- **Recommendation:** Replace with a modern charting library such as Chart.js (client-side) or use the `LiveCharts2` NuGet package for .NET 8.
- **Effort:** High

#### ISSUE-011: Web.config — Not Supported in .NET 8 ASP.NET Core
- **File:** Web.config
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** The entire `Web.config` configuration system (`<system.web>`, `<system.webServer>`, `<connectionStrings>`, `<appSettings>`, `<system.codedom>`) is .NET Framework-specific and not used by ASP.NET Core.
- **Recommendation:** Migrate to `appsettings.json`:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=...;Database=tourdb;..."
    },
    "AppSettings": {
      "ValidationSettings:UnobtrusiveValidationMode": "None"
    }
  }
  ```
  Configure in `Program.cs` using `builder.Configuration`.
- **Effort:** Medium

#### ISSUE-012: Legacy .csproj Format — Must Be Replaced with SDK-Style
- **File:** Tour_Management.csproj
- **Severity:** Critical
- **Breaking Change:** Yes
- **Description:** The project file uses the legacy MSBuild format with `ProjectTypeGuids` for Web Application (`{349c5851-65df-11da-9384-00065b846f21}`), explicit file listings, and `TargetFrameworkVersion v4.7.2`. This format is incompatible with .NET 8.
- **Code Snippet (Tour_Management.csproj, line 10):**
  ```xml
  <ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>
  <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  ```
- **Recommendation:** Replace with SDK-style project file:
  ```xml
  <Project Sdk="Microsoft.NET.Sdk.Web">
    <PropertyGroup>
      <TargetFramework>net8.0</TargetFramework>
      <Nullable>enable</Nullable>
      <ImplicitUsings>enable</ImplicitUsings>
    </PropertyGroup>
  </Project>
  ```
- **Effort:** Medium

---

### HIGH Issues

#### ISSUE-013: Raw ADO.NET SqlConnection — Should Be Replaced with EF Core
- **Files:** AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs, TourCrud.aspx.cs, DisplayTours.aspx.cs
- **Severity:** High
- **Breaking Change:** No (ADO.NET works in .NET 8, but is not recommended)
- **Description:** All data access uses raw `SqlConnection`, `SqlCommand`, and `SqlDataAdapter`. While `System.Data.SqlClient` is available in .NET 8 via the `Microsoft.Data.SqlClient` NuGet package, the migration rules require EF Core.
- **Code Snippet (AddTour.aspx.cs, lines 18–28):**
  ```csharp
  SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
  conn.Open();
  string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) values(...)";
  SqlCommand com = new SqlCommand(insertQuery, conn);
  ```
- **Recommendation:** Replace with EF Core `DbContext` and repository pattern. Create `TourDbContext` with `DbSet<Tour>` and `DbSet<Booking>` entities.
- **Effort:** High

#### ISSUE-014: ConfigurationManager — Not Available in .NET 8 Without NuGet Package
- **Files:** AddTour.aspx.cs (line 5), Order.aspx.cs (line 5), SignUpForm.aspx.cs (line 5), userlogin.aspx.cs (line 5), TourCrud.aspx.cs (line 10), DisplayTours.aspx.cs (line 5)
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** `System.Configuration.ConfigurationManager` is used to read connection strings. In .NET 8, configuration is handled via `IConfiguration` / `appsettings.json`.
- **Code Snippet:**
  ```csharp
  using System.Configuration;
  ...
  ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString
  ```
- **Recommendation:** Inject `IConfiguration` via DI and use `configuration.GetConnectionString("DefaultConnection")`. Better yet, use EF Core with `DbContext` configured in `Program.cs`.
- **Effort:** Medium

#### ISSUE-015: Microsoft.CodeDom.Providers.DotNetCompilerPlatform — Incompatible with .NET 8
- **File:** packages.config, Tour_Management.csproj, Web.config
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** `Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1` is a .NET Framework package for Roslyn compiler support in Web Forms. It is not needed in .NET 8 (Roslyn is built-in).
- **Code Snippet (packages.config):**
  ```xml
  <package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />
  ```
- **Recommendation:** Remove this package entirely. .NET 8 uses the Roslyn compiler natively.
- **Effort:** Low

#### ISSUE-016: ASP.NET Server Controls in .aspx Markup — All Must Be Replaced
- **Files:** All .aspx files
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** All .aspx pages use `runat="server"` server controls: `<asp:Label>`, `<asp:TextBox>`, `<asp:Button>`, `<asp:DropDownList>`, `<asp:RegularExpressionValidator>`, `<asp:HyperLink>`, `<asp:FileUpload>`. None of these exist in .NET 8.
- **Code Snippet (AddTour.aspx, line 35):**
  ```xml
  <asp:TextBox id="tour_name" required="true" ForeColor="Black" class="form-control" runat="server"/>
  <asp:Button BackColor="#cc6600" ID="Register" runat="server" Text="Register" OnClick="Register_Click" />
  ```
- **Recommendation:** Replace with standard HTML elements and Razor Tag Helpers:
  ```html
  <input asp-for="TourName" class="form-control" required />
  <button type="submit" class="btn btn-warning">Register</button>
  ```
- **Effort:** High

#### ISSUE-017: Postback Event Handlers — Not Supported in .NET 8
- **Files:** AddTour.aspx.cs (Register_Click), Order.aspx.cs (btn_click), SignUpForm.aspx.cs (Register_Click), userlogin.aspx.cs (Btn_Submit, Btn_reg)
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** Web Forms postback event handlers (`OnClick="Register_Click"`) are tied to the Web Forms event model which does not exist in .NET 8.
- **Code Snippet (AddTour.aspx, line 72):**
  ```xml
  <asp:Button ID="Register" runat="server" Text="Register" OnClick="Register_Click" />
  ```
- **Recommendation:** Replace with Razor Pages `OnPost()` / `OnPostAsync()` handler methods. Use `<form method="post">` with `asp-page-handler` Tag Helper for named handlers.
- **Effort:** High

#### ISSUE-018: Hardcoded Absolute File Path in Connection String
- **File:** Web.config, line 30
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** The connection string contains a hardcoded absolute path to the developer's local machine.
- **Code Snippet:**
  ```xml
  connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf;Integrated Security=True"
  ```
- **Recommendation:** Use environment-specific configuration in `appsettings.json` and `appsettings.Development.json`. Use a proper SQL Server instance or SQL Server LocalDB with a relative path.
- **Effort:** Low

#### ISSUE-019: No Authentication/Authorization Middleware
- **Files:** AdminLogin2.aspx.cs, userlogin.aspx.cs, AdminProfile.aspx, MainProfilePage.aspx
- **Severity:** High
- **Breaking Change:** No (new requirement)
- **Description:** There is no session management, no authentication cookies, and no authorization checks on protected pages. After "login", there is no mechanism to verify the user is authenticated on subsequent requests.
- **Recommendation:** Implement ASP.NET Core Identity or cookie authentication. Add `[Authorize]` attributes to protected Razor Pages. Configure authentication middleware in `Program.cs`.
- **Effort:** High

#### ISSUE-020: No Input Validation Beyond RegularExpressionValidator
- **Files:** AddTour.aspx (RegularExpressionValidator), all other forms
- **Severity:** High
- **Breaking Change:** No (new requirement)
- **Description:** Only one `<asp:RegularExpressionValidator>` exists in the entire application. No server-side validation is performed before database operations.
- **Code Snippet (AddTour.aspx, line 71):**
  ```xml
  <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
      ControlToValidate="tour_info" ValidationExpression="^[\s\S]{0,250}$" 
      ErrorMessage="Characters less than 250">
  </asp:RegularExpressionValidator>
  ```
- **Recommendation:** Implement FluentValidation or Data Annotations for all input models. Add server-side validation in PageModel handlers.
- **Effort:** Medium

#### ISSUE-021: No Error Handling / Try-Catch Blocks
- **Files:** All code-behind files
- **Severity:** High
- **Breaking Change:** No (quality issue)
- **Description:** No code-behind file contains any try-catch error handling. Database connections are opened but may not be closed if an exception occurs.
- **Code Snippet (AddTour.aspx.cs, lines 18–30):**
  ```csharp
  SqlConnection conn = new SqlConnection(...);
  conn.Open();
  // ... no try-catch, conn.Close() at end may never execute
  conn.Close();
  ```
- **Recommendation:** Use `using` statements for `SqlConnection`/`DbContext`. Implement global exception handling middleware in .NET 8.
- **Effort:** Medium

#### ISSUE-022: Response.Write for User Feedback — Not Appropriate
- **Files:** AddTour.aspx.cs (line 30), Order.aspx.cs (line 21), SignUpForm.aspx.cs (line 29), userlogin.aspx.cs (lines 33, 43)
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** `Response.Write()` is used to display success/error messages directly into the HTTP response stream. This is not available in .NET 8 in the same way and produces poor UX.
- **Code Snippet (AddTour.aspx.cs, line 30):**
  ```csharp
  Response.Write("ADD  Successful");
  ```
- **Recommendation:** Use `TempData` for redirect messages or `ModelState.AddModelError()` for validation errors. Display messages using Razor syntax.
- **Effort:** Low

#### ISSUE-023: System.Web.DataVisualization Chart Registration in allbooking.aspx
- **File:** allbooking.aspx, lines 3–4
- **Severity:** High
- **Breaking Change:** Yes
- **Description:** The `allbooking.aspx` page registers the `System.Web.DataVisualization.Charting` assembly via `<%@ Register %>` directive, even though no chart control is actually used on the page.
- **Code Snippet:**
  ```xml
  <%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, ..." 
      namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
  ```
- **Recommendation:** Remove this registration. If charting is needed, use Chart.js or a compatible .NET 8 library.
- **Effort:** Low

#### ISSUE-024: Dead Code After Response.Redirect
- **Files:** Order.aspx.cs, userlogin.aspx.cs, SignUpForm.aspx.cs
- **Severity:** High
- **Breaking Change:** No (code quality)
- **Description:** Code after `Response.Redirect()` (including `Server.Transfer()` and `conn.Close()`) is unreachable because `Response.Redirect()` throws `ThreadAbortException` in .NET Framework.
- **Code Snippet (Order.aspx.cs, lines 22–25):**
  ```csharp
  Response.Redirect("mybooking.aspx");
  Server.Transfer("mybooking.aspx");  // Never executes
  conn.Close();                        // Never executes — connection leak
  ```
- **Recommendation:** Restructure code to ensure cleanup happens before redirect. Use `using` blocks for connections.
- **Effort:** Low

#### ISSUE-025: No Async/Await — All Database Operations Are Synchronous
- **Files:** All code-behind files with database operations
- **Severity:** High
- **Breaking Change:** No (performance issue)
- **Description:** All database operations are synchronous, blocking threads. .NET 8 best practices require async/await for all I/O operations.
- **Recommendation:** Use `await conn.OpenAsync()`, `await com.ExecuteNonQueryAsync()`, and EF Core async methods (`await _context.SaveChangesAsync()`).
- **Effort:** Medium

#### ISSUE-026: No Logging Infrastructure
- **Files:** All files
- **Severity:** High
- **Breaking Change:** No (new requirement)
- **Description:** No logging is implemented anywhere in the application. No way to diagnose issues in production.
- **Recommendation:** Implement `ILogger<T>` via dependency injection. Configure Serilog or Microsoft.Extensions.Logging in `Program.cs`.
- **Effort:** Medium

#### ISSUE-027: Inline CSS Styles — Should Be Externalized
- **Files:** All .aspx files
- **Severity:** High
- **Breaking Change:** No (quality/maintainability)
- **Description:** All CSS is defined inline within `<style>` tags in each page's `<head>`. No shared stylesheet exists.
- **Recommendation:** Create a shared `site.css` in `wwwroot/css/`. Use Bootstrap 5 for layout and components.
- **Effort:** Medium

#### ISSUE-028: No Dependency Injection
- **Files:** All code-behind files
- **Severity:** High
- **Breaking Change:** No (architectural issue)
- **Description:** All dependencies (database connections, configuration) are instantiated directly in code-behind files. No DI container is used.
- **Recommendation:** Configure DI in `Program.cs`. Inject `IConfiguration`, `DbContext`, and service classes via constructor injection in PageModel classes.
- **Effort:** High

---

### MEDIUM Issues

#### ISSUE-029: .aspx Page Directives — Not Supported in .NET 8
- **Files:** All .aspx files
- **Severity:** Medium
- **Breaking Change:** Yes
- **Description:** `<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="..." Inherits="..." %>` directives are Web Forms-specific and have no equivalent in .NET 8 Razor Pages.
- **Recommendation:** Replace with Razor Pages `.cshtml` files with `@page` and `@model` directives.
- **Effort:** Medium

#### ISSUE-030: Designer Files (.aspx.designer.cs) — Not Needed in .NET 8
- **Files:** All 10 .aspx.designer.cs files
- **Severity:** Medium
- **Breaking Change:** No (cleanup)
- **Description:** Designer files auto-generate server control field declarations. They are not needed in Razor Pages.
- **Recommendation:** Delete all `.aspx.designer.cs` files during migration.
- **Effort:** Low

#### ISSUE-031: ConnectionStrings Expression Syntax in Markup
- **Files:** DisplayTours.aspx, TourCrud.aspx, mybooking.aspx, allbooking.aspx, usercrud.aspx
- **Severity:** Medium
- **Breaking Change:** Yes
- **Description:** `ConnectionString="<%$ ConnectionStrings:dbconnection %>"` is a Web Forms expression syntax not available in Razor Pages.
- **Code Snippet:**
  ```xml
  <asp:SqlDataSource ConnectionString="<%$ ConnectionStrings:dbconnection %>" ...>
  ```
- **Recommendation:** Remove SqlDataSource controls entirely. Use EF Core with connection string from `appsettings.json`.
- **Effort:** Low (part of SqlDataSource migration)

#### ISSUE-032: Eval() Data Binding Expressions in Markup
- **Files:** DisplayTours.aspx (line 22), TourCrud.aspx (line 20)
- **Severity:** Medium
- **Breaking Change:** Yes
- **Description:** `<%#Eval("pic") %>` is a Web Forms data binding expression not available in Razor Pages.
- **Code Snippet (DisplayTours.aspx, line 22):**
  ```xml
  <img src="Tour_pics/<%#Eval("pic") %>" style="width:200px;height:200px" />
  ```
- **Recommendation:** Use Razor syntax: `<img src="Tour_pics/@item.Pic" style="width:200px;height:200px" />`
- **Effort:** Low

#### ISSUE-033: No HTTPS Enforcement
- **File:** Web.config, Tour_Management.csproj
- **Severity:** Medium
- **Breaking Change:** No (security improvement)
- **Description:** No HTTPS redirection is configured. The IIS Express SSL port (44300) is set but not enforced.
- **Recommendation:** Add `app.UseHttpsRedirection()` and `app.UseHsts()` in `Program.cs`.
- **Effort:** Low

#### ISSUE-034: No Anti-Forgery Token (CSRF Protection)
- **Files:** All .aspx form pages
- **Severity:** Medium
- **Breaking Change:** No (security improvement)
- **Description:** No CSRF protection is implemented on any form.
- **Recommendation:** Razor Pages automatically include anti-forgery tokens with `<form method="post">`. Ensure `services.AddAntiforgery()` is configured in `Program.cs`.
- **Effort:** Low

#### ISSUE-035: Inconsistent Page Naming Convention
- **Files:** userlogin.aspx, usercrud.aspx, mybooking.aspx, allbooking.aspx
- **Severity:** Medium
- **Breaking Change:** No (naming convention)
- **Description:** Some pages use lowercase names (userlogin, usercrud, mybooking, allbooking) while others use PascalCase (AddTour, AdminLogin2, AdminProfile).
- **Recommendation:** Use consistent PascalCase naming for all Razor Pages following the migration strategy: `UserLogin.cshtml`, `UserCrud.cshtml`, etc.
- **Effort:** Low

#### ISSUE-036: App_Data Folder with .mdf File — Not Recommended for Production
- **File:** App_Data/tourdb.mdf
- **Severity:** Medium
- **Breaking Change:** No (deployment concern)
- **Description:** The application uses a LocalDB `.mdf` file in the `App_Data` folder. This is a development-only approach not suitable for production.
- **Recommendation:** Migrate to a proper SQL Server instance. Use EF Core migrations to manage schema. Store connection string in environment variables or Azure Key Vault for production.
- **Effort:** Medium

#### ISSUE-037: No Pagination on GridViews
- **Files:** DisplayTours.aspx, TourCrud.aspx, allbooking.aspx, usercrud.aspx
- **Severity:** Medium
- **Breaking Change:** No (feature gap)
- **Description:** GridViews display all records without pagination, which will cause performance issues with large datasets.
- **Recommendation:** Implement server-side pagination using EF Core `.Skip()` / `.Take()` with page number parameters.
- **Effort:** Medium

#### ISSUE-038: Missing DOCTYPE in userlogin.aspx
- **File:** userlogin.aspx
- **Severity:** Medium
- **Breaking Change:** No (HTML quality)
- **Description:** `userlogin.aspx` is missing the `<!DOCTYPE html>` declaration, which can cause browser rendering issues.
- **Recommendation:** Add `<!DOCTYPE html>` to the Razor Page layout.
- **Effort:** Low

#### ISSUE-039: Unclosed HTML div Tag in userlogin.aspx
- **File:** userlogin.aspx, line 28
- **Severity:** Medium
- **Breaking Change:** No (HTML quality)
- **Description:** There is an unclosed `</div>` tag in the markup (`</div>` appears without a matching opening tag).
- **Code Snippet (userlogin.aspx, line 28):**
  ```html
  </div>  <!-- No matching opening <div> -->
  ```
- **Recommendation:** Fix HTML structure during migration to Razor Pages.
- **Effort:** Low

#### ISSUE-040: External Google Fonts CDN Reference Without SRI
- **File:** userlogin.aspx, line 14
- **Severity:** Medium
- **Breaking Change:** No (security concern)
- **Description:** External CDN resource loaded without Subresource Integrity (SRI) hash.
- **Code Snippet:**
  ```html
  <link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Rubik:400,700'>
  ```
- **Recommendation:** Add SRI hash or self-host the font. Consider using a Content Security Policy (CSP) header.
- **Effort:** Low

---

### LOW Issues

#### ISSUE-041: Web.Debug.config and Web.Release.config — Not Needed in .NET 8
- **Files:** Web.Debug.config, Web.Release.config
- **Severity:** Low
- **Breaking Change:** No (cleanup)
- **Description:** Web.config transform files are not used in .NET 8.
- **Recommendation:** Delete these files. Use `appsettings.Development.json` and `appsettings.Production.json` instead.
- **Effort:** Low

#### ISSUE-042: AssemblyInfo.cs — Partially Redundant in SDK-Style Projects
- **File:** Properties/AssemblyInfo.cs
- **Severity:** Low
- **Breaking Change:** No (cleanup)
- **Description:** SDK-style .NET 8 projects auto-generate assembly attributes. The existing `AssemblyInfo.cs` may cause duplicate attribute errors.
- **Recommendation:** Remove or simplify `AssemblyInfo.cs`. Add `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>` to `.csproj` if keeping the file.
- **Effort:** Low

#### ISSUE-043: No Master Page / Shared Layout
- **Files:** All .aspx files
- **Severity:** Low
- **Breaking Change:** No (architectural improvement)
- **Description:** No master page is used. Navigation menus are duplicated across AdminProfile.aspx and MainProfilePage.aspx as inline HTML.
- **Recommendation:** Create a shared `_Layout.cshtml` in `Pages/Shared/` with the navigation bar and common HTML structure.
- **Effort:** Medium

#### ISSUE-044: Tour_pics Folder — Static Files Handling
- **Files:** AddTour.aspx.cs, DisplayTours.aspx, TourCrud.aspx
- **Severity:** Low
- **Breaking Change:** No (deployment concern)
- **Description:** Tour images are stored in `Tour_pics/` folder and referenced directly. In .NET 8, static files must be in `wwwroot/`.
- **Recommendation:** Move `Tour_pics/` to `wwwroot/Tour_pics/`. Configure `app.UseStaticFiles()` in `Program.cs`.
- **Effort:** Low

#### ISSUE-045: No Null Checks on Database Results
- **Files:** userlogin.aspx.cs (line 30), TourCrud.aspx.cs
- **Severity:** Low
- **Breaking Change:** No (robustness)
- **Description:** `ExecuteScalar()` can return null, which is handled with `?.ToString() ?? ""` in userlogin but not consistently elsewhere.
- **Code Snippet (userlogin.aspx.cs, line 30):**
  ```csharp
  string password = passComm.ExecuteScalar()?.ToString() ?? "";
  ```
- **Recommendation:** Enable nullable reference types (`<Nullable>enable</Nullable>`) and handle all nullable returns consistently.
- **Effort:** Low

#### ISSUE-046: Commented-Out Code in TourCrud.aspx.cs
- **File:** TourCrud.aspx.cs, lines 18–30
- **Severity:** Low
- **Breaking Change:** No (code quality)
- **Description:** Large blocks of commented-out code exist in `TourCrud.aspx.cs`, including an alternative data loading approach.
- **Code Snippet:**
  ```csharp
  // SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;...");
  // SqlCommand cmd = new SqlCommand("select * from tbl_data", con);
  // SqlDataAdapter sda = new SqlDataAdapter(cmd);
  ```
- **Recommendation:** Remove all commented-out code during migration.
- **Effort:** Low

#### ISSUE-047: No Global.asax — Missing Application Startup Configuration
- **Severity:** Low
- **Breaking Change:** No (missing feature)
- **Description:** No `Global.asax` file exists, meaning no application-level startup configuration, error handling, or session configuration is present.
- **Recommendation:** Create `Program.cs` with full ASP.NET Core middleware pipeline configuration including authentication, authorization, EF Core, static files, and routing.
- **Effort:** Medium

---

## Migration Roadmap

### Phase 1: Foundation (Week 1–2) — ~20 hours
1. Create new SDK-style solution with clean architecture layers
2. Set up `Program.cs` with middleware pipeline
3. Create `appsettings.json` with connection strings
4. Implement EF Core `DbContext` with `Tour`, `UserInfo`, and `Booking` entities
5. Create repository interfaces and implementations
6. Configure ASP.NET Core Identity for authentication

### Phase 2: Core Pages Migration (Week 2–3) — ~25 hours
7. Migrate `userlogin.aspx` → `Pages/Account/Login.cshtml` with cookie auth
8. Migrate `SignUpForm.aspx` → `Pages/Account/Register.cshtml` with Identity
9. Migrate `MainProfilePage.aspx` → `Pages/Index.cshtml`
10. Migrate `DisplayTours.aspx` → `Pages/Tours/Index.cshtml`
11. Migrate `Order.aspx` → `Pages/Tours/Book.cshtml`
12. Migrate `mybooking.aspx` → `Pages/Bookings/MyBookings.cshtml`

### Phase 3: Admin Pages Migration (Week 3–4) — ~15 hours
13. Migrate `AdminLogin2.aspx` → Role-based auth (admin role)
14. Migrate `AdminProfile.aspx` → `Pages/Admin/Index.cshtml`
15. Migrate `AddTour.aspx` → `Pages/Admin/Tours/Create.cshtml` with IFormFile
16. Migrate `TourCrud.aspx` → `Pages/Admin/Tours/Index.cshtml` (CRUD)
17. Migrate `usercrud.aspx` → `Pages/Admin/Users/Index.cshtml`
18. Migrate `allbooking.aspx` → `Pages/Admin/Bookings/Index.cshtml`

### Phase 4: Security & Quality (Week 4) — ~20 hours
19. Implement password hashing (remove plaintext passwords)
20. Fix SQL injection vulnerability
21. Add FluentValidation for all input models
22. Implement CSRF protection
23. Add HTTPS enforcement
24. Implement logging with Serilog
25. Add error handling middleware

---

## Migration Mapping Table

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| `System.Web.UI.Page` | `Microsoft.AspNetCore.Mvc.RazorPages.PageModel` |
| `.aspx` markup | `.cshtml` Razor Page |
| `Page_Load()` | `OnGet()` / `OnGetAsync()` |
| Button `OnClick` handler | `OnPost()` / `OnPostAsync()` |
| `<asp:TextBox>` | `<input asp-for="..." />` |
| `<asp:Label>` | `<label asp-for="..." />` or `<span>` |
| `<asp:Button>` | `<button type="submit">` |
| `<asp:GridView>` | `@foreach` loop with Bootstrap table |
| `<asp:SqlDataSource>` | EF Core `DbContext` + Repository |
| `<asp:FileUpload>` | `IFormFile` parameter |
| `<asp:DropDownList>` | `<select asp-for="..." asp-items="...">` |
| `<asp:RegularExpressionValidator>` | FluentValidation / Data Annotations |
| `Response.Redirect()` | `return RedirectToPage("...")` |
| `Server.MapPath()` | `IWebHostEnvironment.WebRootPath` |
| `Server.Transfer()` | Remove (use RedirectToPage) |
| `Response.Write()` | `TempData` / `ModelState` |
| `ConfigurationManager` | `IConfiguration` |
| `SqlConnection` / `SqlCommand` | EF Core `DbContext` |
| `Web.config` | `appsettings.json` + `Program.cs` |
| `Global.asax` | `Program.cs` |
| `packages.config` | `<PackageReference>` in .csproj |
| `System.Web.DataVisualization` | Chart.js (client-side) |

---

## Recommended .NET 8 Package References

```xml
<!-- Infrastructure Layer -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.0" />

<!-- Web Layer -->
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
```

---

*Report generated by Web Forms Migration Analyzer v1.1.0*
