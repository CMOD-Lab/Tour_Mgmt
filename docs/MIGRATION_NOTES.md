# Migration Notes

## What Was Migrated

### From ASP.NET Web Forms 4.7.2 to .NET 8 ASP.NET Core Razor Pages

| Web Forms Component | .NET 8 Equivalent |
|---------------------|-------------------|
| `userlogin.aspx` | `Pages/Users/Login.cshtml` |
| `SignUpForm.aspx` | `Pages/Users/Register.cshtml` |
| `MainProfilePage.aspx` | `Pages/Users/Profile.cshtml` |
| `DisplayTours.aspx` | `Pages/Tours/Index.cshtml` |
| `AddTour.aspx` | `Pages/Tours/Create.cshtml` |
| `TourCrud.aspx` | `Pages/Tours/Index.cshtml` + Edit/Delete |
| `Order.aspx` | `Pages/Bookings/Create.cshtml` |
| `mybooking.aspx` | `Pages/Bookings/MyBookings.cshtml` |
| `allbooking.aspx` | `Pages/Bookings/AllBookings.cshtml` |
| `AdminLogin2.aspx` | `Pages/Admin/Login.cshtml` |
| `AdminProfile.aspx` | `Pages/Admin/Dashboard.cshtml` |
| `usercrud.aspx` | `Pages/Users/Index.cshtml` |
| `Web.config` | `appsettings.json` |
| `packages.config` | `.csproj` PackageReference |

## Key Differences from Web Forms

1. **No Code-Behind**: Logic moved to PageModel classes with proper separation
2. **No ViewState**: State managed via Session and TempData
3. **No Server Controls**: Replaced with HTML Tag Helpers
4. **No System.Web**: Replaced with ASP.NET Core equivalents
5. **Dependency Injection**: All services injected via constructor
6. **Async/Await**: All I/O operations are async
7. **EF Core**: Replaced ADO.NET with Entity Framework Core 8

## Breaking Changes

- Password storage: Now uses BCrypt hashing (existing plain-text passwords won't work)
- Database schema: `BookingDate` column added to `booking` table
- Password column length increased to 100 chars for BCrypt hash

## Configuration Changes

- Connection string moved from `Web.config` to `appsettings.json`
- Admin credentials configurable in `appsettings.json`

## Security Improvements

- Passwords now hashed with BCrypt
- CSRF protection enabled by default in Razor Pages
- Input validation with FluentValidation
- Parameterized queries via EF Core (prevents SQL injection)
