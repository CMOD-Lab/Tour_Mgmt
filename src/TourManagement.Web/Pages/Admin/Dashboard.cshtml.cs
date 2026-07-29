using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard.
/// </summary>
public class DashboardModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly ILogger<DashboardModel> _logger;

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

    public int TourCount { get; set; }
    public int UserCount { get; set; }
    public int BookingCount { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/AdminLogin");

        try
        {
            var tours = await _tourService.GetAllToursAsync(cancellationToken);
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            var bookings = await _bookingService.GetAllBookingsAsync(cancellationToken);

            TourCount = tours.Count();
            UserCount = users.Count();
            BookingCount = bookings.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
        }

        return Page();
    }
}
