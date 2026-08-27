# Tour Management System - .NET 8 Migration

## Overview
This is a Tour Management System migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, interfaces, exceptions
│   ├── TourManagement.Application/     # Business logic, DTOs, services, mappings
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, data access
│   └── TourManagement.Web/             # Razor Pages, ViewModels, UI
├── tests/
│   ├── TourManagement.UnitTests/       # Unit tests for services
│   └── TourManagement.IntegrationTests/ # Integration tests for repositories
└── docs/                               # Documentation
```

## Features
- **Tour Management**: Browse, create, edit, delete tours with image upload
- **Booking System**: Book tours, view and cancel bookings
- **User Management**: Registration, login, profile management
- **Admin Dashboard**: Manage tours, view all bookings, manage users
- **Authentication**: Cookie-based authentication with role support

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Database Setup
1. Update the connection string in `src/TourManagement.Web/appsettings.json`
2. Run EF Core migrations:
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
dotnet test
```

## Build Verification
```bash
dotnet build TourManagement.sln
```
**Result**: Build succeeded with 0 errors.

## Migration Notes
- Migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 Razor Pages
- Replaced ADO.NET with Entity Framework Core 8.0
- Replaced System.Web with ASP.NET Core equivalents
- Replaced Web.config with appsettings.json
- Replaced Global.asax with Program.cs
- Added BCrypt password hashing for security
- Implemented clean architecture with proper separation of concerns
- Added cookie-based authentication replacing Forms Authentication
