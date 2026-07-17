// This file has been migrated to ASP.NET Core Razor Pages.
// The Web Forms page userlogin.aspx has been replaced by Pages/User/Login.cshtml
// with its code-behind at Pages/User/Login.cshtml.cs.
//
// Fixes applied:
//   cr-dotnet-0013: Direct SqlConnection replaced with Entity Framework Core
//                   (TourManagementDbContext) with Azure SQL connection resiliency
//                   (EnableRetryOnFailure) and built-in connection pooling.
//   cr-dotnet-0026: Web Forms (System.Web.UI.Page) replaced with ASP.NET Core
//                   Razor Pages (PageModel), eliminating ViewState, postbacks,
//                   and server affinity for stateless horizontal scaling.
//   cr-dotnet-0010: ConfigurationManager.ConnectionStrings replaced with
//                   IConfiguration reading from appsettings.json and
//                   environment variables at runtime.
//
// See: Pages/User/Login.cshtml.cs

using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Data;

namespace Tour_Management
{
    // Stub retained for reference — active implementation is in Pages/User/Login.cshtml.cs
    [Obsolete("Migrated to ASP.NET Core Razor Pages. See Pages/User/Login.cshtml.cs")]
    public class UserLoginLegacyStub
    {
        // Original Web Forms code-behind replaced by Razor Page model.
        // EF Core DbContext (TourManagementDbContext) injected via DI
        // replaces: new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString)
    }
}
