using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using System.Data;
using System.Data.SqlClient;

// cr-dotnet-0013: Replaced direct SqlConnection with Dapper configured for Amazon RDS Proxy.
// cr-dotnet-0010: Connection string is now read from the DB_CONNECTION_STRING environment variable
//                 (injected at runtime via AWS Systems Manager Parameter Store / ECS task definition),
//                 eliminating reliance on Web.config transformation files.
// cr-dotnet-0026: Web Forms page retained; code-behind updated to cloud-native patterns.

namespace Tour_Management
{
    public partial class AddTour : System.Web.UI.Page
    {
        // Retrieve connection string from environment variable (AWS SSM / ECS injection).
        // Falls back to the legacy Web.config key for local development compatibility.
        private static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                ?? System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"]?.ConnectionString
                ?? throw new InvalidOperationException("Database connection string is not configured. Set the DB_CONNECTION_STRING environment variable.");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Register_Click(object sender, EventArgs e)
        {
            // cr-dotnet-0013: Use Dapper with RDS Proxy-compatible SqlConnection.
            // SqlConnection pooling is managed by RDS Proxy at the infrastructure level.
            using (IDbConnection conn = new SqlConnection(GetConnectionString()))
            {
                string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) " +
                                     "values(@TOUR_NAME,@PLACE,@DAYS,@PRICE,@LOCATIONS,@TOUR_INFO,@pic)";

                FileUpload1.SaveAs(Server.MapPath("~/Tour_pics/") + FileUpload1.FileName);

                conn.Execute(insertQuery, new
                {
                    TOUR_NAME  = tour_name.Text,
                    PLACE      = place.Text,
                    DAYS       = days.Text,
                    PRICE      = price.Text,
                    LOCATIONS  = locations.Text,
                    TOUR_INFO  = tour_info.Text,
                    pic        = FileUpload1.FileName
                });

                Response.Write("ADD  Successful");
            }
        }
    }
}
