# Migration Notes

## What Was Migrated

### From ASP.NET Web Forms (.NET 4.7.2) to .NET 8 Razor Pages

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| `userlogin.aspx` | `Pages/Users/Login.cshtml` |
| `SignUpForm.aspx` | `Pages/Users/Register.cshtml` |
| `MainProfilePage.aspx` | `Pages/Users/Profile.cshtml` |
| `AdminLogin2.aspx` | `Pages/Admin/Login.cshtml` |
| `AdminProfile.aspx` | `Pages/Admin/Dashboard.cshtml` |
| `AddTour.aspx` | `Pages/Tours/Create.cshtml` |
| `TourCrud.aspx` | `Pages/Tours/Index.cshtml` |
| `DisplayTours.aspx` | `Pages/Tours/Index.cshtml` |
| `Order.aspx` | `Pages/Bookings/Create.cshtml` |
| `mybooking.aspx` | `Pages/Bookings/MyBookings.cshtml` |
| `allbooking.aspx` | `Pages/Bookings/Index.cshtml` |
| `usercrud.aspx` | `Pages/Users/Index.cshtml` |
| `Web.config` | `appsettings.json` |
| `Global.asax` | `Program.cs` |

## Key Differences from Web Forms

1. **No ViewState** - State managed via session and TempData
2. **No Code-Behind** - Page models with proper separation of concerns
3. **No Server Controls** - HTML helpers and Tag Helpers
4. **No System.Web** - ASP.NET Core equivalents used throughout
5. **No ADO.NET** - Entity Framework Core 8.0 with repository pattern
6. **No Forms Authentication** - Session-based authentication (can be upgraded to ASP.NET Core Identity)

## Breaking Changes

- Password storage: Passwords are now hashed with SHA256 (existing plain-text passwords in old DB won't work)
- Database schema: New columns added (IsActive, CreatedDate, ModifiedDate, etc.)
- Connection string format: Updated to EF Core format

## Configuration Changes

- `Web.config` → `appsettings.json`
- Connection string moved to `ConnectionStrings:DefaultConnection`
- Admin credentials configurable via `AdminCredentials` section

## Known Issues

- Existing database data may need migration due to schema changes
- Image files from `Tour_pics/` folder need to be copied to `wwwroot/images/tours/`

## Future Improvements

- Implement ASP.NET Core Identity for proper authentication
- Add JWT authentication for API endpoints
- Implement pagination for large datasets
- Add image optimization
- Implement caching strategies
