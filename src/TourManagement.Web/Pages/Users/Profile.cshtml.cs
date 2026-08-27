using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for the user profile page.
/// </summary>
[Authorize]
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    /// <summary>Gets the user profile to display.</summary>
    public UserProfileViewModel? Profile { get; private set; }

    /// <summary>Initializes a new instance of ProfileModel.</summary>
    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the profile page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("Login");

            var dto = await _userService.GetByEmailAsync(email, cancellationToken);
            if (dto == null)
                return NotFound();

            // Manual mapping from DTO to ViewModel
            Profile = new UserProfileViewModel
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Street = dto.Street,
                City = dto.City,
                State = dto.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user profile");
            return RedirectToPage("/Index");
        }
    }
}
