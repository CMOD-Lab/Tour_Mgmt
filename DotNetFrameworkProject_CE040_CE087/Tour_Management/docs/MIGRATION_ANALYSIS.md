# Tour_Management – ASP.NET Web Forms to .NET 8 Migration Analysis

**Analysis Date:** 2025-01-30  
**Module:** Tour_Management  
**Current Framework:** ASP.NET Web Forms 4.7.2  
**Target Framework:** .NET 8  
**Migration Complexity:** Complex  
**Estimated Effort:** 80–120 hours  

---

## Executive Summary

The Tour_Management application is a classic ASP.NET Web Forms project targeting .NET Framework 4.7.2.
It contains **11 Web Forms pages** (.aspx), **11 code-behind files** (.aspx.cs), **no master pages**, and **no user controls**.
The application uses raw ADO.NET (SqlConnection / SqlCommand) for all data access, hard-coded credentials for admin authentication, plain-text password storage, and direct `System.Web` APIs throughout.

**Total Issues Found: 47**

| Severity | Count |
|----------|-------|
| Critical | 14    |
| High     | 16    |
| Medium   | 11    |
| Low      | 6     |

---

## 1. Project Configuration Issues

### 1.1 Legacy Non-SDK-Style Project File (Critical)
- **File:** `Tour_Management.csproj`
- **Issue:** Uses the old MSBuild project format with `ProjectTypeGuids` for Web Application. Not compatible with .NET 8 SDK-style projects.
- **Fix:** Migrate to SDK-style project: `<Project Sdk="Microsoft.NET.Sdk.Web">` with `<TargetFramework>net8.0</TargetFramework>`.

### 1.2 TargetFrameworkVersion v4.7.2 (Critical)
- **File:** `Tour_Management.csproj`, line 18
- **Code:** `<TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>`
- **Fix:** Replace with `<TargetFramework>net8.0</TargetFramework>` in SDK-style project.

### 1.3 System.Web Assembly References (Critical)
- **File:** `Tour_Management.csproj`, lines 44–60
- **Code:** `<Reference Include="System.Web" />`, `<Reference Include="System.Web.Extensions" />`, `<Reference Include="System.Web.DynamicData" />`, `<Reference Include="System.Web.Entity" />`, `<Reference Include="System.Web.ApplicationServices" />`
- **Fix:** Remove all System.Web references. Use ASP.NET Core equivalents via NuGet packages.

### 1.4 Web.config Must Be Replaced with appsettings.json (Critical)
- **File:** `Web.config`
- **Issue:** Web.config is not supported in .NET 8. Contains connection strings, compilation settings, HTTP handlers, and app settings.
- **Fix:** Migrate to `appsettings.json` and configure in `Program.cs`.

### 1.5 Microsoft.CodeDom.Providers.DotNetCompilerPlatform Package (High)
- **File:** `packages.config`, line 3
- **Code:** `<package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.1" targetFramework="net472" />`
- **Fix:** Remove this package. .NET 8 uses Roslyn by default; this package is not needed.

---

## 2. System.Web Dependencies (Critical – All Files)

### 2.1 System.Web.UI.Page Base Class (Critical)
All 11 code-behind files inherit from `System.Web.UI.Page`, which does not exist in .NET 8.

| File | Line | Code |
|------|------|------|
| AddTour.aspx.cs | 12 | `public partial class AddTour : System.Web.UI.Page` |
| AdminLogin2.aspx.cs | 11 | `public partial class AdminLogin2 : System.Web.UI.Page` |
| AdminProfile.aspx.cs | 11 | `public partial class AdminProfile : System.Web.UI.Page` |
| DisplayTours.aspx.cs | 11 | `public partial class DisplayTours : System.Web.UI.Page` |
| TourCrud.aspx.cs | 12 | `public partial class TourCrud : System.Web.UI.Page` |
| Order.aspx.cs | 11 | `public partial class Order : System.Web.UI.Page` |
| SignUpForm.aspx.cs | 11 | `public partial class SignUpForm : System.Web.UI.Page` |
| userlogin.aspx.cs | 11 | `public partial class userlogin : System.Web.UI.Page` |
| usercrud.aspx.cs | 11 | `public partial class usercrud : System.Web.UI.Page` |
| allbooking.aspx.cs | 11 | `public partial class allbooking : System.Web.UI.Page` |
| mybooking.aspx.cs | 11 | `public partial class mybooking : System.Web.UI.Page` |

