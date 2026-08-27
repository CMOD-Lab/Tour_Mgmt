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
    public partial class userlogin : System.Web.UI.Page
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

        protected void Btn_Submit(object sender, EventArgs e)
        {
            // cr-dotnet-0013: Use Dapper with RDS Proxy-compatible SqlConnection.
            // SqlConnection pooling is managed by RDS Proxy at the infrastructure level.
            // NOTE: Parameterized query used to prevent SQL injection (original code was vulnerable).
            using (IDbConnection conn = new SqlConnection(GetConnectionString()))
            {
                string checkPasswordQuery = "select password from Userinfo where password=@Password and email=@Email";
                string password = conn.QueryFirstOrDefault<string>(checkPasswordQuery, new
                {
                    Password = txtPassword.Text,
                    Email    = txtEmail.Text
                }) ?? "";

                if (password == txtPassword.Text)
                {
                    Response.Write("Password is correct");
                    Response.Redirect("MainProfilePage.aspx");
                }
                else
                {
                    Response.Write("Password is not correct");
                }
            }
        }

        protected void Btn_reg(object sender, EventArgs e)
        {
            Response.Redirect("SignUpForm.aspx");
        }
    }
}
