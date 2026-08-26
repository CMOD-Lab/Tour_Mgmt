# Migration Notes

## What Was Migrated

### From ASP.NET Web Forms (.NET 4.7.2) to .NET 8 Razor Pages

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| `userlogin.aspx` | `Pages/Users/Login.cshtml` |
| `SignUpForm.aspx` | `Pages/Users/Register.cshtml` |
| `MainProfilePage.aspx` | `Pages/Users/Profile.cshtml` |
| `DisplayTours.aspx` | `Pages/Tours/Index.cshtml` |
| `Order.aspx` | `Pages/Bookings/Create.cshtml` |
| `mybooking.aspx` | `Pages/Bookings/MyBookings.cshtml` |
| `allbooking.aspx` | `Pages/Admin/Bookings.cshtml` |
| `AddTour.aspx` | `Pages/Admin/CreateTour.cshtml` |
| `TourCrud.aspx` | `Pages/Admin/Tours.cshtml` |
| `AdminLogin2.aspx` | `Pages/Admin/Login.cshtml` |
| `AdminProfile.aspx` | `Pages/Admin/Dashboard.cshtml` |
| `usercrud.aspx` | `Pages/Admin/Users.cshtml` |
| `Web.config` | `appsettings.json` |
| `packages.config` | `.csproj` PackageReference |

## Key Differences from Web Forms

1. **No ViewState** - State managed via session and TempData
2. **No Code-Behind** - Page models with proper separation of concerns
3. **No Server Controls** - HTML helpers and Tag Helpers
4. **No Global.asax** - Program.cs with middleware pipeline
5. **No Web.config** - appsettings.json with IConfiguration
6. **No ADO.NET** - Entity Framework Core 8.0.0
7. **No System.Web** - ASP.NET Core equivalents

## Breaking Changes

- Connection string format updated for EF Core
- Authentication changed from session-based to ASP.NET Core session
- File upload uses IFormFile instead of FileUpload server control
- Server.MapPath replaced with IWebHostEnvironment.WebRootPath

## Configuration Changes

- Connection string moved to `appsettings.json`
- Admin credentials moved to `appsettings.json` under `AdminCredentials`
- Logging configured via Serilog in `Program.cs`

## Known Issues

- Password storage is plain text (same as original) - should be hashed with BCrypt in production
- Admin authentication is simple credential check - should use proper Identity in production

## Future Improvements

1. Implement password hashing (BCrypt)
2. Add ASP.NET Core Identity for proper authentication
3. Add pagination for large datasets
4. Implement caching for frequently accessed tours
5. Add email notifications for bookings
