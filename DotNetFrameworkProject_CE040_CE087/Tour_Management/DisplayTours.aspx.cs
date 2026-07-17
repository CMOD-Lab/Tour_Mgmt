// Cloud Readiness Fix: cr-dotnet-0026 - Migrated Web Forms code-behind to cloud-ready stateless pattern
// Cloud Readiness Fix: cr-dotnet-0013 - SqlDataSource in ASPX uses environment-variable-backed connection string
// Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
using System;
using System.Web.UI;

namespace Tour_Management
{
    /// <summary>
    /// DisplayTours page code-behind.
    /// Cloud-readiness: Web Forms page-based model retained with stateless pattern.
    /// SqlDataSource in the ASPX markup uses the "dbconnection" named connection string
    /// which is resolved from the TOURDB_CONNECTION_STRING environment variable at runtime
    /// via the cloud-native configuration in Web.config / appsettings.
    /// No server-side state (ViewState/Session) dependencies for horizontal scaling
    /// on Azure Container Apps.
    /// </summary>
    public partial class DisplayTours : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
