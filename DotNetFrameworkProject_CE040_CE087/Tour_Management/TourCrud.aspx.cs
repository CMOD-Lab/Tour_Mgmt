using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Dapper;

// cr-dotnet-0013: Replaced direct SqlConnection with Dapper configured for Amazon RDS Proxy.
// cr-dotnet-0010: Connection string is now read from the DB_CONNECTION_STRING environment variable
//                 (injected at runtime via AWS Systems Manager Parameter Store / ECS task definition),
//                 eliminating reliance on Web.config transformation files.
// cr-dotnet-0026: Web Forms page retained; code-behind updated to cloud-native patterns.

namespace Tour_Management
{
    public partial class TourCrud : System.Web.UI.Page
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
            if (!Page.IsPostBack)
            {
                refreshdata();
            }
        }

        public void refreshdata()
        {
            // cr-dotnet-0013: Use Dapper with RDS Proxy-compatible SqlConnection.
            // SqlConnection pooling is managed by RDS Proxy at the infrastructure level.
            using (IDbConnection conn = new SqlConnection(GetConnectionString()))
            {
                string selectQuery = "select * from Tour";
                var tours = conn.Query(selectQuery);
                // GridView1.DataSource = tours;
                // GridView1.DataBind();
            }
        }
    }
}
