using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking (Order page).
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the booking creation view model.</summary>
    [BindProperty]
    public BookingCreateViewModel Booking { get; set; } = new();

    /// <summary>Initializes a new instance of CreateModel.</summary>
    public CreateModel(IBookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the create booking page.</summary>
    public IActionResult OnGet(int? tourId, string? tourName)
    {
        Booking.TourId = tourId;
        Booking.TourName = tourName ?? string.Empty;

        // Pre-fill email if user is logged in
        if (User.Identity?.IsAuthenticated == true)
        {
            Booking.Email = User.Identity.Name ?? string.Empty;
        }

        return Page();
    }

    /// <summary>Handles POST requests to create a new booking.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Manual mapping from ViewModel to DTO
            var createDto = new BookingCreateDto
            {
                TourName = Booking.TourName,
                Place = Booking.Place,
                Email = Booking.Email,
                FirstName = Booking.FirstName,
                TourId = Booking.TourId
            };

            await _bookingService.CreateAsync(createDto, cancellationToken);
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Booking.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking.");
            return Page();
        }
    }
}
