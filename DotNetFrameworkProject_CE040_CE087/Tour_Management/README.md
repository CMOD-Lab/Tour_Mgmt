# Tour Management - .NET 8 Migration

## Overview
This is a Tour Management web application migrated from ASP.NET Web Forms (.NET 4.7.2) to .NET 8 using clean architecture principles with Razor Pages.

## Architecture
The solution follows Clean Architecture with four layers:

- **TourManagement.Domain** - Domain entities, interfaces, and exceptions
- **TourManagement.Application** - Business logic, services, DTOs, validators, AutoMapper profiles
- **TourManagement.Infrastructure** - EF Core DbContext, repositories, data configurations
- **TourManagement.Web** - Razor Pages UI, ViewModels, static files

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Setup

1. Clone the repository
2. Update the connection string in `src/TourManagement.Web/appsettings.json`
3. Run the application - the database will be created automatically on first run

```bash
cd src/TourManagement.Web
dotnet run
```

## Default Admin Credentials
- Email: `admin@gmail.com`
- Password: `admin`

## Features
- **Tours**: Browse, search, view details, book tours (admin: add, edit, delete)
- **Users**: Register, login, view/edit profile
- **Bookings**: Create bookings, view my bookings, cancel bookings (admin: view all, edit, delete)
- **Admin Dashboard**: Statistics overview, quick actions

## Running Tests
```bash
dotnet test tests/TourManagement.UnitTests/TourManagement.UnitTests.csproj
```

## Building
```bash
dotnet build TourManagement.sln
```
