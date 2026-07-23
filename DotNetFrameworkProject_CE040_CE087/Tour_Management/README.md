# Tour Management System - .NET 8 Migration

## Overview
This is a Tour Management System migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

```
Tour_Management/
├── src/
│   ├── Tour_Management.Domain/          # Domain entities, interfaces, exceptions
│   ├── Tour_Management.Application/     # Business logic, services, DTOs, validators
│   ├── Tour_Management.Infrastructure/  # EF Core, repositories, data access
│   └── Tour_Management.Web/             # Razor Pages UI, ViewModels
├── tests/
│   └── Tour_Management.UnitTests/       # xUnit unit tests
└── docs/                                # Documentation
```

## Features
- **Tours**: Browse, search, create, edit, delete tours with image upload
- **User Registration & Login**: Secure registration with BCrypt password hashing
- **Bookings**: Book tours, view personal bookings, cancel bookings
- **Admin Panel**: Dashboard with statistics, manage users and all bookings

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Configuration
1. Update the connection string in `src/Tour_Management.Web/appsettings.json`
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
cd tests/Tour_Management.UnitTests
dotnet test
```

## Build Verification
All projects build successfully with 0 errors:
- Tour_Management.Domain ✅
- Tour_Management.Application ✅
- Tour_Management.Infrastructure ✅
- Tour_Management.Web ✅
- Tour_Management.UnitTests ✅

## Default Admin Credentials
- Email: admin@gmail.com
- Password: admin

> **Note**: Change admin credentials in `appsettings.json` before production deployment.
