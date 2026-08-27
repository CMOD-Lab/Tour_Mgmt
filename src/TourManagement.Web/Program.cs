using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using TourManagement.Application.Extensions;
using TourManagement.Infrastructure.Extensions;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/tourmanagement-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Tour Management application");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog for logging
    builder.Host.UseSerilog();

    // Add Razor Pages
    builder.Services.AddRazorPages();

    // Add Infrastructure services (DbContext, Repositories)
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // Add Application services (AutoMapper, Services)
    builder.Services.AddApplicationServices();

    // Add Cookie Authentication
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Users/Login";
            options.LogoutPath = "/Users/Logout";
            options.AccessDeniedPath = "/Users/Login";
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
        });

    builder.Services.AddAuthorization();

    // Add HTTP context accessor
    builder.Services.AddHttpContextAccessor();

    // Add memory cache
    builder.Services.AddMemoryCache();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
