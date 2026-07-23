using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user logout.
/// </summary>
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="LogoutModel"/>.
    /// </summary>
    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests to log out the user.
    /// </summary>
    public IActionResult OnGet()
    {
        _logger.LogInformation("User {Email} logged out", HttpContext.Session.GetString("UserEmail"));
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "You have been logged out successfully.";
        return RedirectToPage("/Index");
    }
}
