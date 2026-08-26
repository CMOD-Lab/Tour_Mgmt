using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin all bookings page.
/// </summary>
public class BookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingsModel> _logger;

    /// <summary>Gets or sets the list of bookings.</summary>
    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    /// <summary>Initializes a new instance of the <see cref="BookingsModel"/> class.</summary>
    public BookingsModel(IBookingService bookingService, ILogger<BookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin bookings page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var bookings = await _bookingService.GetAllAsync(cancellationToken);

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
            _logger.LogError(ex, "Error loading admin bookings");
            return Page();
        }
    }
}
