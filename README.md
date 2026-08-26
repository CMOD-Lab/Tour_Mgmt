# Tour Booking System - .NET 8 Migration

## Overview
This is a Tour Booking web application migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles with Razor Pages.

## Architecture
The solution follows Clean Architecture with four layers:

- **TourBooking.Domain** - Domain entities, interfaces, and exceptions
- **TourBooking.Application** - Business logic services, DTOs, AutoMapper profiles
- **TourBooking.Infrastructure** - EF Core DbContext, repositories, data configurations
- **TourBooking.Web** - ASP.NET Core Razor Pages UI layer

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Setup Instructions

1. **Clone the repository**
2. **Update connection string** in `src/TourBooking.Web/appsettings.json`
3. **Run database migrations** (or create the database manually using `Tour_Mgmt_SQL.sql`)
4. **Run the application**:
   ```bash
   cd src/TourBooking.Web
   dotnet run
   ```

## Database Setup
Run the SQL scripts in `Tour_Mgmt_SQL.sql` to create the database and tables:
- `tourdb` database
- `UserInfo` table
- `Tour` table
- `booking` table

## Default Admin Credentials
- Email: `admin@gmail.com`
- Password: `admin`

## Features
- User registration and login
- Browse and search tours
- Book tours
- View and cancel bookings
- Admin panel for managing tours, users, and bookings

## Migration Notes
See `docs/MIGRATION_NOTES.md` for details on what was migrated from Web Forms.

## Build
```bash
dotnet build TourBooking.sln
```

## Test
```bash
dotnet test TourBooking.sln
```
