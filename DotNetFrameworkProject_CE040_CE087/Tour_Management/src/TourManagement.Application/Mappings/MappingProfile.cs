using AutoMapper;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of <see cref="MappingProfile"/> and configures all mappings.
    /// </summary>
    public MappingProfile()
    {
        // Tour mappings
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
        CreateMap<TourUpdateDto, Tour>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // UserInfo mappings
        CreateMap<UserInfo, UserDto>();
        CreateMap<UserCreateDto, UserInfo>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => "User"));
        CreateMap<UserUpdateDto, UserInfo>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Booking mappings
        CreateMap<Booking, BookingDto>();
        CreateMap<BookingCreateDto, Booking>()
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
        CreateMap<BookingUpdateDto, Booking>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