**Fix:** Migrate each page to a Razor Page (`PageModel`) or MVC Controller.

### 2.2 System.Web.UI.WebControls Namespace (Critical)
- **Files:** All .aspx.cs files
- **Code:** `using System.Web.UI.WebControls;`
- **Fix:** Remove. Use Tag Helpers and HTML helpers in Razor Pages.

### 2.3 System.Web.HttpContext / Response / Request (Critical)
- **Files:** AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs
- **Code:** `Response.Write(...)`, `Response.Redirect(...)`, `Server.Transfer(...)`, `Server.MapPath(...)`
- **Fix:**
  - `Response.Redirect(url)` → `return RedirectToPage("PageName")`
  - `Server.MapPath(path)` → `IWebHostEnvironment.WebRootPath`
  - `Response.Write(text)` → Use TempData or model properties

### 2.4 System.Web.HttpServerUtility.MapPath (Critical)
- **File:** `AddTour.aspx.cs`, line 30
- **Code:** `FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);`
- **Fix:** Inject `IWebHostEnvironment` and use `_env.WebRootPath` to resolve physical paths.

---

## 3. Web Forms Page Lifecycle Events (High)

### 3.1 Page_Load Event Handler (High)
- **Files:** All .aspx.cs files
- **Code:** `protected void Page_Load(object sender, EventArgs e)`
- **Fix:** Replace with `OnGet()` / `OnPost()` methods in Razor Page `PageModel`.

### 3.2 Page.IsPostBack Check (High)
- **File:** `TourCrud.aspx.cs`, line 14
- **Code:** `if (!Page.IsPostBack) { refreshdata(); }`
- **Fix:** In Razor Pages, `OnGet()` is only called on GET requests; no IsPostBack check needed.

### 3.3 Button Click Event Handlers (High)
- **Files:** AddTour.aspx.cs (Register_Click), Order.aspx.cs (btn_click), SignUpForm.aspx.cs (Register_Click), userlogin.aspx.cs (Btn_Submit, Btn_reg)
- **Code:** `protected void Register_Click(object sender, EventArgs e)`
- **Fix:** Replace with `OnPost()` or named handler methods (`OnPostRegister()`) in Razor Page PageModel.

---

## 4. Server Controls (High – All .aspx Files)

### 4.1 asp:TextBox Controls (High)
- **Files:** All .aspx files
- **Fix:** Replace with standard HTML `<input>` elements with `asp-for` Tag Helpers.

### 4.2 asp:Button Controls (High)
- **Files:** AddTour.aspx, Order.aspx, SignUpForm.aspx, userlogin.aspx, AdminLogin2.aspx
- **Fix:** Replace with `<button type="submit">` or `<input type="submit">`.

### 4.3 asp:Label Controls (High)
- **Files:** Multiple .aspx files
- **Fix:** Replace with `<label asp-for="...">` Tag Helpers or plain HTML `<span>`.

### 4.4 asp:GridView Controls (High)
- **Files:** DisplayTours.aspx, TourCrud.aspx, usercrud.aspx, allbooking.aspx, mybooking.aspx
- **Code:** `<asp:GridView ID="GridView1" runat="server" ...>`
- **Fix:** Replace with Razor `@foreach` loops rendering HTML `<table>` elements, or use a modern component library.

### 4.5 asp:SqlDataSource Controls (Critical)
- **Files:** DisplayTours.aspx, TourCrud.aspx, usercrud.aspx, allbooking.aspx, mybooking.aspx
- **Code:** `<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbconnection %>" ...>`
- **Fix:** Remove entirely. Implement data access via EF Core repositories injected into PageModel.

### 4.6 asp:FileUpload Control (High)
- **File:** `AddTour.aspx`, line 55
- **Code:** `<asp:FileUpload ID="FileUpload1" runat="server"/>`
- **Fix:** Replace with `<input type="file" asp-for="UploadedFile">` and handle `IFormFile` in PageModel.

