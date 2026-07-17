using Microsoft.EntityFrameworkCore;
using Tour_Management.Data;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// Configuration: environment-based configuration replaces Web.config
// transformations. Values are read from appsettings.json,
// appsettings.{Environment}.json, and environment variables at runtime.
// -----------------------------------------------------------------------

// Add Razor Pages (replaces ASP.NET Web Forms)
builder.Services.AddRazorPages();

// -----------------------------------------------------------------------
// Database: Entity Framework Core with Azure SQL connection resiliency
// replaces direct SqlConnection management.
// EnableRetryOnFailure provides built-in transient fault handling for
// Azure SQL Database (replaces manual connection open/close patterns).
// -----------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("dbconnection")
    ?? Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DBCONNECTION")
    ?? throw new InvalidOperationException(
        "Connection string 'dbconnection' not found. " +
        "Set the CONNECTIONSTRINGS__DBCONNECTION environment variable.");

builder.Services.AddDbContext<TourManagementDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Azure SQL connection resiliency: retry on transient failures
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        // Connection timeout for cloud environments
        sqlOptions.CommandTimeout(60);
    }));

// Add structured logging for cloud monitoring
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// -----------------------------------------------------------------------
// HTTP pipeline configuration
// -----------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Default route: redirect root to user login
app.MapGet("/", () => Results.Redirect("/User/Login"));

app.Run();
