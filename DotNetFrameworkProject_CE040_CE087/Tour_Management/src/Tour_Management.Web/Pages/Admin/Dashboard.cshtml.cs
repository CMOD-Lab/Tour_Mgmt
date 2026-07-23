using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard.
/// </summary>
public class DashboardModel : PageModel
{
    private readonly TourService _tourService;
    private readonly UserService _userService;
    private readonly BookingService _bookingService;
    private readonly ILogger<DashboardModel> _logger;

    public DashboardModel(
        TourService tourService,
        UserService userService,
        BookingService bookingService,
        ILogger<DashboardModel> logger)
    {
        _tourService = tourService;
        _userService = userService;
        _bookingService = bookingService;
        _logger = logger;
    }

    public int TotalTours { get; set; }
    public int TotalUsers { get; set; }
    public int TotalBookings { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("./Login");

        try
        {
            var tours = await _tourService.GetAllAsync(cancellationToken);
            var users = await _userService.GetAllAsync(cancellationToken);
            var bookings = await _bookingService.GetAllAsync(cancellationToken);

            TotalTours = tours.Count();
            TotalUsers = users.Count();
            TotalBookings = bookings.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard data");
        }

        return Page();
    }
}
