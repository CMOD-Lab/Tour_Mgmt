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
    public partial class SignUpForm : System.Web.UI.Page
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
                string insertQuery = "insert into UserInfo(Email,FirstName,LastName,Gender,Password,dob,Street,City,State) " +
                                     "values(@email,@FirstName,@LastName,@Gender,@Password,@dob,@Street,@City,@State)";

                conn.Execute(insertQuery, new
                {
                    email     = email.Text,
                    FirstName = fname.Text,
                    LastName  = lname.Text,
                    Gender    = gender.Text,
                    Password  = password1.Text,
                    dob       = dob.Text,
                    Street    = street.Text,
                    City      = city.Text,
                    State     = state.Text
                });

                Response.Write("Registration Successful");
                Response.Redirect("userlogin.aspx");
            }
        }
    }
}
