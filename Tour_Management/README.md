# Tour Management System - .NET 8

A modern Tour Management web application built with ASP.NET Core 8 Razor Pages, following Clean Architecture principles.

## Architecture

This solution follows Clean Architecture with four layers:

```
Tour_Management/
├── src/
│   ├── Tour_Management.Domain/          # Domain entities, interfaces, exceptions
│   ├── Tour_Management.Application/     # Business logic, services, DTOs, validators
│   ├── Tour_Management.Infrastructure/  # EF Core, repositories, data access
│   └── Tour_Management.Web/             # Razor Pages UI, ViewModels, Program.cs
└── tests/
    ├── Tour_Management.UnitTests/        # Unit tests for services
    └── Tour_Management.IntegrationTests/ # Integration tests for repositories
```

## Features

- **Tour Management**: Browse, create, edit, and delete tour packages
- **User Registration & Login**: Secure user authentication with session management
- **Booking System**: Book tours and manage your bookings
- **Admin Dashboard**: Admin panel for managing tours, users, and bookings
- **Responsive UI**: Bootstrap 5 responsive design

## Technology Stack

- **Framework**: ASP.NET Core 8 Razor Pages
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server (configurable)
- **Logging**: Serilog
- **Mapping**: AutoMapper 12
- **Validation**: FluentValidation 11
- **Testing**: xUnit, Moq, FluentAssertions

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (or SQL Server LocalDB)

### Configuration
1. Update the connection string in `src/Tour_Management.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=tourdb;..."
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

## Database Schema

The application uses three main tables:
- **UserInfo**: User accounts (Email as PK)
- **Tour**: Tour packages (TOUR_ID as PK)
- **booking**: Tour bookings (TOUR_ID as PK)

## Admin Access
- Email: `admin@gmail.com`
- Password: `admin`

## Migration Notes

This application was migrated from ASP.NET Web Forms 4.7.2 to .NET 8.
See `docs/MIGRATION_NOTES.md` for detailed migration information.