### 4.7 asp:RegularExpressionValidator Control (Medium)
- **File:** `AddTour.aspx`, line 72
- **Code:** `<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ...>`
- **Fix:** Replace with Data Annotations (`[MaxLength(250)]`) and FluentValidation.

### 4.8 asp:HyperLink Control (Medium)
- **File:** `DisplayTours.aspx`, line 26
- **Code:** `<asp:HyperLink ID="HyperLink1" href="Order.aspx" runat="server">Book Now</asp:HyperLink>`
- **Fix:** Replace with `<a asp-page="/Order">Book Now</a>`.

### 4.9 asp:DropDownList Control (Medium)
- **File:** `SignUpForm.aspx`, lines 42–46
- **Code:** `<asp:DropDownList ID="gender" runat="server" ...>`
- **Fix:** Replace with `<select asp-for="Gender" asp-items="...">`.

### 4.10 runat="server" Attribute on HTML Elements (Medium)
- **Files:** Multiple .aspx files
- **Code:** `<form id="form1" runat="server">`, `<div ... runat="server">`
- **Fix:** Remove `runat="server"` attributes. Use standard HTML elements with Tag Helpers.

### 4.11 System.Web.DataVisualization Charting Control (Critical)
- **File:** `allbooking.aspx`, line 2; `Web.config`, lines 5–22
- **Code:** `<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0 ..." %>`
- **Fix:** Replace with a .NET 8 compatible charting library (e.g., Chart.js via JavaScript, or `LiveCharts2`).

---

## 5. Data Access Issues (Critical)

### 5.1 Raw ADO.NET SqlConnection (Critical)
- **Files:** AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs, userlogin.aspx.cs, TourCrud.aspx.cs
- **Code:** `SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);`
- **Fix:** Replace with EF Core `DbContext` and repository pattern.

### 5.2 System.Configuration.ConfigurationManager (Critical)
- **Files:** AddTour.aspx.cs (line 4), Order.aspx.cs (line 4), SignUpForm.aspx.cs (line 4), userlogin.aspx.cs (line 4), TourCrud.aspx.cs (line 10), DisplayTours.aspx.cs (line 4)
- **Code:** `using System.Configuration;` and `ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString`
- **Fix:** Use `IConfiguration` injected via DI. Connection string in `appsettings.json`.

### 5.3 SQL Injection Vulnerability (Critical – Security)
- **File:** `userlogin.aspx.cs`, line 26
- **Code:** `string checkPasswordQuery = "select password from Userinfo where password='" + txtPassword.Text + "' and email = '" + txtEmail.Text + "'";`
- **Fix:** Use parameterized queries (EF Core handles this automatically) or at minimum `SqlParameter`.

### 5.4 Plain-Text Password Storage (Critical – Security)
- **File:** `userlogin.aspx.cs`, line 26; `SignUpForm.aspx.cs`, line 28
- **Code:** Password stored and compared as plain text in database.
- **Fix:** Use ASP.NET Core Identity with password hashing (`IPasswordHasher<T>`).

### 5.5 Hard-Coded Admin Credentials (Critical – Security)
- **File:** `AdminLogin2.aspx.cs`, lines 14–15
- **Code:** `if (password.Text == "admin" && name.Text == "admin@gmail.com")`
- **Fix:** Implement proper authentication using ASP.NET Core Identity with role-based authorization.

### 5.6 Hard-Coded Connection String with Absolute Path (High)
- **File:** `Web.config`, line 28
- **Code:** `AttachDbFilename=C:\Users\gajer\source\repos\Tour_Management\Tour_Management\App_Data\tourdb.mdf`
- **Fix:** Use environment-specific connection strings in `appsettings.json` / environment variables.

### 5.7 Unreachable Code After Response.Redirect (Medium)
- **Files:** Order.aspx.cs (lines 28–29), SignUpForm.aspx.cs (lines 37–38), userlogin.aspx.cs (lines 38–39)
- **Code:** `Response.Redirect("mybooking.aspx"); Server.Transfer("mybooking.aspx");` (both called sequentially)
- **Fix:** In Razor Pages, use `return RedirectToPage(...)` which returns immediately.

