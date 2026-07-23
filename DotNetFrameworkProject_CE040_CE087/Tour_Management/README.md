# Tour Management System - .NET 8

A complete Tour Management web application migrated from ASP.NET Web Forms 4.7.2 to .NET 8 using clean architecture principles.

## Architecture

This application follows **Clean Architecture** with four layers:

```
Tour_Management/
├── src/
│   ├── Tour_Management.Domain/          # Domain entities, interfaces, exceptions
│   ├── Tour_Management.Application/     # Business logic, DTOs, services, AutoMapper
│   ├── Tour_Management.Infrastructure/  # EF Core, repositories, data access
│   └── Tour_Management.Web/             # Razor Pages UI, ViewModels, Program.cs
├── tests/
│   └── Tour_Management.UnitTests/       # xUnit unit tests
├── docs/                                # Documentation
└── Tour_Management.sln                  # Solution file
```

## Features

- **Tour Management**: Browse, search, and book tours (CRUD for admins)
- **User Registration & Login**: Secure authentication with BCrypt password hashing
- **Booking System**: Users can book tours and view their bookings
- **Admin Panel**: Manage tours, users, and all bookings
- **Responsive UI**: Bootstrap 5 responsive design

## Technology Stack

- **.NET 8** with ASP.NET Core Razor Pages
- **Entity Framework Core 8.0** with SQL Server
- **AutoMapper 12** for DTO mapping
- **BCrypt.Net** for password hashing
- **Serilog** for structured logging
- **xUnit + FluentAssertions + Moq** for testing

## Setup

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Configuration
Update `src/Tour_Management.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=True;"
  }
}
```

### Database Migration
```bash
cd src/Tour_Management.Web
dotnet ef migrations add InitialCreate --project ../Tour_Management.Infrastructure
dotnet ef database update
```

### Run
```bash
dotnet run --project src/Tour_Management.Web
```

### Test
```bash
dotnet test tests/Tour_Management.UnitTests
```

## Migration Notes

This application was migrated from ASP.NET Web Forms 4.7.2 to .NET 8:

| Old (Web Forms) | New (.NET 8) |
|-----------------|--------------|
| `.aspx` pages | Razor Pages (`.cshtml`) |
| Code-behind files | Page Models (`.cshtml.cs`) |
| `Web.config` | `appsettings.json` |
| `System.Web` | `Microsoft.AspNetCore` |
| ADO.NET `SqlConnection` | Entity Framework Core 8 |
| `Session["key"]` | `HttpContext.Session.GetString("key")` |
| `Response.Redirect` | `RedirectToPage()` |
| `Server.MapPath` | `IWebHostEnvironment.WebRootPath` |
| `packages.config` | `PackageReference` in `.csproj` |

## Admin Access
- URL: `/Admin/Login`
- Default credentials: `admin@gmail.com` / `admin`
