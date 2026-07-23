using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>Page model for user profile page.</summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public UserViewModel? User { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToPage("Login");

        try
        {
            if (int.TryParse(userIdStr, out var userId))
            {
                var dto = await _userService.GetByIdAsync(userId, cancellationToken);
                if (dto != null)
                {
                    User = new UserViewModel
                    {
                        UserId = dto.UserId,
                        Email = dto.Email,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Gender = dto.Gender,
                        Dob = dto.Dob,
                        Street = dto.Street,
                        City = dto.City,
                        State = dto.State,
                        CreatedDate = dto.CreatedDate,
                        IsActive = dto.IsActive
                    };
                }
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user profile");
            return Page();
        }
    }
}