### 5.8 SqlDataSource with Inline SQL in ASPX (Critical)
- **Files:** DisplayTours.aspx, TourCrud.aspx, usercrud.aspx, allbooking.aspx, mybooking.aspx
- **Code:** `SelectCommand="SELECT * FROM [Tour]"`, `DeleteCommand="Delete from [booking] Where [TOUR_ID]=@TOUR_ID"`
- **Fix:** Move all data access to EF Core repositories. Remove SqlDataSource controls entirely.

---

## 6. Authentication & Security Issues (Critical)

### 6.1 No Authentication Mechanism (Critical)
- **Issue:** No Forms Authentication, no session-based auth, no authorization checks on any page.
- **Fix:** Implement ASP.NET Core Identity with cookie authentication. Add `[Authorize]` attributes to protected pages.

### 6.2 No CSRF Protection (High)
- **Issue:** Web Forms uses ViewState for CSRF protection. No equivalent in the current implementation.
- **Fix:** Razor Pages include anti-forgery tokens by default via `@Html.AntiForgeryToken()` / `[ValidateAntiForgeryToken]`.

### 6.3 Password Displayed in GridView (Critical – Security)
- **File:** `usercrud.aspx`, line 18
- **Code:** `<asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />`
- **Fix:** Never display passwords. Remove this column entirely.

### 6.4 No Input Validation (High)
- **Files:** AddTour.aspx.cs, Order.aspx.cs, SignUpForm.aspx.cs
- **Issue:** No server-side validation beyond a single RegularExpressionValidator.
- **Fix:** Implement Data Annotations and FluentValidation on all input models.

---

## 7. Configuration Migration Issues (High)

### 7.1 Web.config HTTP Handler Registration (High)
- **File:** `Web.config`, lines 4–10
- **Code:** `<system.webServer><handlers>` with `ChartHttpHandler`
- **Fix:** Register equivalent middleware in `Program.cs`.

### 7.2 Web.config Compilation Settings (High)
- **File:** `Web.config`, line 23
- **Code:** `<compilation debug="true" targetFramework="4.7.2">`
- **Fix:** Remove. .NET 8 uses `launchSettings.json` and build configuration for debug/release.

### 7.3 Web.config httpRuntime (Medium)
- **File:** `Web.config`, line 27
- **Code:** `<httpRuntime targetFramework="4.7.2"/>`
- **Fix:** Remove. Not applicable in .NET 8.

### 7.4 Web.config system.codedom Compilers (Medium)
- **File:** `Web.config`, lines 30–37
- **Code:** `<system.codedom><compilers>` section
- **Fix:** Remove entirely. .NET 8 uses Roslyn by default.

### 7.5 ValidationSettings:UnobtrusiveValidationMode (Low)
- **File:** `Web.config`, line 29
- **Code:** `<add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />`
- **Fix:** Remove. Web Forms validation controls do not exist in .NET 8.

---

## 8. Web Forms Page Directives (High)

### 8.1 @Page Directive (High)
- **Files:** All .aspx files
- **Code:** `<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="..." Inherits="..." %>`
- **Fix:** Replace .aspx files with .cshtml Razor Pages. Remove @Page directives.

### 8.2 @Register Directive for DataVisualization (Critical)
- **File:** `allbooking.aspx`, line 2
- **Code:** `<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>`
- **Fix:** Remove. Replace with a .NET 8 compatible charting solution.

### 8.3 ConnectionStrings Expression Syntax (High)
- **Files:** DisplayTours.aspx, TourCrud.aspx, usercrud.aspx, allbooking.aspx, mybooking.aspx
- **Code:** `ConnectionString="<%$ ConnectionStrings:dbconnection %>"`
- **Fix:** Remove SqlDataSource controls. Use EF Core with connection string from `IConfiguration`.

### 8.4 Data Binding Expression Syntax (Medium)
- **Files:** DisplayTours.aspx, TourCrud.aspx
- **Code:** `<%#Eval("pic") %>`
- **Fix:** Replace with Razor `@item.Pic` in foreach loops.

---

## 9. Missing Application Infrastructure (High)

### 9.1 No Global.asax (Medium)
- **Issue:** No Global.asax found. Application startup logic needs to be placed in `Program.cs`.
- **Fix:** Create `Program.cs` with service registration, middleware pipeline, and EF Core configuration.

