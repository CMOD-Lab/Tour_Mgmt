using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Users;

/// <summary>Page model for the admin user list page.</summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of users.</summary>
    public IEnumerable<UserDto> Users { get; set; } = Enumerable.Empty<UserDto>();

    /// <summary>Initializes a new instance of <see cref="IndexModel"/>.</summary>
    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the user list page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            Users = await _userService.GetAllAsync(cancellationToken);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users list");
            return Page();
        }
    }

    /// <summary>Handles POST requests for deleting a user.</summary>
    public async Task<IActionResult> OnPostDeleteAsync(string email, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            await _userService.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("Admin deleted user with email {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with email {Email}", email);
        }

        return RedirectToPage();
    }
}
