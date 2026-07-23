using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for viewing a user's own bookings.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    /// <summary>Gets the list of bookings for the user.</summary>
    public IEnumerable<Booking> Bookings { get; private set; } = Enumerable.Empty<Booking>();

    /// <summary>
    /// Initializes a new instance of the <see cref="MyBookingsModel"/> class.
    /// </summary>
    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the my bookings page.</summary>
    public async Task OnGetAsync(string? email = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var userEmail = email ?? User.Identity?.Name;
            if (!string.IsNullOrEmpty(userEmail))
            {
                Bookings = await _bookingService.GetByEmailAsync(userEmail, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading my bookings page");
            Bookings = Enumerable.Empty<Booking>();
        }
    }
}
