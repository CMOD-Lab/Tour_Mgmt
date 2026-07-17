using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tour_Management.Pages.Admin
{
    /// <summary>
    /// Razor Page model for AdminProfile — replaces AdminProfile.aspx Web Forms page.
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Stateless page model compatible with horizontal scaling on Azure Container Apps.
    /// </summary>
    public class AdminProfileModel : PageModel
    {
        private readonly ILogger<AdminProfileModel> _logger;

        public AdminProfileModel(ILogger<AdminProfileModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Admin accessed profile/dashboard page.");
        }
    }
}
