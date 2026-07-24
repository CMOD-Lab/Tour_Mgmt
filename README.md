# Tour Management System - .NET 8

A modern Tour Management web application built with ASP.NET Core 8 Razor Pages, following Clean Architecture principles.

## Architecture

This application follows Clean Architecture with four layers:

- **Domain** (`TourManagement.Domain`) - Entities, interfaces, domain exceptions
- **Application** (`TourManagement.Application`) - Business logic, services, DTOs, validators
- **Infrastructure** (`TourManagement.Infrastructure`) - EF Core, repositories, data access
- **Web** (`TourManagement.Web`) - Razor Pages, ViewModels, static files

## Features

- User registration and login
- Browse and search tour packages
- Book tours
- Admin dashboard with tour/user/booking management
- Full CRUD for tours, users, and bookings

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Database Setup
1. Update the connection string in `src/TourManagement.Web/appsettings.json`
2. Run migrations:
   ```bash
   cd src/TourManagement.Web
   dotnet ef migrations add InitialCreate --project ../TourManagement.Infrastructure
   dotnet ef database update --project ../TourManagement.Infrastructure
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

## Admin Access
- Default admin email: `admin@gmail.com`
- Default admin password: `admin`
- Configure in `appsettings.json` under `AdminCredentials`

## Migration Notes
This application was migrated from ASP.NET Web Forms 4.7.2 to .NET 8.
See `docs/MIGRATION_NOTES.md` for details.
