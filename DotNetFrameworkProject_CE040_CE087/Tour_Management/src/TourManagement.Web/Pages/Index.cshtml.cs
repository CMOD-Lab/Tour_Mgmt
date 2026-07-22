using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages;

/// <summary>
/// Page model for the home/index page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the featured tours to display.</summary>
    public IEnumerable<TourDto> FeaturedTours { get; private set; } = Enumerable.Empty<TourDto>();

    /// <summary>Gets the total number of tours.</summary>
    public int TotalTours { get; private set; }

    /// <summary>Gets the total number of bookings.</summary>
    public int TotalBookings { get; private set; }

    /// <summary>Gets the total number of users.</summary>
    public int TotalUsers { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
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

    /// <summary>
    /// Handles GET requests for the home page.
    /// </summary>
    public async Task OnGetAsync()
    {
        try
        {
            var allTours = await _tourService.GetAllAsync();
            FeaturedTours = allTours.Take(6);
            TotalTours = allTours.Count();

            var allBookings = await _bookingService.GetAllAsync();
            TotalBookings = allBookings.Count();

            var allUsers = await _userService.GetAllAsync();
            TotalUsers = allUsers.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page data");
        }
    }
}
