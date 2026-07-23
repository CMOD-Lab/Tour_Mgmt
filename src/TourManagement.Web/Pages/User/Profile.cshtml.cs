using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.User;

/// <summary>
/// User profile page model - migrated from MainProfilePage.aspx.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    public UserProfileViewModel? UserProfile { get; set; }

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
            return RedirectToPage("/User/Login");
        }

        try
        {
            var userDto = await _userService.GetByEmailAsync(email, cancellationToken);
            if (userDto == null)
            {
                return RedirectToPage("/User/Login");
            }

            // Manual mapping from DTO to ViewModel
            UserProfile = new UserProfileViewModel
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Gender = userDto.Gender,
                Dob = userDto.Dob,
                Street = userDto.Street,
                City = userDto.City,
                State = userDto.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for email: {Email}", email);
            return RedirectToPage("/Error");
        }
    }
}
