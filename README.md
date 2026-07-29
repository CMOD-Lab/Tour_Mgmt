# Tour Management System - .NET 8 Migration

## Overview
This project is a complete migration of the ASP.NET Web Forms 4.7.2 Tour Management application to .NET 8 using clean architecture principles.

## Architecture
The solution follows Clean Architecture with four layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, interfaces, exceptions
│   ├── TourManagement.Application/     # Business logic, services, DTOs, mappings
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, data access
│   └── TourManagement.Web/             # Razor Pages UI, ViewModels, static files
├── tests/
│   └── TourManagement.UnitTests/       # xUnit unit tests
└── docs/                               # Documentation
```

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Database Setup
1. Update the connection string in `src/TourManagement.Web/appsettings.json`
2. Run migrations: `dotnet ef database update --project src/TourManagement.Infrastructure --startup-project src/TourManagement.Web`

### Running the Application
```bash
cd src/TourManagement.Web
dotnet run
```

### Running Tests
```bash
dotnet test
```

## Key Features
- Tour browsing and search
- User registration and login
- Tour booking management
- Admin dashboard with CRUD operations
- Session-based authentication

## Migration Notes
- Migrated from ASP.NET Web Forms 4.7.2 to .NET 8 Razor Pages
- Replaced ADO.NET with Entity Framework Core 8.0
- Replaced System.Web with ASP.NET Core equivalents
- Replaced Web.config with appsettings.json
- Implemented clean architecture with proper separation of concerns
- Added dependency injection throughout
- Implemented async/await patterns
- Added comprehensive error handling and logging with Serilog
