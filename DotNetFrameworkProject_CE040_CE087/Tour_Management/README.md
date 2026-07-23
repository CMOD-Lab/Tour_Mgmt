# Tour Management System

## Overview
This is a .NET 8 ASP.NET Core Razor Pages application migrated from ASP.NET Web Forms 4.7.2. It provides a complete tour management system with user registration, tour browsing, and booking functionality.

## Architecture
The application follows **Clean Architecture** with four layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, repository interfaces
│   ├── TourManagement.Application/     # Business logic, services, DTOs, mappings
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, database
│   └── TourManagement.Web/             # Razor Pages UI, ViewModels
├── tests/
│   ├── TourManagement.UnitTests/       # Unit tests for services
│   └── TourManagement.IntegrationTests/ # Integration tests for repositories
└── docs/                               # Documentation
```

## Features
- **Tour Management**: Browse, create, edit, and delete tour packages
- **User Management**: Registration, login, profile management
- **Booking System**: Book tours, view personal bookings, admin view all bookings
- **Admin Dashboard**: Overview of tours, users, and bookings

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Configuration
1. Update the connection string in `src/TourManagement.Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;..."
     }
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
- Migrated from ASP.NET Web Forms 4.7.2 to .NET 8 Razor Pages
- Replaced ADO.NET with Entity Framework Core 8.0
- Replaced System.Web with ASP.NET Core equivalents
- Replaced Web.config with appsettings.json
- Implemented BCrypt password hashing (replaces plain-text passwords)
- Session-based authentication (can be upgraded to ASP.NET Core Identity)

## Default Admin Credentials
- Email: admin@gmail.com
- Password: admin
