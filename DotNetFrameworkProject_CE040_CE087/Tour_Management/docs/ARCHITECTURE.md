# Architecture Documentation

## Clean Architecture Overview

The Tour Management application follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────┐
│                    Web Layer (UI)                        │
│  Razor Pages, ViewModels, Program.cs, Static Files      │
├─────────────────────────────────────────────────────────┤
│                 Application Layer                        │
│  Services, DTOs, AutoMapper Profiles, Validators        │
├─────────────────────────────────────────────────────────┤
│                Infrastructure Layer                      │
│  EF Core DbContext, Repositories, Configurations        │
├─────────────────────────────────────────────────────────┤
│                   Domain Layer                           │
│  Entities, Interfaces, DTOs, Exceptions                 │
└─────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer (`Tour_Management.Domain`)
- **Entities**: `Tour`, `UserInfo`, `Booking`
- **Interfaces**: `ITourRepository`, `IUserRepository`, `IBookingRepository`, `ITourService`, `IUserService`, `IBookingService`
- **DTOs**: `TourDto`, `UserDto`, `BookingDto` (and Create/Update variants)
- **Exceptions**: `NotFoundException`, `ValidationException`, `DuplicateEntityException`

### Application Layer (`Tour_Management.Application`)
- **Services**: `TourService`, `UserService`, `BookingService`
- **Mappings**: `MappingProfile` (AutoMapper - entities ↔ DTOs only)
- **Validators**: FluentValidation validators for all DTOs
- **Extensions**: `ServiceCollectionExtensions` for DI registration

### Infrastructure Layer (`Tour_Management.Infrastructure`)
- **DbContext**: `TourManagementDbContext`
- **Configurations**: EF Core `IEntityTypeConfiguration<T>` for each entity
- **Repositories**: `TourRepository`, `UserRepository`, `BookingRepository`
- **Extensions**: `ServiceCollectionExtensions` for DI registration

### Web Layer (`Tour_Management.Web`)
- **Pages**: Razor Pages for all CRUD operations
- **ViewModels**: Manually mapped from/to DTOs (no AutoMapper in Web layer)
- **Program.cs**: Application startup, middleware, DI configuration
- **Static Files**: CSS, JavaScript, images

## Dependency Flow

```
Web → Application → Domain ← Infrastructure
```

- Web depends on Application and Infrastructure (for DI setup)
- Application depends on Domain
- Infrastructure depends on Domain and Application
- Domain has no dependencies

## Key Design Decisions

1. **Manual ViewModel Mapping**: ViewModels in the Web layer are manually mapped to/from DTOs to maintain clear separation
2. **AutoMapper only in Application**: AutoMapper is used only for entity ↔ DTO mapping
3. **Soft Delete**: Entities are soft-deleted (IsActive = false) rather than hard-deleted
4. **Async/Await**: All I/O operations use async/await with CancellationToken
5. **Repository Pattern**: Abstracts data access behind interfaces
6. **Session-based Auth**: Simple session-based authentication (upgrade to Identity recommended)
