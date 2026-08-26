using AutoMapper;
using TourBooking.Application.DTOs;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>Initializes a new instance of the <see cref="MappingProfile"/> class.</summary>
    public MappingProfile()
    {
        // UserInfo mappings
        CreateMap<UserInfo, UserDto>();
        CreateMap<UserCreateDto, UserInfo>();
        CreateMap<UserUpdateDto, UserInfo>();

        // Tour mappings
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>();
        CreateMap<TourUpdateDto, Tour>();

        // Booking mappings
        CreateMap<Booking, BookingDto>();
        CreateMap<BookingCreateDto, Booking>();
        CreateMap<BookingUpdateDto, Booking>();
    }
}
