# Tour Management System - .NET 8

A modern ASP.NET Core 8 Razor Pages application for managing tours and bookings, migrated from ASP.NET Web Forms 4.7.2.

## Architecture

This application follows **Clean Architecture** principles with four layers:

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities, interfaces, exceptions
│   ├── TourManagement.Application/     # Business logic, services, DTOs, validators
│   ├── TourManagement.Infrastructure/  # EF Core, repositories, data access
│   └── TourManagement.Web/             # Razor Pages UI, ViewModels, Program.cs
├── tests/
│   ├── TourManagement.UnitTests/       # Unit tests (xUnit, Moq, FluentAssertions)
│   └── TourManagement.IntegrationTests/ # Integration tests (EF Core InMemory)
└── docs/                               # Documentation
```

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Setup

1. **Clone the repository**
2. **Update connection string** in `src/TourManagement.Web/appsettings.json`
3. **Run the application**:
   ```bash
   cd src/TourManagement.Web
   dotnet run
   ```

## Database Setup

The application uses Entity Framework Core with SQL Server. The database is created automatically on first run (development mode).

SQL schema reference: `Tour_Mgmt_SQL.sql`

## Features

- **User Registration & Login** - Secure user authentication with session management
- **Tour Browsing** - View and search available tours
- **Tour Booking** - Book tours with confirmation
- **My Bookings** - View and cancel personal bookings
- **Admin Panel** - Manage tours, users, and all bookings
- **File Upload** - Tour picture upload support

## Default Admin Credentials

- Email: `admin@gmail.com`
- Password: `admin`

## Running Tests

```bash
dotnet test
```

## Migration Notes

See `docs/MIGRATION_NOTES.md` for details on what was migrated from Web Forms.
