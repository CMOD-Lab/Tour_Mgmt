using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>Page model for the user profile page.</summary>
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

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("./Login");
        }

        try
        {
            var dto = await _userService.GetByEmailAsync(email, cancellationToken);
            if (dto != null)
            {
                User = UserViewModel.FromDto(dto);
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for email: {Email}", email);
            return Page();
        }
    }
}
