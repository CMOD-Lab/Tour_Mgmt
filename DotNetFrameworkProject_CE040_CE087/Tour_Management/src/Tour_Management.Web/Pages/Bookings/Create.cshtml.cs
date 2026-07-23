using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>Page model for creating a new booking.</summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IBookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [BindProperty]
    public BookingCreateViewModel Booking { get; set; } = new();

    public IActionResult OnGet(int? tourId, string? tourName, string? place)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            return RedirectToPage("/Users/Login");
        }

        // Pre-fill from query parameters
        Booking.TourId = tourId;
        Booking.TourName = tourName ?? string.Empty;
        Booking.Place = place ?? string.Empty;

        // Pre-fill user email from session
        var email = HttpContext.Session.GetString("UserEmail");
        if (!string.IsNullOrEmpty(email))
        {
            Booking.Email = email;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            return RedirectToPage("/Users/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var createDto = Booking.ToCreateDto();
            await _bookingService.CreateAsync(createDto, cancellationToken);

            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToPage("./MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Booking.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking. Please try again.");
            return Page();
        }
    }
}
