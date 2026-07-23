using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user profile page.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    public UserProfileViewModel? Profile { get; set; }

    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

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
            if (user != null)
            {
                // Manually map domain entity to ViewModel
                Profile = new UserProfileViewModel
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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for user {Email}", email);
        }

        return Page();
    }
}
