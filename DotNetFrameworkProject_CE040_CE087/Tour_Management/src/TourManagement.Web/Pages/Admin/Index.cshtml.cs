using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets whether the current user is an admin.</summary>
    public bool IsAdmin { get; set; }

    /// <summary>Gets the total number of tours.</summary>
    public int TotalTours { get; set; }

    /// <summary>Gets the total number of users.</summary>
    public int TotalUsers { get; set; }

    /// <summary>Gets the total number of bookings.</summary>
    public int TotalBookings { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(
        ITourService tourService,
        IUserService userService,
        IBookingService bookingService,
        ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _userService = userService;
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the admin dashboard.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        var role = HttpContext.Session.GetString("UserRole");
        IsAdmin = role == "Admin";

        if (IsAdmin)
        {
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
                TempData["ErrorMessage"] = "An error occurred while loading dashboard data.";
            }
        }
    }
}
