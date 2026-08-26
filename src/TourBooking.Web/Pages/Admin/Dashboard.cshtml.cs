using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard page.
/// </summary>
public class DashboardModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly ILogger<DashboardModel> _logger;

    /// <summary>Gets or sets the total tour count.</summary>
    public int TourCount { get; set; }

    /// <summary>Gets or sets the total user count.</summary>
    public int UserCount { get; set; }

    /// <summary>Gets or sets the total booking count.</summary>
    public int BookingCount { get; set; }

    /// <summary>Initializes a new instance of the <see cref="DashboardModel"/> class.</summary>
    public DashboardModel(
        ITourService tourService,
        IUserService userService,
        IBookingService bookingService,
        ILogger<DashboardModel> logger)
    {
        _tourService = tourService;
        _userService = userService;
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin dashboard page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var tours = await _tourService.GetAllAsync(cancellationToken);
            var users = await _userService.GetAllAsync(cancellationToken);
            var bookings = await _bookingService.GetAllAsync(cancellationToken);

            TourCount = tours.Count();
            UserCount = users.Count();
            BookingCount = bookings.Count();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            return Page();
        }
    }
}
