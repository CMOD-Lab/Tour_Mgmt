# Migration Notes — Tour_Management

## What Was Migrated

This document describes the migration from ASP.NET Web Forms 4.7.2 to .NET 8 Razor Pages with clean architecture.

### Source Application
- **Framework:** ASP.NET Web Forms 4.7.2
- **Pages:** 11 Web Forms pages (.aspx)
- **Data Access:** Raw ADO.NET (SqlConnection / SqlCommand)
- **Configuration:** Web.config
- **Authentication:** Custom (hardcoded credentials + raw SQL)

### Target Application
- **Framework:** .NET 8 (net8.0)
- **UI:** Razor Pages
- **Data Access:** Entity Framework Core 8.0.0
- **Configuration:** appsettings.json
- **Authentication:** ASP.NET Core Identity

---

## Key Differences from Web Forms

| Web Forms | .NET 8 Razor Pages |
|---|---|
| `.aspx` + `.aspx.cs` code-behind | `.cshtml` + `.cshtml.cs` PageModel |
| `System.Web.UI.Page` base class | `Microsoft.AspNetCore.Mvc.RazorPages.PageModel` |
| `Page_Load` event | `OnGet()` / `OnGetAsync()` |
| Button `OnClick` event | `OnPost()` / `OnPostAsync()` |
| `asp:TextBox`, `asp:Button` server controls | HTML elements + Tag Helpers |
| `asp:GridView` + `asp:SqlDataSource` | Razor `@foreach` + EF Core |
| `Response.Redirect()` | `return RedirectToPage()` |
| `Server.MapPath()` | `IWebHostEnvironment.WebRootPath` |
| `ConfigurationManager` | `IConfiguration` |
| `Web.config` | `appsettings.json` |
| `packages.config` | `<PackageReference>` in `.csproj` |
| `Global.asax` | `Program.cs` |
| HTTP Modules | ASP.NET Core Middleware |
| HTTP Handlers | Minimal API Endpoints |
| ViewState | Hidden fields / TempData |
| Forms Authentication | ASP.NET Core Identity |
| `Session["key"]` | `HttpContext.Session["key"]` (with configuration) |

---

## Breaking Changes

1. **System.Web removed** — All `using System.Web.*` statements must be removed. No equivalent NuGet package exists.
2. **Web Forms page lifecycle removed** — `Page_Load`, `Page_PreRender`, `IsPostBack`, etc. do not exist.
3. **Server controls removed** — All `asp:*` controls must be replaced with HTML + Tag Helpers.
4. **Web.config removed** — Configuration must move to `appsettings.json`.
5. **Non-SDK project format** — `.csproj` must be rewritten in SDK style.
6. **packages.config removed** — Must use `<PackageReference>` format.
7. **Designer files removed** — `.aspx.designer.cs` files have no equivalent.
8. **System.Web.DataVisualization removed** — Chart controls must be replaced with JavaScript libraries.
9. **ConfigurationManager** — Must be replaced with `IConfiguration`.
10. **Server.MapPath()** — Must be replaced with `IWebHostEnvironment`.

---

## Configuration Changes

### Connection String Migration

**Before (Web.config):**
```xml
<connectionStrings>
  <add name="dbconnection" 
       connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gajer\...\tourdb.mdf;Integrated Security=True" 
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

**After (appsettings.json):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### App Settings Migration

**Before (Web.config):**
```xml
<appSettings>
  <add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />
  <add key="ChartImageHandler" value="storage=file;timeout=20;dir=c:\TempImageFiles\;" />
</appSettings>
```

**After (appsettings.json):**
```json
{
  "AppSettings": {
    "ImageUploadPath": "Tour_pics"
  }
}
```
Note: `ValidationSettings:UnobtrusiveValidationMode` is Web Forms-specific and not needed. `ChartImageHandler` is removed as the chart control is replaced.

---

## Known Issues

1. **Plaintext passwords in database** — Existing user passwords stored in plaintext cannot be automatically migrated. All users must reset their passwords after migration.
2. **Hardcoded admin credentials** — The admin account (`admin@gmail.com` / `admin`) must be seeded into the Identity database with a hashed password.
3. **MDF file** — The `App_Data/tourdb.mdf` LocalDB file must be migrated to a proper SQL Server database using EF Core migrations.
4. **Image files** — The `Tour_pics/` directory contents must be moved to `wwwroot/Tour_pics/` in the new project.
5. **Chart control** — `System.Web.DataVisualization` is registered in `allbooking.aspx` but no chart is rendered. The registration can be removed.
6. **SQL injection** — The `userlogin.aspx.cs` login query is vulnerable to SQL injection. This is fixed by migrating to ASP.NET Core Identity.

---

## Future Improvements

1. Implement proper image storage (Azure Blob Storage or AWS S3) instead of file system
2. Add email confirmation for user registration
3. Implement password reset functionality
4. Add tour search and filtering
5. Implement booking cancellation workflow
6. Add admin dashboard with statistics
7. Implement proper pagination for all list pages
8. Add API endpoints for potential mobile app integration
9. Implement caching for frequently accessed tour data
10. Add comprehensive logging and monitoring
