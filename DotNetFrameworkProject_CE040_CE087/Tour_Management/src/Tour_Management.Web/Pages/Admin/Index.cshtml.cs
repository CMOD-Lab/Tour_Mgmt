using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(
        ITourService tourService,
        IBookingService bookingService,
        IUserService userService,
        ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _bookingService = bookingService;
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Gets the total number of tours.</summary>
    public int TourCount { get; private set; }

    /// <summary>Gets the total number of bookings.</summary>
    public int BookingCount { get; private set; }

    /// <summary>Gets the total number of users.</summary>
    public int UserCount { get; private set; }

    /// <summary>
    /// Handles GET requests for the admin dashboard.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("./Login");
        }

        try
        {
            var tours = await _tourService.GetAllAsync(cancellationToken);
            var bookings = await _bookingService.GetAllAsync(cancellationToken);
            var users = await _userService.GetAllAsync(cancellationToken);

            TourCount = tours.Count();
            BookingCount = bookings.Count();
            UserCount = users.Count();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            return Page();
        }
    }
}
