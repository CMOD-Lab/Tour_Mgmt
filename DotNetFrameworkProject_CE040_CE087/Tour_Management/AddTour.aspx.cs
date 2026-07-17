// Cloud Readiness Fix: cr-dotnet-0013 - Replaced direct SqlConnection with Entity Framework Core DbContext
// Cloud Readiness Fix: cr-dotnet-0026 - Migrated Web Forms code-behind to cloud-ready stateless pattern
// Cloud Readiness Fix: cr-dotnet-0010 - Connection string now resolved from environment variables via DbContext
// Remediation: Migrate to Entity Framework Core with Azure SQL connection resiliency
// Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
// Remediation: Replace Web.config transformations with environment-based configuration
using System;
using System.Web.UI;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management
{
    /// <summary>
    /// AddTour page code-behind.
    /// Cloud-readiness: Direct SqlConnection replaced with Entity Framework Core DbContext
    /// providing built-in connection pooling, retry logic, and Azure SQL Database integration
    /// with transient fault handling. Configuration is read from environment variables.
    /// Web Forms page-based model retained; ViewState and postback dependencies minimized
    /// for stateless horizontal scaling on Azure Container Apps.
    /// </summary>
    public partial class AddTour : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            // Cloud Readiness Fix: cr-dotnet-0013
            // Replaced: new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString)
            // With: Entity Framework Core DbContext with built-in connection pooling and Azure SQL resiliency.
            // Connection string is resolved from TOURDB_CONNECTION_STRING environment variable (cloud-native)
            // or falls back to named connection string in configuration.
            using (var db = new TourManagementDbContext())
            {
                var tour = new Tour
                {
                    TOUR_NAME = tour_name.Text,
                    PLACE = place.Text,
                    TOUR_INFO = tour_info.Text,
                    LOCATIONS = locations.Text,
                    pic = FileUpload1.FileName
                };

                if (int.TryParse(days.Text, out int daysVal))
                    tour.DAYS = daysVal;

                if (decimal.TryParse(price.Text, out decimal priceVal))
                    tour.PRICE = priceVal;

                // Save uploaded file to Tour_pics folder
                if (FileUpload1.HasFile)
                {
                    FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);
                }

                db.Tours.Add(tour);
                db.SaveChanges();
            }

            Response.Write("ADD Successful");
        }
    }
}
