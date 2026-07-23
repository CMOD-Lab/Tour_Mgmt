# Tour Management System - .NET 8

A modern Tour Management web application built with ASP.NET Core 8 Razor Pages, following Clean Architecture principles.

## Architecture

This solution follows **Clean Architecture** with four main layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, interfaces, exceptions
│   ├── TourManagement.Application/     # Business logic, services, DTOs, validators
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, data access
│   └── TourManagement.Web/             # Razor Pages UI, ViewModels, static files
├── tests/
│   ├── TourManagement.UnitTests/       # Unit tests for services
│   └── TourManagement.IntegrationTests/ # Integration tests for repositories
└── docs/                               # Documentation
```

## Features

- **User Management**: Registration, login, profile management
- **Tour Management**: Full CRUD for tour packages with image upload
- **Booking System**: Book tours, view/cancel bookings
- **Admin Panel**: Dashboard, manage tours, users, and bookings
- **Search**: Search tours by name or place

## Technology Stack

- **Framework**: ASP.NET Core 8 Razor Pages
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server (LocalDB for development)
- **Logging**: Serilog
- **Mapping**: AutoMapper 12
- **Validation**: FluentValidation 11
- **Testing**: xUnit, Moq, FluentAssertions

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server LocalDB

### Configuration
1. Update the connection string in `src/TourManagement.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=tourdb;Trusted_Connection=True;"
   }
   ```

2. Run database migrations:
   ```bash
   cd src/TourManagement.Web
   dotnet ef database update
   ```

### Running the Application
```bash
cd src/TourManagement.Web
dotnet run
```

### Running Tests
```bash
dotnet test
```

## Migration Notes

This application was migrated from ASP.NET Web Forms 4.7.2 to .NET 8.

### Key Changes
- Replaced `.aspx` pages with Razor Pages (`.cshtml`)
- Replaced ADO.NET with Entity Framework Core 8
- Replaced `Web.config` with `appsettings.json`
- Replaced `System.Web` with ASP.NET Core equivalents
- Replaced `Response.Redirect` with `RedirectToPage()`
- Replaced `Server.MapPath` with `IWebHostEnvironment`
- Replaced `Session` with ASP.NET Core session middleware
- Replaced `ConfigurationManager` with `IConfiguration`
- Implemented Clean Architecture (Domain/Application/Infrastructure/Web)
- Added dependency injection throughout
- Added proper async/await patterns
- Added comprehensive error handling and logging

## Default Admin Credentials
- Email: `admin@gmail.com`
- Password: `admin`

> **Note**: Change these credentials in `appsettings.json` before deploying to production.
