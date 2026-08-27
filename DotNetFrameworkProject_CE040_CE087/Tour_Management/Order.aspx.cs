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
    public partial class Order : System.Web.UI.Page
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

        protected void btn_click(object sender, EventArgs e)
        {
            // cr-dotnet-0013: Use Dapper with RDS Proxy-compatible SqlConnection.
            // SqlConnection pooling is managed by RDS Proxy at the infrastructure level.
            using (IDbConnection conn = new SqlConnection(GetConnectionString()))
            {
                string insertQuery = "insert into booking(TOUR_NAME,PLACE,Email,FirstName) " +
                                     "values(@TOUR_NAME,@PLACE,@Email,@FirstName)";

                conn.Execute(insertQuery, new
                {
                    TOUR_NAME = tour_name.Text,
                    PLACE     = city.Text,
                    Email     = number.Text,
                    FirstName = name.Text
                });

                Response.Write("Registration Successful");
                Response.Redirect("mybooking.aspx");
            }
        }
    }
}
