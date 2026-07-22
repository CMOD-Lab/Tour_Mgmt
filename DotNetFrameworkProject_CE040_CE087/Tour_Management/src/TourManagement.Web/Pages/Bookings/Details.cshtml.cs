using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for booking details.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets the booking details.</summary>
    public BookingDto? Booking { get; private set; }

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
    public async Task OnGetAsync(int id)
    {
        try
        {
            Booking = await _bookingService.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for id {BookingId}", id);
        }
    }
}
