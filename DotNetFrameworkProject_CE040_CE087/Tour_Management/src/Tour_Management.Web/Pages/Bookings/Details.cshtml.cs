using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for displaying booking details.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets or sets the booking details to display.</summary>
    public BookingDetailsViewModel? Booking { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailsModel"/> class.
    /// </summary>
    public DetailsModel(IBookingService bookingService, ILogger<DetailsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the booking details page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

            Booking = new BookingDetailsViewModel
            {
                BookingId = dto.BookingId,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for id {BookingId}", id);
            TempData["Error"] = "An error occurred while loading booking details.";
            return RedirectToPage("./MyBookings");
        }
    }
}
