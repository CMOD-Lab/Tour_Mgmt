using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for displaying user profile.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileModel"/> class.
    /// </summary>
    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Gets the user profile view model.</summary>
    public UserProfileViewModel? Profile { get; private set; }

    /// <summary>
    /// Handles GET requests for the profile page.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return;
            }

            var user = await _userService.GetByEmailAsync(userEmail, cancellationToken);
            if (user != null)
            {
                // Manual ViewModel mapping
                Profile = new UserProfileViewModel
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    Street = user.Street,
                    City = user.City,
                    State = user.State,
                    CreatedDate = user.CreatedDate
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user profile");
        }
    }
}
