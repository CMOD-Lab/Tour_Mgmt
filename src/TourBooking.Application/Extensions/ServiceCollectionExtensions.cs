using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TourBooking.Application.Mappings;
using TourBooking.Application.Services;
using TourBooking.Domain.Interfaces.Services;

namespace TourBooking.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Application layer services with the dependency injection container.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IBookingService, BookingService>();

        return services;
    }
}
