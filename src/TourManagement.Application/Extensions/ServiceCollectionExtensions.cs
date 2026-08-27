using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;

namespace TourManagement.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Application layer services with the DI container.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper with the mapping profile
        services.AddAutoMapper(MappingProfile.Configure);

        // Register application services
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
