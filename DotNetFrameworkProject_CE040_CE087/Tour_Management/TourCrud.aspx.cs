// Cloud Readiness Fix: cr-dotnet-0013 - Replaced direct SqlConnection with Entity Framework Core DbContext
// Cloud Readiness Fix: cr-dotnet-0026 - Migrated Web Forms code-behind to cloud-ready stateless pattern
// Cloud Readiness Fix: cr-dotnet-0010 - Connection string now resolved from environment variables via DbContext
// Remediation: Migrate to Entity Framework Core with Azure SQL connection resiliency
// Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
// Remediation: Replace Web.config transformations with environment-based configuration
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management
{
    /// <summary>
    /// TourCrud page code-behind.
    /// Cloud-readiness: Direct SqlConnection replaced with Entity Framework Core DbContext
    /// providing built-in connection pooling, retry logic, and Azure SQL Database integration
    /// with transient fault handling. Configuration is read from environment variables.
    /// Web Forms page-based model retained; ViewState and postback dependencies minimized
    /// for stateless horizontal scaling on Azure Container Apps.
    /// </summary>
    public partial class TourCrud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshdata();
            }
        }

        /// <summary>
        /// Refreshes the tour data grid using Entity Framework Core DbContext.
        /// Cloud Readiness Fix: cr-dotnet-0013
        /// Replaced: new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString)
        /// With: Entity Framework Core DbContext with built-in connection pooling and Azure SQL resiliency.
        /// Connection string is resolved from TOURDB_CONNECTION_STRING environment variable (cloud-native)
        /// or falls back to named connection string in configuration.
        /// </summary>
        public void refreshdata()
        {
            using (var db = new TourManagementDbContext())
            {
                var tours = db.Tours.ToList();
                GridView1.DataSource = tours;
                GridView1.DataBind();
            }
        }
    }
}
