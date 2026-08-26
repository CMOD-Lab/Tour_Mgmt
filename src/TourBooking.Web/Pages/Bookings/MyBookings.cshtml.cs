using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Bookings;

/// <summary>
/// Page model for the user's bookings page.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    /// <summary>Gets or sets the list of bookings.</summary>
    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    /// <summary>Initializes a new instance of the <see cref="MyBookingsModel"/> class.</summary>
    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the my bookings page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var bookings = await _bookingService.GetByEmailAsync(email, cancellationToken);

            // Manually map domain entities to ViewModels
            Bookings = bookings.Select(b => new BookingViewModel
            {
                TourId = b.TourId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName
            });

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for email: {Email}", email);
            return Page();
        }
    }
}
