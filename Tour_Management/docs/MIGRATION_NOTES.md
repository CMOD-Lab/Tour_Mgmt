# Migration Notes

## Overview
This document describes the migration from ASP.NET Web Forms 4.7.2 to .NET 8 ASP.NET Core Razor Pages.

## What Was Migrated

### Pages Migrated
| Web Forms Page | Razor Page |
|---|---|
| userlogin.aspx | Pages/Users/Login.cshtml |
| SignUpForm.aspx | Pages/Users/Register.cshtml |
| MainProfilePage.aspx | Pages/Users/Profile.cshtml |
| AdminLogin2.aspx | Pages/Users/Login.cshtml (admin) |
| AdminProfile.aspx | Pages/Admin/Dashboard.cshtml |
| DisplayTours.aspx | Pages/Tours/Index.cshtml |
| AddTour.aspx | Pages/Tours/Create.cshtml |
| TourCrud.aspx | Pages/Tours/Index.cshtml + Edit/Delete |
| Order.aspx | Pages/Bookings/Create.cshtml |
| mybooking.aspx | Pages/Bookings/MyBookings.cshtml |
| allbooking.aspx | Pages/Bookings/Index.cshtml |
| usercrud.aspx | Pages/Users/Index.cshtml |

## Key Differences from Web Forms

1. **No ViewState**: State managed via TempData, Session, and model binding
2. **No Code-Behind**: Logic moved to PageModel classes and service layer
3. **No Server Controls**: Replaced with HTML Tag Helpers
4. **No Global.asax**: Application startup in Program.cs
5. **No Web.config**: Configuration in appsettings.json
6. **No ADO.NET**: Replaced with Entity Framework Core 8

## Breaking Changes

1. **Password Hashing**: Passwords are now hashed with SHA-256. Existing plain-text passwords in the database will not work.
2. **Connection String**: Must be updated in appsettings.json
3. **Database Schema**: Added `CreatedDate` and `IsActive` columns to all tables
4. **Password Column**: Extended to 200 characters for hashed passwords

## Configuration Changes

### Old Web.config
```xml
<connectionStrings>
  <add name="dbconnection" connectionString="..." />
</connectionStrings>
```

### New appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```

## Security Improvements

1. Passwords are now hashed (SHA-256 with salt)
2. CSRF protection enabled by default in Razor Pages
3. Input validation with FluentValidation
4. SQL injection prevention via EF Core parameterized queries
5. Session-based authentication

## Known Issues

1. File upload paths need to be configured for production
2. Admin credentials are stored in appsettings.json (should use proper identity in production)
3. Password hashing uses SHA-256 (consider BCrypt for production)

## Future Improvements

1. Implement ASP.NET Core Identity for proper authentication
2. Add JWT authentication for API endpoints
3. Implement pagination for large datasets
4. Add image optimization for tour pictures
5. Implement email notifications for bookings
