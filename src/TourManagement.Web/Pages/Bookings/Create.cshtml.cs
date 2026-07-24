using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>Page model for the create booking page.</summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the booking form input model.</summary>
    [BindProperty]
    public BookingFormViewModel Input { get; set; } = new();

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of <see cref="CreateModel"/>.</summary>
    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the booking creation page.</summary>
    public async Task<IActionResult> OnGetAsync(int? tourId, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToPage("/Users/Login");
        }

        // Pre-fill user info from session
        Input.Email = HttpContext.Session.GetString("UserEmail") ?? string.Empty;
        Input.FirstName = HttpContext.Session.GetString("UserFirstName") ?? string.Empty;

        // Pre-fill tour info if tourId provided
        if (tourId.HasValue)
        {
            try
            {
                var tour = await _tourService.GetByIdAsync(tourId.Value, cancellationToken);
                if (tour != null)
                {
                    Input.TourName = tour.TourName;
                    Input.Place = tour.Place;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tour for booking, ID {TourId}", tourId);
            }
        }

        return Page();
    }

    /// <summary>Handles POST requests for creating a booking.</summary>
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
            // Manual mapping from ViewModel to DTO
            var dto = new BookingCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName
            };

            await _bookingService.CreateAsync(dto, cancellationToken);
            _logger.LogInformation("Booking created for email {Email}, tour {TourName}", Input.Email, Input.TourName);
            return RedirectToPage("/Bookings/MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for email {Email}", Input.Email);
            ErrorMessage = "An error occurred while creating the booking. Please try again.";
            return Page();
        }
    }
}
