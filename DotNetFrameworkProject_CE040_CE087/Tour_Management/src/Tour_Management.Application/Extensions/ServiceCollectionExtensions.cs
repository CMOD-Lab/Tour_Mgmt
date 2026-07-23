using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Application.Validators;
using Tour_Management.Domain.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services with the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Registers all Application layer services.</summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register Services
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookingService, BookingService>();

        // Register FluentValidation validators
        services.AddScoped<IValidator<TourCreateDto>, TourCreateDtoValidator>();
        services.AddScoped<IValidator<TourUpdateDto>, TourUpdateDtoValidator>();
        services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
        services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
        services.AddScoped<IValidator<BookingCreateDto>, BookingCreateDtoValidator>();

        return services;
    }
}
