using System;
using System.Web;

namespace Tour_Management
{
    /// <summary>
    /// Health check HTTP handler for containerization liveness/readiness probes.
    /// Accessible at GET /health
    /// Returns HTTP 200 with JSON body: {"status":"healthy","application":"Tour_Management"}
    /// </summary>
    public class HealthCheckHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.Write("{\"status\":\"healthy\",\"application\":\"Tour_Management\"}");
        }
    }
}
