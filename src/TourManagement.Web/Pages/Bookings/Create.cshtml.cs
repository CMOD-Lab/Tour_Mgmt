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

    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? tourId, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        Input.Email = email;
        Input.FirstName = HttpContext.Session.GetString("UserName") ?? string.Empty;

        if (tourId.HasValue)
        {
            var tour = await _tourService.GetTourByIdAsync(tourId.Value, cancellationToken);
            if (tour != null)
            {
                Input.TourId = tour.TourId;
                Input.TourName = tour.TourName;
                Input.Place = tour.Place;
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
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
                FirstName = Input.FirstName,
                TourId = Input.TourId
            };

            await _bookingService.CreateBookingAsync(booking, cancellationToken);
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToPage("./MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for user: {Email}", email);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking.");
            return Page();
        }
    }
}
