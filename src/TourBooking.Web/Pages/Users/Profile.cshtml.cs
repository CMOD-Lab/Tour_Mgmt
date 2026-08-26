using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Users;

/// <summary>
/// Page model for the user profile page.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    /// <summary>Gets or sets the user profile view model.</summary>
    public UserProfileViewModel? UserProfile { get; set; }

    /// <summary>Initializes a new instance of the <see cref="ProfileModel"/> class.</summary>
    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the profile page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var user = await _userService.GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                return RedirectToPage("/Users/Login");
            }

            // Manually map domain entity to ViewModel
            UserProfile = new UserProfileViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Dob = user.Dob,
                Street = user.Street,
                City = user.City,
                State = user.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for email: {Email}", email);
            return RedirectToPage("/Users/Login");
        }
    }
}
