using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourBooking.Web.Pages.Users;

/// <summary>
/// Page model for the logout page.
/// </summary>
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    /// <summary>Initializes a new instance of the <see cref="LogoutModel"/> class.</summary>
    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger;
    }

    /// <summary>Handles GET requests for the logout page.</summary>
    public IActionResult OnGet()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        HttpContext.Session.Clear();
        _logger.LogInformation("User logged out: {Email}", email);
        return RedirectToPage("/Users/Login");
    }
}
