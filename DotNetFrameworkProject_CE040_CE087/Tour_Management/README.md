# Tour Management System - .NET 8

## Overview
This is a Tour Management web application migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

```
Tour_Management/
├── src/
│   ├── Tour_Management.Domain/          # Domain entities, interfaces, DTOs, exceptions
│   ├── Tour_Management.Application/     # Business logic, services, validators, AutoMapper
│   ├── Tour_Management.Infrastructure/  # EF Core, repositories, data configurations
│   └── Tour_Management.Web/             # Razor Pages UI, ViewModels, Program.cs
├── tests/
│   ├── Tour_Management.UnitTests/       # xUnit unit tests with Moq
│   └── Tour_Management.IntegrationTests/ # Integration tests with InMemory DB
└── docs/
    ├── MIGRATION_NOTES.md
    ├── ARCHITECTURE.md
    └── BUILD_VERIFICATION.md
```

## Features
- **Tours**: Full CRUD (Index, Create, Details, Edit, Delete)
- **Bookings**: Full CRUD + My Bookings per user
- **Users**: Registration, Login, Profile, CRUD management
- **Admin Dashboard**: Statistics and quick actions

## Technology Stack
- **.NET 8** with ASP.NET Core Razor Pages
- **Entity Framework Core 8.0** (SQL Server)
- **AutoMapper 12.0.1** for entity-to-DTO mapping
- **FluentValidation 11.9.0** for input validation
- **Serilog 8.0** for structured logging
- **xUnit + Moq + FluentAssertions** for testing

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Configuration
1. Update `appsettings.json` with your connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=True;"
  }
}
```

2. Run database migrations:
```bash
cd src/Tour_Management.Web
dotnet ef database update
```

### Running the Application
```bash
cd src/Tour_Management.Web
dotnet run
```

### Running Tests
```bash
dotnet test
```

## Admin Access
- Email: `admin@gmail.com`
- Password: `admin`

## Migration Notes
See `docs/MIGRATION_NOTES.md` for details on what was migrated from Web Forms.
