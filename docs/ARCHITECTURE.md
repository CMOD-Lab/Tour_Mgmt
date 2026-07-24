# Architecture Documentation

## Clean Architecture Overview

```
TourManagement.sln
├── src/
│   ├── TourManagement.Domain          # Core business entities and interfaces
│   ├── TourManagement.Application     # Business logic and services
│   ├── TourManagement.Infrastructure  # Data access and EF Core
│   └── TourManagement.Web             # Razor Pages UI
└── tests/
    ├── TourManagement.UnitTests
    └── TourManagement.IntegrationTests
```

## Layer Responsibilities

### Domain Layer
- Contains domain entities: `UserInfo`, `Tour`, `Booking`
- Defines repository interfaces: `IUserRepository`, `ITourRepository`, `IBookingRepository`
- Defines service interfaces: `IUserService`, `ITourService`, `IBookingService`
- Contains domain exceptions: `NotFoundException`
- No dependencies on other layers

### Application Layer
- Implements business logic in services: `UserService`, `TourService`, `BookingService`
- Contains DTOs for data transfer between layers
- AutoMapper profiles for entity-to-DTO mapping
- FluentValidation validators
- Depends only on Domain layer

### Infrastructure Layer
- EF Core `TourManagementDbContext`
- Entity configurations (table mappings, constraints)
- Repository implementations
- Depends on Domain and Application layers

### Web Layer
- Razor Pages for UI
- ViewModels (manually mapped from DTOs)
- Program.cs for application startup and DI configuration
- Static files (CSS, JS)
- Depends on Application and Infrastructure layers

## Dependency Flow
```
Web → Application → Domain
Web → Infrastructure → Domain
Infrastructure → Application
```

## Key Patterns Used
- Repository Pattern
- Service Layer Pattern
- DTO Pattern
- Clean Architecture
- Dependency Injection
- Async/Await throughout
