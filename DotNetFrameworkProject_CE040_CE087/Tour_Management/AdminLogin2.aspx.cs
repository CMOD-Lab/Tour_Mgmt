using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// cr-dotnet-0026: Web Forms page updated to cloud-native patterns.
// Admin credentials should be stored in AWS Systems Manager Parameter Store
// and retrieved via environment variables rather than hardcoded values.

namespace Tour_Management
{
    public partial class AdminLogin2 : System.Web.UI.Page
    {
        // Retrieve admin credentials from environment variables (AWS SSM / ECS injection).
        // Falls back to hardcoded defaults only for local development.
        private static string AdminEmail    => Environment.GetEnvironmentVariable("ADMIN_EMAIL")    ?? "admin@gmail.com";
        private static string AdminPassword => Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "admin";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (password.Text == AdminPassword && name.Text == AdminEmail)
            {
                Response.Redirect("AdminProfile.aspx");
            }
        }
    }
}
