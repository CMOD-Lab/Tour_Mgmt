# Tour_Management — .NET 8 Migration

## Overview

Tour_Management is an ASP.NET Web Forms 4.7.2 application for managing tour packages, user registrations, and bookings. This repository contains the **migration analysis** for upgrading to .NET 8 with clean architecture.

## Current State

| Property | Value |
|---|---|
| Framework | ASP.NET Web Forms 4.7.2 |
| Pages | 11 Web Forms pages (.aspx) |
| Data Access | Raw ADO.NET |
| Authentication | Custom (hardcoded + raw SQL) |
| Configuration | Web.config |
| Compatibility Score | 18 / 100 |

## Migration Target

| Property | Value |
|---|---|
| Framework | .NET 8 (net8.0) |
| UI | Razor Pages |
| Data Access | Entity Framework Core 8.0.0 |
| Authentication | ASP.NET Core Identity |
| Configuration | appsettings.json |
| Architecture | Clean Architecture (4 layers) |

## Migration Complexity: **COMPLEX**

- **Total Issues:** 42
- **Critical:** 12
- **High:** 14
- **Medium:** 10
- **Low:** 6
- **Estimated Effort:** 80–120 hours

## Critical Issues Summary

1. System.Web dependencies (all 11 files)
2. Web Forms page lifecycle (all 11 pages)
3. Raw ADO.NET without async/await
4. SQL injection vulnerability (userlogin.aspx.cs)
5. Plaintext password storage
6. Hardcoded admin credentials
7. Web.config incompatibility
8. Non-SDK-style .csproj
9. System.Web.DataVisualization (chart control)
10. ConfigurationManager usage
11. Server.MapPath() usage
12. Response.Write() for user feedback

## Documentation

- [Migration Analysis](docs/MIGRATION_ANALYSIS.md) — Full issue list with file locations and code snippets
- [Migration Notes](docs/MIGRATION_NOTES.md) — What changed and why
- [Architecture](docs/ARCHITECTURE.md) — Target clean architecture design
- [Build Verification](docs/BUILD_VERIFICATION.md) — Build process and verification checklist

## Application Features

- **Tour Management:** Add, edit, delete, display tour packages with images
- **User Registration:** Sign up with email, name, gender, DOB, address
- **User Login:** Email/password authentication
- **Tour Booking:** Book tours, view bookings, cancel bookings
- **Admin Panel:** Manage tours, view all bookings, manage users

## Setup Instructions (Post-Migration)

```bash
# Clone repository
git clone <repository-url>
cd Tour_Management

# Restore packages
dotnet restore

# Update connection string in appsettings.json
# "DefaultConnection": "Server=...;Database=TourManagementDb;..."

# Apply EF Core migrations
dotnet ef database update --project src/TourManagement.Infrastructure --startup-project src/TourManagement.Web

# Run application
dotnet run --project src/TourManagement.Web
```

## Testing

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/TourManagement.UnitTests

# Run integration tests only
dotnet test tests/TourManagement.IntegrationTests
```

## Migration Roadmap

| Phase | Description | Effort |
|---|---|---|
| Phase 1 | Foundation (solution structure, EF Core, Identity) | 20 hours |
| Phase 2 | Data Access Layer (repositories, services, DTOs) | 15 hours |
| Phase 3 | Security (Identity, authorization, password hashing) | 15 hours |
| Phase 4 | UI Migration (11 pages → Razor Pages) | 40 hours |
| Phase 5 | Testing & Documentation | 20 hours |
| **Total** | | **80–120 hours** |
