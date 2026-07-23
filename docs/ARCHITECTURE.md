# Architecture Documentation

## Clean Architecture Overview

The Tour Management System follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────┐
│                    Web Layer (UI)                        │
│              TourManagement.Web                          │
│         Razor Pages, ViewModels, Static Files            │
├─────────────────────────────────────────────────────────┤
│                 Application Layer                        │
│            TourManagement.Application                    │
│         Services, DTOs, Validators, AutoMapper           │
├─────────────────────────────────────────────────────────┤
│                Infrastructure Layer                      │
│           TourManagement.Infrastructure                  │
│         EF Core DbContext, Repositories, Configs         │
├─────────────────────────────────────────────────────────┤
│                   Domain Layer                           │
│              TourManagement.Domain                       │
│         Entities, Interfaces, DTOs, Exceptions           │
└─────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer
- **Entities**: `UserInfo`, `Tour`, `Booking`
- **Interfaces**: Repository and Service contracts
- **DTOs**: Data transfer objects for cross-layer communication
- **Exceptions**: Domain-specific exceptions

### Application Layer
- **Services**: Business logic implementation (`UserService`, `TourService`, `BookingService`)
- **Mappings**: AutoMapper profiles for Entity ↔ DTO mapping
- **Validators**: FluentValidation validators for DTOs
- **Extensions**: DI registration extensions

### Infrastructure Layer
- **DbContext**: `TourManagementDbContext` with EF Core
- **Configurations**: Entity type configurations
- **Repositories**: EF Core repository implementations
- **Extensions**: DI registration extensions

### Web Layer
- **Pages**: Razor Pages for all UI screens
- **ViewModels**: UI-specific view models (manually mapped from DTOs)
- **wwwroot**: Static files (CSS, JS, images)
- **Program.cs**: Application startup and DI configuration

## Dependency Flow
```
Web → Application → Domain ← Infrastructure
```

## Key Design Decisions

1. **Manual ViewModel Mapping**: ViewModels in Web layer are manually mapped to/from DTOs (no AutoMapper in Web layer)
2. **Repository Pattern**: All data access through repository interfaces
3. **Service Layer**: All business logic in Application services
4. **Session Authentication**: Simple session-based auth (can be upgraded to Identity)
5. **BCrypt Passwords**: Secure password hashing