### 9.2 No Dependency Injection (High)
- **Issue:** All dependencies are instantiated directly (new SqlConnection, etc.).
- **Fix:** Register all services in `Program.cs` using `builder.Services.Add*()`.

### 9.3 No Logging (High)
- **Issue:** No logging framework used anywhere in the application.
- **Fix:** Add `ILogger<T>` via DI. Configure Serilog or Microsoft.Extensions.Logging in `Program.cs`.

### 9.4 No Error Handling (High)
- **Issue:** No try-catch blocks in any data access code. Unhandled exceptions will crash the application.
- **Fix:** Add try-catch in all service methods. Configure global exception handling middleware.

---

## 10. Static Files & Assets (Low)

### 10.1 Tour_pics Folder (Low)
- **Issue:** Image files stored in `Tour_pics/` folder. In .NET 8, static files must be in `wwwroot/`.
- **Fix:** Move to `wwwroot/tour-pics/`. Update file upload path logic.

### 10.2 pics Folder (Low)
- **Issue:** Background images in `pics/` folder referenced in inline CSS.
- **Fix:** Move to `wwwroot/images/`. Update CSS references.

### 10.3 External Font Reference (Low)
- **File:** `userlogin.aspx`, line 14
- **Code:** `<link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Rubik:400,700'>`
- **Fix:** Keep CDN reference or bundle locally. Update in shared layout.

---

## Migration Roadmap

### Phase 1: Foundation (Week 1–2)
1. Create new .NET 8 solution with clean architecture (Domain, Application, Infrastructure, Web)
2. Set up EF Core with SQL Server provider
3. Create domain entities: `Tour`, `UserInfo`, `Booking`
4. Implement repository interfaces and EF Core implementations
5. Configure `appsettings.json` with connection strings
6. Set up ASP.NET Core Identity for authentication

### Phase 2: Core Pages (Week 3–4)
1. Migrate `userlogin.aspx` → `Pages/Account/Login.cshtml`
2. Migrate `SignUpForm.aspx` → `Pages/Account/Register.cshtml`
3. Migrate `AdminLogin2.aspx` → Role-based admin access via Identity
4. Migrate `MainProfilePage.aspx` → `Pages/Index.cshtml`
5. Migrate `AdminProfile.aspx` → `Pages/Admin/Index.cshtml`

### Phase 3: Tour Management (Week 5–6)
1. Migrate `AddTour.aspx` → `Pages/Admin/Tours/Create.cshtml`
2. Migrate `TourCrud.aspx` → `Pages/Admin/Tours/Index.cshtml` (with Edit/Delete)
3. Migrate `DisplayTours.aspx` → `Pages/Tours/Index.cshtml`
4. Implement file upload with `IFormFile`

### Phase 4: Booking (Week 7)
1. Migrate `Order.aspx` → `Pages/Bookings/Create.cshtml`
2. Migrate `mybooking.aspx` → `Pages/Bookings/Index.cshtml`
3. Migrate `allbooking.aspx` → `Pages/Admin/Bookings/Index.cshtml`
4. Migrate `usercrud.aspx` → `Pages/Admin/Users/Index.cshtml`

### Phase 5: Security & Quality (Week 8)
1. Implement password hashing
2. Add authorization policies
3. Add input validation (FluentValidation)
4. Add logging (Serilog)
5. Add error handling middleware
6. Write unit and integration tests

---

## Page Complexity Assessment

| Page | Complexity | Key Issues |
|------|-----------|------------|
| AddTour.aspx | Complex | FileUpload, ADO.NET, Server.MapPath |
| AdminLogin2.aspx | Simple | Hard-coded credentials |
| AdminProfile.aspx | Simple | Navigation only |
| DisplayTours.aspx | Medium | GridView, SqlDataSource, image binding |
| MainProfilePage.aspx | Simple | Navigation only |
| Order.aspx | Medium | ADO.NET insert, redirect |
| SignUpForm.aspx | Medium | ADO.NET insert, plain-text password |
| TourCrud.aspx | Complex | GridView CRUD, SqlDataSource |
| allbooking.aspx | Medium | GridView, DataVisualization reference |
| mybooking.aspx | Medium | GridView with delete, SqlDataSource |
| userlogin.aspx | Medium | SQL injection, plain-text password |
| usercrud.aspx | Medium | GridView, password exposed |
