using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for creating a new booking.
/// </summary>
public class CreateModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(BookingService bookingService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [BindProperty]
    public BookingCreateViewModel Input { get; set; } = new();

    public IActionResult OnGet(int? tourId, string? tourName, string? place)
    {
        // Pre-fill from query parameters
        Input.TourId = tourId;
        Input.TourName = tourName ?? string.Empty;
        Input.Place = place ?? string.Empty;

        // Pre-fill user info from session if logged in
        var userEmail = HttpContext.Session.GetString("UserEmail");
        var userName = HttpContext.Session.GetString("UserName");
        if (!string.IsNullOrEmpty(userEmail))
        {
            Input.Email = userEmail;
            Input.FirstName = userName?.Split(' ').FirstOrDefault() ?? string.Empty;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Manual mapping from ViewModel to DTO
            var dto = new BookingCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                TourId = Input.TourId
            };

            await _bookingService.CreateAsync(dto, cancellationToken);
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToPage("./MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", Input.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while processing your booking. Please try again.");
            return Page();
        }
    }
}
