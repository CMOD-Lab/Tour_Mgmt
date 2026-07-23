using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking.
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the booking input model.</summary>
    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateModel"/> class.
    /// </summary>
    public CreateModel(IBookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the booking creation page.</summary>
    public IActionResult OnGet(int? tourId, string? tourName, string? place)
    {
        // Pre-fill from query parameters
        Input.TourId = tourId;
        Input.TourName = tourName ?? string.Empty;
        Input.Place = place ?? string.Empty;

        // Pre-fill user info from session
        var email = HttpContext.Session.GetString("UserEmail");
        var firstName = HttpContext.Session.GetString("UserFirstName");
        if (!string.IsNullOrEmpty(email))
            Input.Email = email;
        if (!string.IsNullOrEmpty(firstName))
            Input.FirstName = firstName;

        return Page();
    }

    /// <summary>Handles POST requests for the booking form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var createDto = new BookingCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                TourId = Input.TourId,
                UserId = userId
            };

            await _bookingService.CreateAsync(createDto, cancellationToken);
            _logger.LogInformation("Booking created for tour {TourName} by {Email}", Input.TourName, Input.Email);
            TempData["Success"] = "Booking confirmed successfully!";
            return RedirectToPage("./MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour {TourName}", Input.TourName);
            TempData["Error"] = "An error occurred while creating your booking. Please try again.";
            return Page();
        }
    }
}
