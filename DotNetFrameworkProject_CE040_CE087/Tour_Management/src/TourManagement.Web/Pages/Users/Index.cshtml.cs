using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for the users management listing page (admin only).
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the list of users.</summary>
    public IEnumerable<UserDto> Users { get; private set; } = Enumerable.Empty<UserDto>();

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the users management page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            TempData["ErrorMessage"] = "You must be an admin to manage users.";
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            Users = await _userService.GetAllAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users list");
            TempData["ErrorMessage"] = "An error occurred while loading users.";
            return Page();
        }
    }
}
