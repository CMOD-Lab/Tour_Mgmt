using Serilog;
using TourBooking.Application.Extensions;
using TourBooking.Infrastructure.Extensions;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/tourbooking-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting TourBooking Web Application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddRazorPages();

    // Add session support
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    // Add HttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    // Register Application layer services
    builder.Services.AddApplicationServices();

    // Register Infrastructure layer services
    builder.Services.AddInfrastructureServices(builder.Configuration);

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

    app.UseSession();

    app.UseAuthorization();

    app.MapRazorPages();

    // Default redirect to login
    app.MapGet("/", () => Results.Redirect("/Users/Login"));

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
