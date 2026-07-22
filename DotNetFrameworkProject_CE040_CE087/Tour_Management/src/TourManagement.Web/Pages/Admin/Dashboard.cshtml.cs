using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard.
/// </summary>
public class DashboardModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly ILogger<DashboardModel> _logger;

    /// <summary>Gets the total number of tours.</summary>
    public int TotalTours { get; private set; }

    /// <summary>Gets the total number of bookings.</summary>
    public int TotalBookings { get; private set; }

    /// <summary>Gets the total number of users.</summary>
    public int TotalUsers { get; private set; }

    /// <summary>Gets the recent bookings.</summary>
    public IEnumerable<BookingDto> RecentBookings { get; private set; } = Enumerable.Empty<BookingDto>();

    /// <summary>
    /// Initializes a new instance of <see cref="DashboardModel"/>.
    /// </summary>
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

    /// <summary>
    /// Handles GET requests for the admin dashboard.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            TempData["ErrorMessage"] = "You must be an admin to access the dashboard.";
            return RedirectToPage("Login");
        }

        try
        {
            var tours = await _tourService.GetAllAsync();
            TotalTours = tours.Count();

            var bookings = await _bookingService.GetAllAsync();
            TotalBookings = bookings.Count();
            RecentBookings = bookings.Take(10);

            var users = await _userService.GetAllAsync();
            TotalUsers = users.Count();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            return Page();
        }
    }
}
