using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
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

    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int? tourId = null, CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Account/Login");
        }

        // Pre-fill user email and first name from session
        Input.Email = email;
        Input.FirstName = HttpContext.Session.GetString("UserFirstName") ?? string.Empty;

        // Pre-fill tour details if tourId provided
        if (tourId.HasValue)
        {
            try
            {
                var tour = await _tourService.GetByIdAsync(tourId.Value, cancellationToken);
                if (tour is not null)
                {
                    Input.TourId = tour.TourId;
                    Input.TourName = tour.TourName;
                    Input.Place = tour.Place;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tour {TourId} for booking.", tourId);
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Account/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var createDto = new BookingCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                TourId = Input.TourId
            };

            await _bookingService.CreateAsync(createDto, cancellationToken);
            _logger.LogInformation("Booking created for user {Email} on tour {TourName}.", Input.Email, Input.TourName);
            return RedirectToPage("/Bookings/MyBookings", new { message = "Booking confirmed successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for user {Email}.", Input.Email);
            ErrorMessage = "An error occurred while creating your booking. Please try again.";
            return Page();
        }
    }
}
