using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TourManagement.Application.Interfaces;
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
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

        // Register application services
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookingService, BookingService>();

        return services;
    }
}
