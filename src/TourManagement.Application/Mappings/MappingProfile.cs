using AutoMapper;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// Note: ViewModels in the Web layer are manually mapped to/from DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>Initializes a new instance of MappingProfile with all entity-DTO mappings.</summary>
    public MappingProfile()
    {
        // Tour mappings
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<TourUpdateDto, Tour>()
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());

        // Booking mappings
        CreateMap<Booking, BookingDto>();
        CreateMap<BookingCreateDto, Booking>()
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            .ForMember(dest => dest.Tour, opt => opt.Ignore());
        CreateMap<BookingUpdateDto, Booking>()
            .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            .ForMember(dest => dest.BookingDate, opt => opt.Ignore())
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.Tour, opt => opt.Ignore());

        // UserInfo mappings
        CreateMap<UserInfo, UserDto>();
        CreateMap<UserCreateDto, UserInfo>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<UserUpdateDto, UserInfo>()
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
    }
}
