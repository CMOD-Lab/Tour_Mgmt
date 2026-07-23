using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking.
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? tourId = null)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        // Pre-fill from tour if tourId provided
        if (tourId.HasValue)
        {
            var tour = await _tourService.GetTourByIdAsync(tourId.Value);
            if (tour != null)
            {
                Input.TourName = tour.TourName;
                Input.Place = tour.Place;
            }
        }

        // Pre-fill email from session
        Input.Email = email;
        Input.FirstName = HttpContext.Session.GetString("UserFirstName") ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var booking = new Booking
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName
            };

            await _bookingService.CreateBookingAsync(booking);
            TempData["SuccessMessage"] = $"Booking for '{booking.TourName}' confirmed successfully!";
            return RedirectToPage("MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Input.TourName);
            ErrorMessage = "An error occurred while creating your booking. Please try again.";
            return Page();
        }
    }
}
