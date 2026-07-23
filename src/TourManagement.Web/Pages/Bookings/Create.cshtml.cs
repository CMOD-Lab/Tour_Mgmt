using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking (Order page).
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<CreateModel> _logger;

    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IBookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IActionResult OnGet(int? tourId, string? tourName, string? place)
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToPage("/Users/Login");
        }

        // Pre-fill from query parameters
        Input.TourId = tourId;
        Input.TourName = tourName ?? string.Empty;
        Input.Place = place ?? string.Empty;
        Input.Email = HttpContext.Session.GetString("UserEmail") ?? string.Empty;
        Input.FirstName = HttpContext.Session.GetString("UserFirstName") ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToPage("/Users/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manually map ViewModel to domain entity
            var booking = new Booking
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                TourId = Input.TourId
            };

            await _bookingService.CreateAsync(booking, cancellationToken);
            _logger.LogInformation("Booking created for tour {TourName} by {Email}", Input.TourName, Input.Email);
            return RedirectToPage("/Bookings/MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour {TourName}", Input.TourName);
            ErrorMessage = "An error occurred while creating the booking. Please try again.";
            return Page();
        }
    }
}
