# Architecture Documentation

## Clean Architecture Overview

This solution follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────┐
│                    Web Layer                             │
│         (Razor Pages, ViewModels, Program.cs)           │
├─────────────────────────────────────────────────────────┤
│                 Application Layer                        │
│         (Services, DTOs, Mappings, Validators)          │
├─────────────────────────────────────────────────────────┤
│               Infrastructure Layer                       │
│         (EF Core, Repositories, DbContext)              │
├─────────────────────────────────────────────────────────┤
│                  Domain Layer                            │
│         (Entities, Interfaces, Exceptions)              │
└─────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer (`Tour_Management.Domain`)
- **Entities**: UserInfo, Tour, Booking
- **Interfaces**: Repository and Service contracts
- **Exceptions**: Domain-specific exceptions (NotFoundException, DuplicateEntityException)
- **No dependencies** on other layers

### Application Layer (`Tour_Management.Application`)
- **Services**: Business logic implementation (TourService, UserService, BookingService)
- **DTOs**: Data Transfer Objects for each entity
- **Mappings**: AutoMapper profiles
- **Validators**: FluentValidation validators
- **Depends on**: Domain layer only

### Infrastructure Layer (`Tour_Management.Infrastructure`)
- **DbContext**: TourManagementDbContext
- **Repositories**: EF Core implementations
- **Configurations**: Entity type configurations
- **Depends on**: Domain and Application layers

### Web Layer (`Tour_Management.Web`)
- **Razor Pages**: UI pages with PageModel classes
- **ViewModels**: UI-specific models (manually mapped from DTOs)
- **Program.cs**: Application startup and DI configuration
- **Depends on**: Application and Infrastructure layers

## Dependency Injection

Services are registered via extension methods:
- `AddApplicationServices()` - registers services and AutoMapper
- `AddInfrastructureServices()` - registers DbContext and repositories

## Data Flow

```
User Request → Razor Page → Service → Repository → Database
                         ↓
              ViewModel ← DTO ← Entity
```

## Key Design Decisions

1. **ViewModels are manually mapped** from DTOs in page handlers (not via AutoMapper)
2. **AutoMapper** is used only between Entities and DTOs in the Application layer
3. **Session-based authentication** for simplicity (can be upgraded to Identity)
4. **Soft deletes** via IsActive flag on all entities
5. **Async/await** throughout for scalability
