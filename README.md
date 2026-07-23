# Tour Management System - .NET 8 Migration

## Overview
This is a Tour Management System migrated from ASP.NET Web Forms 4.7.2 to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, interfaces, DTOs
│   ├── TourManagement.Application/     # Business logic, services, validators
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, data access
│   └── TourManagement.Web/             # Razor Pages UI layer
├── tests/
│   └── TourManagement.UnitTests/       # Unit tests
└── docs/                               # Documentation
```

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Setup Instructions

### 1. Configure Database
Update the connection string in `src/TourManagement.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=tourdb;Trusted_Connection=True;"
  }
}
```

### 2. Run Database Migrations
```bash
cd src/TourManagement.Web
dotnet ef database update --project ../TourManagement.Infrastructure
```

### 3. Run the Application
```bash
cd src/TourManagement.Web
dotnet run
```

## Features
- **User Management**: Registration, login, profile management
- **Tour Management**: Browse, create, edit, delete tours (admin)
- **Booking System**: Book tours, view/cancel bookings
- **Admin Panel**: Dashboard with statistics, manage all data

## Default Admin Credentials
- Email: `admin@gmail.com`
- Password: `admin`

## Migration Notes
See `docs/MIGRATION_NOTES.md` for details on what was migrated from Web Forms.

## Build
```bash
dotnet build TourManagement.sln
```

## Test
```bash
dotnet test TourManagement.sln
```
