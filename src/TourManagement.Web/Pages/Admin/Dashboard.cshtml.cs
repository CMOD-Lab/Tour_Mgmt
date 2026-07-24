using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Admin;

/// <summary>Page model for the admin dashboard page.</summary>
public class DashboardModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly ILogger<DashboardModel> _logger;

    /// <summary>Gets or sets the total number of tours.</summary>
    public int TotalTours { get; set; }

    /// <summary>Gets or sets the total number of bookings.</summary>
    public int TotalBookings { get; set; }

    /// <summary>Gets or sets the total number of users.</summary>
    public int TotalUsers { get; set; }

    /// <summary>Initializes a new instance of <see cref="DashboardModel"/>.</summary>
    public DashboardModel(
        ITourService tourService,
        IBookingService bookingService,
        IUserService userService,
        ILogger<DashboardModel> logger)
    {
        _tourService = tourService;
        _bookingService = bookingService;
        _userService = userService;
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
            var bookings = await _bookingService.GetAllAsync(cancellationToken);
            var users = await _userService.GetAllAsync(cancellationToken);

            TotalTours = tours.Count();
            TotalBookings = bookings.Count();
            TotalUsers = users.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard data");
        }

        return Page();
    }
}
