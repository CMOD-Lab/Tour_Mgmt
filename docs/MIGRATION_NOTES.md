# Migration Notes

## What Was Migrated

### From ASP.NET Web Forms 4.7.2 to .NET 8 Razor Pages

| Web Forms File | .NET 8 Equivalent |
|---|---|
| `userlogin.aspx` | `Pages/User/Login.cshtml` |
| `SignUpForm.aspx` | `Pages/User/Register.cshtml` |
| `MainProfilePage.aspx` | `Pages/User/Profile.cshtml` |
| `usercrud.aspx` | `Pages/User/Index.cshtml` |
| `AdminLogin2.aspx` | `Pages/Admin/Login.cshtml` |
| `AdminProfile.aspx` | `Pages/Admin/Dashboard.cshtml` |
| `DisplayTours.aspx` | `Pages/Tour/Index.cshtml` |
| `TourCrud.aspx` | `Pages/Tour/Index.cshtml` (admin view) |
| `AddTour.aspx` | `Pages/Tour/Create.cshtml` |
| `Order.aspx` | `Pages/Booking/Create.cshtml` |
| `mybooking.aspx` | `Pages/Booking/MyBookings.cshtml` |
| `allbooking.aspx` | `Pages/Booking/AllBookings.cshtml` |
| `Web.config` | `appsettings.json` |
| `packages.config` | `.csproj` PackageReference |

## Key Differences from Web Forms

1. **No Code-Behind**: Logic moved to PageModel classes with proper separation
2. **No ViewState**: State managed via session and TempData
3. **No Server Controls**: Replaced with HTML Tag Helpers
4. **No Response.Write**: Proper Razor syntax used
5. **No Server.MapPath**: Replaced with `IWebHostEnvironment.WebRootPath`
6. **No HttpContext.Current**: Injected via `IHttpContextAccessor`
7. **No SqlConnection/SqlCommand**: Replaced with EF Core repositories
8. **No ConfigurationManager**: Replaced with `IConfiguration`

## Breaking Changes

1. **Password Hashing**: Passwords are now BCrypt hashed (old plain-text passwords won't work)
2. **Database Schema**: `BookingDate` column added to booking table
3. **Password Column Length**: Increased from 50 to 100 chars for BCrypt hash

## Configuration Changes

- Connection string moved from `Web.config` to `appsettings.json`
- Admin credentials configurable in `appsettings.json` under `AppSettings`

## Security Improvements

1. BCrypt password hashing (was plain-text)
2. CSRF protection via Razor Pages anti-forgery tokens
3. Parameterized queries via EF Core (was SQL injection vulnerable)
4. Session-based authentication

## Known Issues

1. Existing users with plain-text passwords need to re-register
2. Tour images need to be copied to `wwwroot/images/tours/`

## Future Improvements

1. Implement ASP.NET Core Identity for full authentication
2. Add JWT authentication for API endpoints
3. Implement pagination for large datasets
4. Add email notifications for bookings
5. Implement image optimization for tour pictures
