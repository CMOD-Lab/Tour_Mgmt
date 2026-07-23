using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for viewing user profile.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    /// <summary>Gets or sets the user view model.</summary>
    public UserViewModel? User { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ProfileModel"/>.
    /// </summary>
    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the profile page.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var dto = await _userService.GetByIdAsync(userId.Value, cancellationToken);
                if (dto is not null)
                {
                    User = new UserViewModel
                    {
                        Id = dto.Id,
                        Email = dto.Email,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Gender = dto.Gender,
                        Dob = dto.Dob,
                        Street = dto.Street,
                        City = dto.City,
                        State = dto.State,
                        Role = dto.Role,
                        CreatedDate = dto.CreatedDate,
                        IsActive = dto.IsActive
                    };
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user profile");
            TempData["ErrorMessage"] = "An error occurred while loading your profile.";
        }
    }
}
