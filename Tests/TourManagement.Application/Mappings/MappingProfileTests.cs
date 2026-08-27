using System;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TourManagement.Application.DTOs;
using TourManagement.Application.Mappings;
using TourManagement.Domain.Entities;
using Xunit;

namespace TourManagement.Application.Mappings.Tests
{
    /// <summary>
    /// Unit tests for AutoMapper MappingProfile.
    /// </summary>
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var config = new MapperConfiguration(MappingProfile.Configure, NullLoggerFactory.Instance);
            _mapper = config.CreateMapper();
        }

        // ==================== Tour Mapping Tests ====================

        [Fact]
        public void MappingProfile_ConfigurationIsValid()
        {
            // Arrange
            var config = new MapperConfiguration(MappingProfile.Configure, NullLoggerFactory.Instance);

            // Act & Assert
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Tour_To_TourDto_MapsAllProperties()
        {
            // Arrange
            var tour = new Tour
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches",
                Pic = "goa.jpg",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            // Act
            var dto = _mapper.Map<TourDto>(tour);

            // Assert
            Assert.Equal(tour.TourId, dto.TourId);
            Assert.Equal(tour.TourName, dto.TourName);
            Assert.Equal(tour.Place, dto.Place);
            Assert.Equal(tour.Days, dto.Days);
            Assert.Equal(tour.Price, dto.Price);
            Assert.Equal(tour.Locations, dto.Locations);
            Assert.Equal(tour.TourInfo, dto.TourInfo);
            Assert.Equal(tour.Pic, dto.Pic);
            Assert.Equal(tour.CreatedDate, dto.CreatedDate);
            Assert.Equal(tour.IsActive, dto.IsActive);
        }

        [Fact]
        public void TourCreateDto_To_Tour_MapsCorrectly()
        {
            // Arrange
            var createDto = new TourCreateDto
            {
                TourName = "Kerala Tour",
                Place = "Kerala",
                Days = 6,
                Price = 20000m,
                Locations = "Backwaters",
                TourInfo = "God's own country",
                Pic = "kerala.jpg"
            };

            // Act
            var tour = _mapper.Map<Tour>(createDto);

            // Assert
            Assert.Equal(createDto.TourName, tour.TourName);
            Assert.Equal(createDto.Place, tour.Place);
            Assert.Equal(createDto.Days, tour.Days);
            Assert.Equal(createDto.Price, tour.Price);
            Assert.Equal(createDto.Locations, tour.Locations);
            Assert.Equal(createDto.TourInfo, tour.TourInfo);
            Assert.Equal(createDto.Pic, tour.Pic);
            Assert.Equal(0, tour.TourId); // Ignored
            Assert.True(tour.IsActive);   // Default true
        }

        [Fact]
        public void TourCreateDto_To_Tour_SetsCreatedDateToUtcNow()
        {
            // Arrange
            var createDto = new TourCreateDto { TourName = "Test", Place = "Test", Days = 1, Price = 100m, Locations = "Test", TourInfo = "Test" };
            var before = DateTime.UtcNow.AddSeconds(-1);

            // Act
            var tour = _mapper.Map<Tour>(createDto);
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(tour.CreatedDate >= before && tour.CreatedDate <= after);
        }

        [Fact]
        public void TourUpdateDto_To_Tour_MapsCorrectly()
        {
            // Arrange
            var existingTour = new Tour { TourId = 1, TourName = "Old Name", Place = "Old Place", Days = 3, Price = 5000m, Locations = "Old", TourInfo = "Old info", IsActive = true };
            var updateDto = new TourUpdateDto
            {
                TourName = "New Name",
                Place = "New Place",
                Days = 7,
                Price = 25000m,
                Locations = "New Locations",
                TourInfo = "New info",
                IsActive = false
            };

            // Act
            _mapper.Map(updateDto, existingTour);

            // Assert
            Assert.Equal("New Name", existingTour.TourName);
            Assert.Equal("New Place", existingTour.Place);
            Assert.Equal(7, existingTour.Days);
            Assert.Equal(25000m, existingTour.Price);
            Assert.Equal("New Locations", existingTour.Locations);
            Assert.Equal("New info", existingTour.TourInfo);
            Assert.False(existingTour.IsActive);
            Assert.Equal(1, existingTour.TourId); // Preserved
        }

        // ==================== Booking Mapping Tests ====================

        [Fact]
        public void Booking_To_BookingDto_MapsAllProperties()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 15);
            var booking = new Booking
            {
                BookingId = 1,
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john@example.com",
                FirstName = "John",
                TourId = 5,
                BookingDate = bookingDate,
                IsActive = true
            };

            // Act
            var dto = _mapper.Map<BookingDto>(booking);

            // Assert
            Assert.Equal(booking.BookingId, dto.BookingId);
            Assert.Equal(booking.TourName, dto.TourName);
            Assert.Equal(booking.Place, dto.Place);
            Assert.Equal(booking.Email, dto.Email);
            Assert.Equal(booking.FirstName, dto.FirstName);
            Assert.Equal(booking.TourId, dto.TourId);
            Assert.Equal(booking.BookingDate, dto.BookingDate);
            Assert.Equal(booking.IsActive, dto.IsActive);
        }

        [Fact]
        public void BookingCreateDto_To_Booking_MapsCorrectly()
        {
            // Arrange
            var createDto = new BookingCreateDto
            {
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john@example.com",
                FirstName = "John",
                TourId = 5
            };

            // Act
            var booking = _mapper.Map<Booking>(createDto);

            // Assert
            Assert.Equal(createDto.TourName, booking.TourName);
            Assert.Equal(createDto.Place, booking.Place);
            Assert.Equal(createDto.Email, booking.Email);
            Assert.Equal(createDto.FirstName, booking.FirstName);
            Assert.Equal(createDto.TourId, booking.TourId);
            Assert.Equal(0, booking.BookingId); // Ignored
            Assert.True(booking.IsActive);       // Default true
        }

        [Fact]
        public void BookingUpdateDto_To_Booking_MapsCorrectly()
        {
            // Arrange
            var existingBooking = new Booking { BookingId = 1, TourName = "Old Tour", Place = "Old Place", Email = "old@test.com", FirstName = "Old", TourId = 3, IsActive = true };
            var updateDto = new BookingUpdateDto
            {
                TourName = "New Tour",
                Place = "New Place",
                Email = "new@test.com",
                FirstName = "New",
                IsActive = false
            };

            // Act
            _mapper.Map(updateDto, existingBooking);

            // Assert
            Assert.Equal("New Tour", existingBooking.TourName);
            Assert.Equal("New Place", existingBooking.Place);
            Assert.Equal("new@test.com", existingBooking.Email);
            Assert.Equal("New", existingBooking.FirstName);
            Assert.False(existingBooking.IsActive);
            Assert.Equal(1, existingBooking.BookingId); // Preserved
            Assert.Equal(3, existingBooking.TourId);    // Preserved
        }

        // ==================== UserInfo Mapping Tests ====================

        [Fact]
        public void UserInfo_To_UserDto_MapsAllProperties()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);
            var createdDate = new DateTime(2024, 1, 1);
            var user = new UserInfo
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = dob,
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra",
                IsActive = true,
                CreatedDate = createdDate
            };

            // Act
            var dto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.Gender, dto.Gender);
            Assert.Equal(user.DateOfBirth, dto.DateOfBirth);
            Assert.Equal(user.Street, dto.Street);
            Assert.Equal(user.City, dto.City);
            Assert.Equal(user.State, dto.State);
            Assert.Equal(user.IsActive, dto.IsActive);
            Assert.Equal(user.CreatedDate, dto.CreatedDate);
        }

        [Fact]
        public void UserCreateDto_To_UserInfo_MapsCorrectly()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);
            var createDto = new UserCreateDto
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "Password123!",
                DateOfBirth = dob,
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var user = _mapper.Map<UserInfo>(createDto);

            // Assert
            Assert.Equal(createDto.Email, user.Email);
            Assert.Equal(createDto.FirstName, user.FirstName);
            Assert.Equal(createDto.LastName, user.LastName);
            Assert.Equal(createDto.Gender, user.Gender);
            Assert.Equal(createDto.DateOfBirth, user.DateOfBirth);
            Assert.Equal(createDto.Street, user.Street);
            Assert.Equal(createDto.City, user.City);
            Assert.Equal(createDto.State, user.State);
            Assert.True(user.IsActive); // Default true
        }

        [Fact]
        public void UserUpdateDto_To_UserInfo_MapsCorrectly()
        {
            // Arrange
            var existingUser = new UserInfo
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "hashedpassword",
                DateOfBirth = new DateTime(1990, 5, 15),
                IsActive = true
            };
            var updateDto = new UserUpdateDto
            {
                FirstName = "Johnny",
                LastName = "Doe Updated",
                Gender = "Male",
                City = "Delhi",
                Street = "456 New St",
                State = "Delhi",
                IsActive = false
            };

            // Act
            _mapper.Map(updateDto, existingUser);

            // Assert
            Assert.Equal("Johnny", existingUser.FirstName);
            Assert.Equal("Doe Updated", existingUser.LastName);
            Assert.Equal("Delhi", existingUser.City);
            Assert.Equal("456 New St", existingUser.Street);
            Assert.Equal("Delhi", existingUser.State);
            Assert.False(existingUser.IsActive);
            Assert.Equal("john@example.com", existingUser.Email);       // Preserved
            Assert.Equal("hashedpassword", existingUser.Password);       // Preserved
            Assert.Equal(new DateTime(1990, 5, 15), existingUser.DateOfBirth); // Preserved
        }
    }
}
