using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for the logout page.
/// </summary>
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutModel"/> class.
    /// </summary>
    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger;
    }

    /// <summary>Handles GET requests for the logout page.</summary>
    public IActionResult OnGet()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        HttpContext.Session.Clear();
        _logger.LogInformation("User {Email} logged out", email);
        TempData["Success"] = "You have been logged out successfully.";
        return RedirectToPage("/Users/Login");
    }
}
