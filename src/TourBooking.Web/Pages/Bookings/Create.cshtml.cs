using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Bookings;

/// <summary>
/// Page model for the create booking page.
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the booking input model.</summary>
    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    /// <summary>Gets or sets the success message.</summary>
    public string SuccessMessage { get; set; } = string.Empty;

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="CreateModel"/> class.</summary>
    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the create booking page.</summary>
    public async Task<IActionResult> OnGetAsync(int? tourId, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        // Pre-fill email from session
        Input.Email = email;
        Input.FirstName = HttpContext.Session.GetString("UserFirstName") ?? string.Empty;

        // Pre-fill tour details if tourId provided
        if (tourId.HasValue)
        {
            var tour = await _tourService.GetByIdAsync(tourId.Value, cancellationToken);
            if (tour != null)
            {
                Input.TourName = tour.TourName;
                Input.Place = tour.Place;
            }
        }

        return Page();
    }

    /// <summary>Handles POST requests for the create booking form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
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
                FirstName = Input.FirstName
            };

            await _bookingService.CreateAsync(booking, cancellationToken);

            _logger.LogInformation("Booking created for tour: {TourName} by {Email}", Input.TourName, Input.Email);
            return RedirectToPage("/Bookings/MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Input.TourName);
            ErrorMessage = "An error occurred while creating the booking. Please try again.";
            return Page();
        }
    }
}
