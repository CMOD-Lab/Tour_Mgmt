using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourManagement.Web.Pages;

/// <summary>Page model for the home/index page.</summary>
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Initializes a new instance of <see cref="IndexModel"/>.</summary>
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    /// <summary>Handles GET requests for the home page.</summary>
    public void OnGet()
    {
        _logger.LogInformation("Home page accessed");
    }
}
