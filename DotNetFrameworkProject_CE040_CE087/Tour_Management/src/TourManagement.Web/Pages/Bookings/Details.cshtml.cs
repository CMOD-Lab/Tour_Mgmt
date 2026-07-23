using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for viewing booking details.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets or sets the booking view model.</summary>
    public BookingViewModel? Booking { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DetailsModel"/>.
    /// </summary>
    public DetailsModel(IBookingService bookingService, ILogger<DetailsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the booking details page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            Booking = new BookingViewModel
            {
                Id = dto.Id,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                BookingDate = dto.BookingDate,
                TourId = dto.TourId,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for id {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading booking details.";
            return RedirectToPage("Index");
        }
    }
}
