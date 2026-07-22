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

    /// <summary>Gets or sets the booking input.</summary>
    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    /// <summary>Gets the selected tour details.</summary>
    public TourDto? SelectedTour { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="CreateModel"/>.
    /// </summary>
    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the create booking page.
    /// </summary>
    public async Task OnGetAsync(int? tourId = null)
    {
        // Pre-fill user info from session
        var email = HttpContext.Session.GetString("UserEmail");
        var userName = HttpContext.Session.GetString("UserName");

        if (!string.IsNullOrEmpty(email))
            Input.Email = email;

        if (!string.IsNullOrEmpty(userName))
            Input.FirstName = userName.Split(' ')[0];

        if (tourId.HasValue)
        {
            try
            {
                SelectedTour = await _tourService.GetByIdAsync(tourId.Value);
                if (SelectedTour != null)
                {
                    Input.TourId = SelectedTour.Id;
                    Input.TourName = SelectedTour.TourName;
                    Input.Place = SelectedTour.Place;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tour for booking, id {TourId}", tourId);
            }
        }
    }

    /// <summary>
    /// Handles POST requests to create a new booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Map ViewModel to DTO manually
            var dto = new BookingCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                TourId = Input.TourId,
                UserId = HttpContext.Session.GetInt32("UserId")
            };

            await _bookingService.CreateAsync(dto);
            TempData["SuccessMessage"] = $"Booking for '{Input.TourName}' was confirmed successfully!";
            return RedirectToPage("MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Input.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking. Please try again.");
            return Page();
        }
    }
}
