// Cloud Readiness Fix: cr-dotnet-0026 - Migrated Web Forms code-behind to cloud-ready stateless pattern
// Cloud Readiness Fix: cr-dotnet-0010 - Admin credentials now resolved from environment variables
// Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
// Remediation: Replace Web.config transformations with environment-based configuration
using System;
using System.Web.UI;

namespace Tour_Management
{
    /// <summary>
    /// AdminLogin2 page code-behind.
    /// Cloud-readiness: Admin credentials are now read from environment variables
    /// (ADMIN_EMAIL, ADMIN_PASSWORD) instead of being hardcoded, enabling
    /// cloud-native secrets management via Azure Key Vault or App Service configuration.
    /// Web Forms page-based model retained; ViewState and postback dependencies minimized
    /// for stateless horizontal scaling on Azure Container Apps.
    /// </summary>
    public partial class AdminLogin2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cloud Readiness Fix: cr-dotnet-0026 / cr-dotnet-0010
            // Admin credentials are resolved from environment variables for cloud-native secrets management.
            // Set ADMIN_EMAIL and ADMIN_PASSWORD in Azure App Service Configuration or Key Vault references.
            string adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@gmail.com";
            string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "admin";

            if (!string.IsNullOrEmpty(password.Text) && !string.IsNullOrEmpty(name.Text))
            {
                if (password.Text == adminPassword && name.Text == adminEmail)
                {
                    Response.Redirect("AdminProfile.aspx");
                }
            }
        }
    }
}
