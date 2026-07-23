# Tour Management - .NET 8 Migration

## Overview
This is a Tour Management web application migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

- **Tour_Management.Domain** - Domain entities, interfaces, and exceptions
- **Tour_Management.Application** - Business logic services, DTOs, AutoMapper profiles
- **Tour_Management.Infrastructure** - EF Core DbContext, repositories, data configurations
- **Tour_Management.Web** - ASP.NET Core Razor Pages UI

## Features
- Tour management (CRUD operations)
- User registration and login
- Tour booking system
- Admin dashboard
- Session-based authentication

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Configuration
Update the connection string in `src/Tour_Management.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=True;"
  }
}
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

### Building
```bash
dotnet build Tour_Management.sln
```

## Migration Notes
- Migrated from ASP.NET Web Forms to ASP.NET Core Razor Pages
- Replaced ADO.NET with Entity Framework Core 8.0
- Replaced Web.config with appsettings.json
- Replaced System.Web with ASP.NET Core equivalents
- Added clean architecture layers
- Added dependency injection throughout
- Added async/await patterns
- Added proper error handling and logging with Serilog

## Admin Access
- URL: `/Admin/Login`
- Default credentials: admin@gmail.com / admin (configurable in appsettings.json)
