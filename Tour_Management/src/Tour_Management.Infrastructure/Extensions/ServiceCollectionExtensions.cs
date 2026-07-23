using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Infrastructure.Data;
using Tour_Management.Infrastructure.Repositories;

namespace Tour_Management.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering Infrastructure layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Infrastructure layer services with the DI container.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure Npgsql to use legacy timestamp behavior for DateTime compatibility
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Register DbContext with PostgreSQL provider
        services.AddDbContext<TourManagementDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                    npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", "public");
                })
            .UseSnakeCaseNamingConvention();
        });

        // Register repositories
        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}
