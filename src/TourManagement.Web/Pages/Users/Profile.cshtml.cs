using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user profile.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public new UserViewModel? User { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("./Login");

        try
        {
            var userEntity = await _userService.GetUserByEmailAsync(email, cancellationToken);
            if (userEntity == null)
                return RedirectToPage("./Login");

            User = new UserViewModel
            {
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Gender = userEntity.Gender,
                Dob = userEntity.Dob,
                Street = userEntity.Street,
                City = userEntity.City,
                State = userEntity.State,
                IsActive = userEntity.IsActive
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for {Email}", email);
            return RedirectToPage("./Login");
        }
    }
}
