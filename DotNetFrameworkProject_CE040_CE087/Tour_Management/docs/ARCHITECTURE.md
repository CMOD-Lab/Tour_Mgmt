# Architecture — Tour_Management .NET 8

## Overview

The migrated Tour_Management application follows **Clean Architecture** principles with four distinct layers, ensuring separation of concerns, testability, and maintainability.

## Layer Responsibilities

### 1. Domain Layer (`TourManagement.Domain`)
- **Purpose:** Core business entities and interfaces
- **Dependencies:** None (no external dependencies)
- **Contents:**
  - `Entities/Tour.cs` — Tour entity
  - `Entities/UserInfo.cs` — User entity
  - `Entities/Booking.cs` — Booking entity
  - `Interfaces/Repositories/ITourRepository.cs`
  - `Interfaces/Repositories/IUserRepository.cs`
  - `Interfaces/Repositories/IBookingRepository.cs`
  - `Interfaces/Services/ITourService.cs`
  - `Interfaces/Services/IUserService.cs`
  - `Interfaces/Services/IBookingService.cs`
  - `Exceptions/NotFoundException.cs`
  - `Exceptions/ValidationException.cs`

### 2. Application Layer (`TourManagement.Application`)
- **Purpose:** Business logic implementation, DTOs, validation
- **Dependencies:** Domain layer only
- **Contents:**
  - `Services/TourService.cs`
  - `Services/UserService.cs`
  - `Services/BookingService.cs`
  - `DTOs/TourDto.cs`, `TourCreateDto.cs`, `TourUpdateDto.cs`
  - `DTOs/UserDto.cs`, `UserCreateDto.cs`
  - `DTOs/BookingDto.cs`, `BookingCreateDto.cs`
  - `Mappings/MappingProfile.cs` (AutoMapper)
  - `Validators/TourCreateValidator.cs` (FluentValidation)
  - `Validators/UserCreateValidator.cs`
  - `Extensions/ServiceCollectionExtensions.cs`

### 3. Infrastructure Layer (`TourManagement.Infrastructure`)
- **Purpose:** Data access, EF Core, external services
- **Dependencies:** Domain + Application layers
- **Contents:**
  - `Data/TourDbContext.cs`
  - `Data/Configurations/TourConfiguration.cs`
  - `Data/Configurations/UserInfoConfiguration.cs`
  - `Data/Configurations/BookingConfiguration.cs`
  - `Data/Migrations/` (EF Core migrations)
  - `Repositories/TourRepository.cs`
  - `Repositories/UserRepository.cs`
  - `Repositories/BookingRepository.cs`
  - `Extensions/ServiceCollectionExtensions.cs`

### 4. Web Layer (`TourManagement.Web`)
- **Purpose:** UI, Razor Pages, ViewModels, static files
- **Dependencies:** Infrastructure + Application layers
- **Contents:**
  - `Pages/Tours/Index.cshtml` — Display all tours
  - `Pages/Tours/Create.cshtml` — Add new tour
  - `Pages/Tours/Edit.cshtml` — Edit tour
  - `Pages/Tours/Details.cshtml` — Tour details
  - `Pages/Tours/Delete.cshtml` — Delete tour
  - `Pages/Bookings/Index.cshtml` — All bookings (admin)
  - `Pages/Bookings/MyBookings.cshtml` — User's bookings
  - `Pages/Bookings/Create.cshtml` — Book a tour
  - `Pages/Account/Login.cshtml` — User login
  - `Pages/Account/Register.cshtml` — User registration
  - `Pages/Admin/Index.cshtml` — Admin dashboard
  - `Pages/Admin/Users.cshtml` — User management
  - `Pages/Shared/_Layout.cshtml` — Main layout
  - `Pages/Shared/_AdminLayout.cshtml` — Admin layout
  - `ViewModels/TourViewModel.cs`
  - `ViewModels/BookingViewModel.cs`
  - `wwwroot/css/site.css`
  - `wwwroot/js/site.js`
  - `Program.cs`
  - `appsettings.json`

## Dependency Flow

```
Web → Application → Domain
Web → Infrastructure → Domain
Infrastructure → Application → Domain
```

**Rule:** Dependencies only flow inward. Domain has no dependencies. Application depends only on Domain. Infrastructure depends on Domain and Application. Web depends on all layers.

## Key Design Patterns

### Repository Pattern
```csharp
// Domain interface
public interface ITourRepository
{
    Task<IEnumerable<Tour>> GetAllAsync(CancellationToken ct = default);
    Task<Tour?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Tour> AddAsync(Tour tour, CancellationToken ct = default);
    Task UpdateAsync(Tour tour, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

// Infrastructure implementation
public class TourRepository : ITourRepository
{
    private readonly TourDbContext _context;
    public TourRepository(TourDbContext context) => _context = context;
    
    public async Task<IEnumerable<Tour>> GetAllAsync(CancellationToken ct = default)
        => await _context.Tours.AsNoTracking().Where(t => t.IsActive).ToListAsync(ct);
}
```

### Service Layer
```csharp
public class TourService : ITourService
{
    private readonly ITourRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TourService> _logger;
    
    public async Task<IEnumerable<TourDto>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var tours = await _repository.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tours");
            throw;
        }
    }
}
```

### ViewModel Manual Mapping (Web Layer)
```csharp
// ViewModels are manually mapped in PageModel handlers
public class CreateModel : PageModel
{
    [BindProperty]
    public TourCreateViewModel Input { get; set; } = new();
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        
        // Manual mapping: ViewModel → DTO
        var dto = new TourCreateDto
        {
            TourName = Input.TourName,
            Place = Input.Place,
            Days = Input.Days,
            Price = Input.Price,
            Locations = Input.Locations,
            TourInfo = Input.TourInfo
        };
        
        await _tourService.CreateAsync(dto);
        TempData["Success"] = "Tour added successfully.";
        return RedirectToPage("./Index");
    }
}
```

## Authentication Architecture

ASP.NET Core Identity is used for authentication and authorization:

```csharp
// Program.cs
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<TourDbContext>()
.AddDefaultTokenProviders();

// Protect admin pages
[Authorize(Roles = "Admin")]
public class AdminIndexModel : PageModel { ... }

// Protect user pages
[Authorize]
public class MyBookingsModel : PageModel { ... }
```

## Database Schema (EF Core)

```csharp
public class Tour
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string TourInfo { get; set; } = string.Empty;
    public string? Pic { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ICollection<Booking> Bookings { get; set; } = [];
}

public class Booking
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public Tour Tour { get; set; } = null!;
}
```
