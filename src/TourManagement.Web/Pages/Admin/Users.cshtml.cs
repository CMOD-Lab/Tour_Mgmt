using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces.Services;

namespace TourManagement.Web.Pages.Admin;

/// <summary>
/// Page model for the admin users management page.
/// </summary>
[Authorize(Roles = "Admin")]
public class UsersModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersModel> _logger;

    /// <summary>Gets the list of users.</summary>
    public IEnumerable<UserDto> Users { get; private set; } = Enumerable.Empty<UserDto>();

    /// <summary>Initializes a new instance of UsersModel.</summary>
    public UsersModel(IUserService userService, ILogger<UsersModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the users management page.</summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Users = await _userService.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users list");
            TempData["ErrorMessage"] = "An error occurred while loading users.";
        }
    }
}
