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
    private readonly ILogger<CreateModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateModel"/> class.
    /// </summary>
    public CreateModel(IBookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Gets or sets the booking create view model.</summary>
    [BindProperty]
    public BookingCreateViewModel Booking { get; set; } = new();

    /// <summary>
    /// Handles GET requests for the create booking page.
    /// </summary>
    public IActionResult OnGet(int? tourId, string? tourName, string? place)
    {
        // Pre-populate from query string
        Booking.TourId = tourId;
        Booking.TourName = tourName ?? string.Empty;
        Booking.Place = place ?? string.Empty;

        // Pre-populate email from session
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (!string.IsNullOrEmpty(userEmail))
        {
            Booking.Email = userEmail;
        }

        return Page();
    }

    /// <summary>
    /// Handles POST requests to create a new booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manual mapping from ViewModel to Domain entity
            var booking = new Booking
            {
                TourName = Booking.TourName,
                Place = Booking.Place,
                Email = Booking.Email,
                FirstName = Booking.FirstName,
                TourId = Booking.TourId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = HttpContext.Session.GetString("UserEmail") ?? "guest"
            };

            await _bookingService.CreateAsync(booking, cancellationToken);
            _logger.LogInformation("Booking created for tour: {TourName}, email: {Email}", booking.TourName, booking.Email);

            TempData["Message"] = $"Booking for '{booking.TourName}' was confirmed successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking.");
            return Page();
        }
    }
}
