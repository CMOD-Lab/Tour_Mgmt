using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for user profile.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly UserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(UserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public UserProfileViewModel? Profile { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
            return RedirectToPage("./Login");

        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("./Login");

            var dto = await _userService.GetByIdAsync(userId.Value, cancellationToken);
            if (dto == null)
                return RedirectToPage("./Login");

            // Manual mapping from DTO to ViewModel
            Profile = new UserProfileViewModel
            {
                UserId = dto.UserId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Dob = dto.Dob,
                Street = dto.Street,
                City = dto.City,
                State = dto.State
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for user: {Email}", userEmail);
            return RedirectToPage("./Login");
        }
    }
}
