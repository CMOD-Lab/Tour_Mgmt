using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for the Booking details page.
/// </summary>
[Authorize]
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets the booking details to display.</summary>
    public BookingDetailsViewModel? Booking { get; private set; }

    /// <summary>Initializes a new instance of DetailsModel.</summary>
    public DetailsModel(IBookingService bookingService, ILogger<DetailsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the booking details page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

            // Manual mapping from DTO to ViewModel
            Booking = new BookingDetailsViewModel
            {
                BookingId = dto.BookingId,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                TourId = dto.TourId,
                BookingDate = dto.BookingDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for ID {BookingId}", id);
            return RedirectToPage("Index");
        }
    }
}
