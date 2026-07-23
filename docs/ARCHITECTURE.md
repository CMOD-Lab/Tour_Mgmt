# Architecture Documentation

## Clean Architecture Overview

This solution follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────┐
│                    Web Layer                             │
│         (TourManagement.Web)                            │
│   Razor Pages, ViewModels, Static Files                 │
├─────────────────────────────────────────────────────────┤
│                 Application Layer                        │
│         (TourManagement.Application)                    │
│   Services, DTOs, AutoMapper, FluentValidation          │
├─────────────────────────────────────────────────────────┤
│               Infrastructure Layer                       │
│         (TourManagement.Infrastructure)                 │
│   EF Core DbContext, Repositories, Migrations           │
├─────────────────────────────────────────────────────────┤
│                  Domain Layer                            │
│           (TourManagement.Domain)                       │
│   Entities, Interfaces, Exceptions, Business Rules      │
└─────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer
- Contains domain entities: `UserInfo`, `Tour`, `Booking`
- Defines repository interfaces: `IUserRepository`, `ITourRepository`, `IBookingRepository`
- Defines service interfaces: `IUserService`, `ITourService`, `IBookingService`
- Contains domain exceptions: `NotFoundException`, `DuplicateEntityException`, `ValidationException`
- No dependencies on other layers

### Application Layer
- Implements business logic via services
- Contains DTOs for data transfer
- AutoMapper profiles for entity-to-DTO mapping
- FluentValidation validators
- Depends only on Domain layer

### Infrastructure Layer
- EF Core `TourManagementDbContext`
- Repository implementations
- Entity configurations (IEntityTypeConfiguration)
- Depends on Domain and Application layers

### Web Layer
- ASP.NET Core 8 Razor Pages
- ViewModels (manually mapped from DTOs)
- Static files (CSS, JS)
- DI configuration in `Program.cs`
- Depends on Application and Infrastructure layers

## Dependency Flow
```
Web → Application → Domain
Web → Infrastructure → Domain
Infrastructure → Application → Domain
```

## Key Design Decisions

### Manual ViewModel Mapping
ViewModels in the Web layer are manually mapped to/from DTOs to maintain clear separation between UI concerns and business logic.

### Repository Pattern
Each entity has its own repository interface (Domain) and implementation (Infrastructure), enabling testability and separation of concerns.

### Session-Based Authentication
The application uses ASP.NET Core session middleware for authentication state management, preserving the original application's behavior.
