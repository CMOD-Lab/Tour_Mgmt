using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking.
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the input view model.</summary>
    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    /// <summary>Gets the selected tour details (if booking from a specific tour).</summary>
    public Tour? SelectedTour { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateModel"/> class.
    /// </summary>
    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the create booking page.</summary>
    public async Task<IActionResult> OnGetAsync(int? tourId = null, CancellationToken cancellationToken = default)
    {
        if (tourId.HasValue)
        {
            SelectedTour = await _tourService.GetByIdAsync(tourId.Value, cancellationToken);
            if (SelectedTour != null)
            {
                Input.TourId = tourId;
                Input.TourName = SelectedTour.TourName;
                Input.Place = SelectedTour.Place;
            }
        }
        return Page();
    }

    /// <summary>Handles POST requests to create a new booking.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            if (Input.TourId.HasValue)
                SelectedTour = await _tourService.GetByIdAsync(Input.TourId.Value, cancellationToken);
            return Page();
        }

        try
        {
            // Map ViewModel to Domain Entity manually
            var booking = new Booking
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                TourId = Input.TourId,
                CreatedBy = User.Identity?.Name ?? "system"
            };

            await _bookingService.CreateAsync(booking, cancellationToken);
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToPage("MyBookings", new { email = Input.Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Input.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking. Please try again.");
            return Page();
        }
    }
}
