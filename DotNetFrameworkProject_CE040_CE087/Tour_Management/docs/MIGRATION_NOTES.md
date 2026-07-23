# Migration Notes

## What Was Migrated

### From ASP.NET Web Forms (.NET 4.7.2) to .NET 8 Razor Pages

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| `AddTour.aspx` | `Pages/Tours/Create.cshtml` |
| `DisplayTours.aspx` | `Pages/Tours/Index.cshtml` |
| `TourCrud.aspx` | `Pages/Tours/Index.cshtml` + Edit/Delete |
| `userlogin.aspx` | `Pages/Users/Login.cshtml` |
| `SignUpForm.aspx` | `Pages/Users/Register.cshtml` |
| `AdminLogin2.aspx` | `Pages/Users/Login.cshtml` (admin check) |
| `AdminProfile.aspx` | `Pages/Admin/Dashboard.cshtml` |
| `MainProfilePage.aspx` | `Pages/Users/Profile.cshtml` |
| `Order.aspx` | `Pages/Bookings/Create.cshtml` |
| `mybooking.aspx` | `Pages/Bookings/MyBookings.cshtml` |
| `allbooking.aspx` | `Pages/Bookings/Index.cshtml` |
| `usercrud.aspx` | `Pages/Users/Index.cshtml` |
| `Web.config` | `appsettings.json` |
| `Global.asax` | `Program.cs` |
| ADO.NET SqlCommand | EF Core repositories |

## Key Differences from Web Forms

1. **No ViewState**: State is managed via session and TempData
2. **No Code-Behind**: Logic moved to PageModel classes and service layer
3. **No Server Controls**: Replaced with HTML Tag Helpers
4. **No SqlDataSource**: Replaced with EF Core DbContext
5. **No FileUpload control**: Replaced with IFormFile
6. **No Response.Write**: Replaced with TempData messages
7. **No Server.MapPath**: Replaced with IWebHostEnvironment

## Breaking Changes

1. **Password Storage**: Passwords are now hashed (SHA256 + salt). Existing plain-text passwords in the database will not work.
2. **Database Schema**: New columns added (CreatedDate, ModifiedDate, IsActive, CreatedBy, ModifiedBy, Role)
3. **Connection String**: Format changed from LocalDB MDF attachment to standard SQL Server connection

## Configuration Changes

- `Web.config` → `appsettings.json`
- Connection string format updated
- Logging configured via Serilog

## Known Issues

1. Existing database data may need migration due to schema changes
2. Tour images stored in `Tour_pics/` folder need to be moved to `wwwroot/tour-pics/`

## Future Improvements

1. Implement proper JWT authentication
2. Add pagination for large datasets
3. Implement image optimization
4. Add email notifications for bookings
5. Implement caching for tour listings
