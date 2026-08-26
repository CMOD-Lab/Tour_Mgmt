# Architecture Documentation

## Clean Architecture Overview

The Tour Booking application follows Clean Architecture principles with four distinct layers:

```
TourBooking.sln
├── src/
│   ├── TourBooking.Domain          # Core business entities and interfaces
│   ├── TourBooking.Application     # Business logic and use cases
│   ├── TourBooking.Infrastructure  # Data access and external services
│   └── TourBooking.Web             # UI layer (Razor Pages)
└── tests/
    ├── TourBooking.UnitTests       # Unit tests for services
    └── TourBooking.IntegrationTests # Integration tests for repositories
```

## Layer Responsibilities

### Domain Layer (TourBooking.Domain)
- **Entities**: `UserInfo`, `Tour`, `Booking`
- **Interfaces**: Repository and service contracts
- **Exceptions**: Domain-specific exceptions (`NotFoundException`, `DuplicateEntityException`)
- **No dependencies** on other layers

### Application Layer (TourBooking.Application)
- **Services**: Business logic implementations (`UserService`, `TourService`, `BookingService`)
- **DTOs**: Data transfer objects for each entity
- **Mappings**: AutoMapper profiles
- **Depends on**: Domain layer only

### Infrastructure Layer (TourBooking.Infrastructure)
- **DbContext**: `TourBookingDbContext` with EF Core 8.0.0
- **Repositories**: EF Core implementations of domain repository interfaces
- **Configurations**: Entity type configurations
- **Depends on**: Domain and Application layers

### Web Layer (TourBooking.Web)
- **Razor Pages**: UI pages with page models
- **ViewModels**: UI-specific view models (manually mapped from DTOs)
- **Program.cs**: Application startup and DI configuration
- **Depends on**: Application and Infrastructure layers

## Dependency Flow
```
Web → Application → Domain
Web → Infrastructure → Domain
Infrastructure → Application → Domain
```

## Design Patterns Used
- **Repository Pattern**: Abstracts data access
- **Service Layer Pattern**: Encapsulates business logic
- **Dependency Injection**: All dependencies injected via constructor
- **DTO Pattern**: Separates domain from presentation
- **ViewModel Pattern**: UI-specific models in Web layer
