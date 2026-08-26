using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourBooking.Web.Pages;

/// <summary>
/// Page model for the home/index page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Initializes a new instance of the <see cref="IndexModel"/> class.</summary>
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    /// <summary>Handles GET requests for the index page.</summary>
    public void OnGet()
    {
        _logger.LogInformation("Home page accessed");
    }
}
