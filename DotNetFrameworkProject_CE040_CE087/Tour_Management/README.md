# Tour Management - .NET 8 Migration

## Overview
This is a Tour Management web application migrated from ASP.NET Web Forms (.NET 4.7.2) to ASP.NET Core Razor Pages (.NET 8) using Clean Architecture principles.

## Architecture

The solution follows Clean Architecture with four layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, interfaces, exceptions
│   ├── TourManagement.Application/     # Business logic, services, DTOs, mappings
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, data access
│   └── TourManagement.Web/             # Razor Pages, ViewModels, UI
├── tests/
│   ├── TourManagement.UnitTests/       # Unit tests for services
│   └── TourManagement.IntegrationTests/# Integration tests
└── docs/                               # Documentation
```

## Features
- **Tour Management**: Full CRUD for tour packages (Index, Create, Details, Edit, Delete)
- **User Management**: Registration, login, profile management
- **Booking System**: Create and manage tour bookings
- **Admin Dashboard**: Overview of tours, users, and bookings
- **Authentication**: Cookie-based authentication

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Configuration
1. Update `appsettings.json` with your database connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=True;"
  }
}
```

2. Run database migrations:
```bash
cd src/TourManagement.Web
dotnet ef database update --project ../TourManagement.Infrastructure
```

### Running the Application
```bash
cd src/TourManagement.Web
dotnet run
```

### Running Tests
```bash
dotnet test tests/TourManagement.UnitTests/
```

## Migration Notes
- Migrated from ASP.NET Web Forms to ASP.NET Core Razor Pages
- Replaced ADO.NET with Entity Framework Core 8.0
- Replaced Web.config with appsettings.json
- Replaced Global.asax with Program.cs
- Replaced Forms Authentication with Cookie Authentication
- Replaced System.Web with ASP.NET Core equivalents
- Added clean architecture layers (Domain, Application, Infrastructure, Web)
- Added dependency injection throughout
- Added async/await patterns
- Added proper error handling and logging with Serilog

## Default Admin Credentials
- Email: admin@gmail.com
- Password: admin

> **Note**: Change these credentials in production via `appsettings.json` AdminSettings section.
