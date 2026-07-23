# Tour Management System - .NET 8 Migration

## Overview
This is a Tour Management System migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

```
Tour_Management/
├── src/
│   ├── Tour_Management.Domain/          # Domain entities, interfaces, DTOs
│   ├── Tour_Management.Application/     # Business logic, services, validators
│   ├── Tour_Management.Infrastructure/  # EF Core, repositories, data access
│   └── Tour_Management.Web/             # Razor Pages UI, ViewModels
├── tests/
│   ├── Tour_Management.UnitTests/       # Unit tests for services
│   └── Tour_Management.IntegrationTests/ # Integration tests for repositories
└── docs/                                # Documentation
```

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Setup Instructions

1. **Clone/navigate to the project directory**

2. **Update connection string** in `src/Tour_Management.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TourManagementDb;..."
   }
   ```

3. **Restore packages**:
   ```bash
   dotnet restore
   ```

4. **Build the solution**:
   ```bash
   dotnet build
   ```

5. **Run the application**:
   ```bash
   cd src/Tour_Management.Web
   dotnet run
   ```

## Running Tests
```bash
dotnet test
```

## Default Admin Credentials
- Email: `admin@gmail.com`
- Password: `admin`

## Features
- Browse and search tours
- User registration and login
- Tour booking management
- Admin dashboard
- CRUD operations for Tours, Users, and Bookings

## Migration Notes
See `docs/MIGRATION_NOTES.md` for details on what was migrated from Web Forms.
