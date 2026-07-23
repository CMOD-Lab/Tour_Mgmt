using AutoMapper;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Tour mappings
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.TourId, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<TourUpdateDto, Tour>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());

        // UserInfo mappings
        CreateMap<UserInfo, UserDto>();
        CreateMap<UserCreateDto, UserInfo>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<UserUpdateDto, UserInfo>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());

        // Booking mappings
        CreateMap<Booking, BookingDto>();
        CreateMap<BookingCreateDto, Booking>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<BookingUpdateDto, Booking>()
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}
