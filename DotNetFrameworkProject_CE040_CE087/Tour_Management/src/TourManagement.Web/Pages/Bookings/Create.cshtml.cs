using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking (order).
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the booking create view model.</summary>
    [BindProperty]
    public BookingCreateViewModel Booking { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CreateModel"/>.
    /// </summary>
    public CreateModel(IBookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the create booking page.
    /// </summary>
    public void OnGet(int? tourId, string? tourName, string? place)
    {
        Booking.TourId = tourId;
        Booking.TourName = tourName ?? string.Empty;
        Booking.Place = place ?? string.Empty;

        // Pre-fill email from session if logged in
        var sessionEmail = HttpContext.Session.GetString("UserEmail");
        if (!string.IsNullOrEmpty(sessionEmail))
        {
            Booking.Email = sessionEmail;
        }

        var sessionName = HttpContext.Session.GetString("UserName");
        if (!string.IsNullOrEmpty(sessionName))
        {
            Booking.FirstName = sessionName.Split(' ')[0];
        }
    }

    /// <summary>
    /// Handles POST requests to create a new booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var dto = new BookingCreateDto
            {
                TourName = Booking.TourName,
                Place = Booking.Place,
                Email = Booking.Email,
                FirstName = Booking.FirstName,
                TourId = Booking.TourId,
                UserId = HttpContext.Session.GetInt32("UserId"),
                CreatedBy = HttpContext.Session.GetString("UserEmail") ?? "guest"
            };

            await _bookingService.CreateAsync(dto, cancellationToken);
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToPage("MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Booking.TourName);
            TempData["ErrorMessage"] = "An error occurred while creating the booking.";
            return Page();
        }
    }
}
