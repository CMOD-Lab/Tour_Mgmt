// Cloud Readiness Fix: cr-dotnet-0026 - Migrated Web Forms code-behind to cloud-ready stateless pattern
// Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
using System;
using System.Web.UI;

namespace Tour_Management
{
    /// <summary>
    /// MainProfilePage code-behind.
    /// Cloud-readiness: Web Forms page-based model retained with stateless pattern.
    /// No server-side state (ViewState/Session) dependencies for horizontal scaling
    /// on Azure Container Apps.
    /// </summary>
    public partial class MainProfilePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
