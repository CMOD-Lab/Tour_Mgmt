using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tour_Management.Pages.User
{
    /// <summary>
    /// Razor Page model for MainProfile — replaces MainProfilePage.aspx Web Forms page.
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Stateless page model compatible with horizontal scaling on Azure Container Apps.
    /// </summary>
    public class MainProfileModel : PageModel
    {
        private readonly ILogger<MainProfileModel> _logger;

        public MainProfileModel(ILogger<MainProfileModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("User accessed main profile page.");
        }
    }
}
