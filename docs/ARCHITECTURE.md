# Architecture Documentation

## Clean Architecture Overview

The Tour Management System follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────┐
│                    Web Layer                             │
│         (TourManagement.Web)                            │
│   Razor Pages, ViewModels, Program.cs, Static Files     │
├─────────────────────────────────────────────────────────┤
│                 Application Layer                        │
│         (TourManagement.Application)                    │
│   Services, DTOs, AutoMapper Profiles, Validators       │
├─────────────────────────────────────────────────────────┤
│               Infrastructure Layer                       │
│         (TourManagement.Infrastructure)                 │
│   EF Core DbContext, Repositories, Configurations       │
├─────────────────────────────────────────────────────────┤
│                  Domain Layer                            │
│           (TourManagement.Domain)                       │
│   Entities, Repository Interfaces, Service Interfaces   │
└─────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer
- **Entities**: `UserInfo`, `Tour`, `Booking`
- **Repository Interfaces**: `IUserRepository`, `ITourRepository`, `IBookingRepository`
- **Service Interfaces**: `IUserService`, `ITourService`, `IBookingService`
- **Exceptions**: `NotFoundException`, `DuplicateEntityException`, `ValidationException`
- **No dependencies** on other layers

### Application Layer
- **Services**: Business logic implementations
- **DTOs**: Data Transfer Objects for each entity (Read, Create, Update)
- **Mappings**: AutoMapper profiles (Entity ↔ DTO only)
- **Validators**: FluentValidation validators
- **Depends on**: Domain layer only

### Infrastructure Layer
- **DbContext**: `TourManagementDbContext` with EF Core 8
- **Repositories**: EF Core implementations of domain interfaces
- **Configurations**: Entity type configurations (table mappings, constraints)
- **Depends on**: Domain and Application layers

### Web Layer
- **Razor Pages**: UI pages with page models
- **ViewModels**: UI-specific models (manually mapped from/to DTOs)
- **Program.cs**: Application startup, DI registration, middleware pipeline
- **Static Files**: CSS, JavaScript, images
- **Depends on**: Application and Infrastructure layers

## Dependency Flow

```
Web → Application → Domain ← Infrastructure
```

## Key Design Decisions

1. **Manual ViewModel Mapping**: ViewModels in the Web layer are manually mapped to/from DTOs (no AutoMapper in Web layer)
2. **Session-based Auth**: Simple session management for user authentication
3. **Repository Pattern**: All data access through repository interfaces
4. **Async/Await**: All I/O operations are asynchronous
5. **Dependency Injection**: All dependencies injected via constructor
