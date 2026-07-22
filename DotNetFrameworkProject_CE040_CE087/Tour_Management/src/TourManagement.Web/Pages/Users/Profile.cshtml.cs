using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for the user profile page.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly ILogger<ProfileModel> _logger;

    /// <summary>Gets the user profile.</summary>
    public UserDto? User { get; private set; }

    /// <summary>Gets the user's recent bookings.</summary>
    public IEnumerable<BookingDto> RecentBookings { get; private set; } = Enumerable.Empty<BookingDto>();

    /// <summary>
    /// Initializes a new instance of <see cref="ProfileModel"/>.
    /// </summary>
    public ProfileModel(IUserService userService, IBookingService bookingService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the profile page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            TempData["ErrorMessage"] = "Please login to view your profile.";
            return RedirectToPage("Login");
        }

        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                User = await _userService.GetByIdAsync(userId.Value);
            }

            var bookings = await _bookingService.GetByEmailAsync(email);
            RecentBookings = bookings.Take(5);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for {Email}", email);
            return Page();
        }
    }
}
