using AutoMapper;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Mappings;

/// <summary>
/// AutoMapper configuration for mapping between domain entities and DTOs.
/// Note: ViewModels in the Web layer are manually mapped to/from DTOs.
/// </summary>
public static class MappingProfile
{
    /// <summary>Configures all entity-DTO mappings on the provided expression.</summary>
    public static void Configure(IMapperConfigurationExpression cfg)
    {
        // Tour mappings
        cfg.CreateMap<Tour, TourDto>();
        cfg.CreateMap<TourCreateDto, Tour>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        cfg.CreateMap<TourUpdateDto, Tour>()
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());

        // Booking mappings
        cfg.CreateMap<Booking, BookingDto>();
        cfg.CreateMap<BookingCreateDto, Booking>()
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            .ForMember(dest => dest.Tour, opt => opt.Ignore());
        cfg.CreateMap<BookingUpdateDto, Booking>()
            .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            .ForMember(dest => dest.BookingDate, opt => opt.Ignore())
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.Tour, opt => opt.Ignore());

        // UserInfo mappings
        cfg.CreateMap<UserInfo, UserDto>();
        cfg.CreateMap<UserCreateDto, UserInfo>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        cfg.CreateMap<UserUpdateDto, UserInfo>()
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
    }
}
