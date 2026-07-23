using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Admin.Users;

/// <summary>
/// Page model for the admin users management page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    public IEnumerable<UserDto> Users { get; private set; } = Enumerable.Empty<UserDto>();

    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Account/AdminLogin");
        }

        try
        {
            Users = await _userService.GetAllAsync(cancellationToken);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin users list.");
            Users = Enumerable.Empty<UserDto>();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Account/AdminLogin");
        }

        try
        {
            await _userService.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User {Email} deleted by admin.", email);
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {Email}.", email);
            return RedirectToPage();
        }
    }
}
