using System;
using TourManagement.Application.DTOs;
using Xunit;

namespace TourManagement.Application.DTOs.Tests
{
    /// <summary>
    /// Unit tests for Tour DTOs.
    /// </summary>
    public class TourDtosTests
    {
        // ==================== TourDto Tests ====================

        [Fact]
        public void TourDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new TourDto();

            // Assert
            Assert.Equal(0, dto.TourId);
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(0, dto.Days);
            Assert.Equal(0m, dto.Price);
            Assert.Equal(string.Empty, dto.Locations);
            Assert.Equal(string.Empty, dto.TourInfo);
            Assert.Null(dto.Pic);
            Assert.False(dto.IsActive);
        }

        [Fact]
        public void TourDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var dto = new TourDto
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

            // Assert
            Assert.Equal(1, dto.TourId);
            Assert.Equal("Goa Tour", dto.TourName);
            Assert.Equal("Goa", dto.Place);
            Assert.Equal(5, dto.Days);
            Assert.Equal(15000m, dto.Price);
            Assert.Equal("Goa Beach", dto.Locations);
            Assert.Equal("Beautiful beaches", dto.TourInfo);
            Assert.Equal("goa.jpg", dto.Pic);
            Assert.True(dto.IsActive);
        }

        // ==================== TourCreateDto Tests ====================

        [Fact]
        public void TourCreateDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new TourCreateDto();

            // Assert
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(0, dto.Days);
            Assert.Equal(0m, dto.Price);
            Assert.Equal(string.Empty, dto.Locations);
            Assert.Equal(string.Empty, dto.TourInfo);
            Assert.Null(dto.Pic);
        }

        [Fact]
        public void TourCreateDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var dto = new TourCreateDto
            {
                TourName = "Kerala Tour",
                Place = "Kerala",
                Days = 6,
                Price = 20000m,
                Locations = "Backwaters",
                TourInfo = "God's own country",
                Pic = "kerala.jpg"
            };

            // Assert
            Assert.Equal("Kerala Tour", dto.TourName);
            Assert.Equal("Kerala", dto.Place);
            Assert.Equal(6, dto.Days);
            Assert.Equal(20000m, dto.Price);
            Assert.Equal("Backwaters", dto.Locations);
            Assert.Equal("God's own country", dto.TourInfo);
            Assert.Equal("kerala.jpg", dto.Pic);
        }

        // ==================== TourUpdateDto Tests ====================

        [Fact]
        public void TourUpdateDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new TourUpdateDto();

            // Assert
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(0, dto.Days);
            Assert.Equal(0m, dto.Price);
            Assert.Equal(string.Empty, dto.Locations);
            Assert.Equal(string.Empty, dto.TourInfo);
            Assert.Null(dto.Pic);
            Assert.True(dto.IsActive); // Default true
        }

        [Fact]
        public void TourUpdateDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var dto = new TourUpdateDto
            {
                TourName = "Updated Tour",
                Place = "Updated Place",
                Days = 10,
                Price = 30000m,
                Locations = "Updated Locations",
                TourInfo = "Updated info",
                Pic = "updated.jpg",
                IsActive = false
            };

            // Assert
            Assert.Equal("Updated Tour", dto.TourName);
            Assert.Equal("Updated Place", dto.Place);
            Assert.Equal(10, dto.Days);
            Assert.Equal(30000m, dto.Price);
            Assert.Equal("Updated Locations", dto.Locations);
            Assert.Equal("Updated info", dto.TourInfo);
            Assert.Equal("updated.jpg", dto.Pic);
            Assert.False(dto.IsActive);
        }
    }

    /// <summary>
    /// Unit tests for Booking DTOs.
    /// </summary>
    public class BookingDtosTests
    {
        // ==================== BookingDto Tests ====================

        [Fact]
        public void BookingDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new BookingDto();

            // Assert
            Assert.Equal(0, dto.BookingId);
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.FirstName);
            Assert.Null(dto.TourId);
            Assert.False(dto.IsActive);
        }

        [Fact]
        public void BookingDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 15);

            // Act
            var dto = new BookingDto
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

            // Assert
            Assert.Equal(1, dto.BookingId);
            Assert.Equal("Goa Tour", dto.TourName);
            Assert.Equal("Mumbai", dto.Place);
            Assert.Equal("john@example.com", dto.Email);
            Assert.Equal("John", dto.FirstName);
            Assert.Equal(5, dto.TourId);
            Assert.Equal(bookingDate, dto.BookingDate);
            Assert.True(dto.IsActive);
        }

        // ==================== BookingCreateDto Tests ====================

        [Fact]
        public void BookingCreateDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new BookingCreateDto();

            // Assert
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.FirstName);
            Assert.Null(dto.TourId);
        }

        [Fact]
        public void BookingCreateDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var dto = new BookingCreateDto
            {
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john@example.com",
                FirstName = "John",
                TourId = 5
            };

            // Assert
            Assert.Equal("Goa Tour", dto.TourName);
            Assert.Equal("Mumbai", dto.Place);
            Assert.Equal("john@example.com", dto.Email);
            Assert.Equal("John", dto.FirstName);
            Assert.Equal(5, dto.TourId);
        }

        // ==================== BookingUpdateDto Tests ====================

        [Fact]
        public void BookingUpdateDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new BookingUpdateDto();

            // Assert
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.FirstName);
            Assert.True(dto.IsActive); // Default true
        }

        [Fact]
        public void BookingUpdateDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var dto = new BookingUpdateDto
            {
                TourName = "Updated Tour",
                Place = "Updated Place",
                Email = "updated@example.com",
                FirstName = "Updated",
                IsActive = false
            };

            // Assert
            Assert.Equal("Updated Tour", dto.TourName);
            Assert.Equal("Updated Place", dto.Place);
            Assert.Equal("updated@example.com", dto.Email);
            Assert.Equal("Updated", dto.FirstName);
            Assert.False(dto.IsActive);
        }
    }

    /// <summary>
    /// Unit tests for User DTOs.
    /// </summary>
    public class UserDtosTests
    {
        // ==================== UserDto Tests ====================

        [Fact]
        public void UserDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new UserDto();

            // Assert
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.FirstName);
            Assert.Equal(string.Empty, dto.LastName);
            Assert.Equal(string.Empty, dto.Gender);
            Assert.Equal(string.Empty, dto.Street);
            Assert.Equal(string.Empty, dto.City);
            Assert.Equal(string.Empty, dto.State);
            Assert.False(dto.IsActive);
        }

        [Fact]
        public void UserDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            var dto = new UserDto
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

            // Assert
            Assert.Equal("john@example.com", dto.Email);
            Assert.Equal("John", dto.FirstName);
            Assert.Equal("Doe", dto.LastName);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal(dob, dto.DateOfBirth);
            Assert.Equal("123 Main St", dto.Street);
            Assert.Equal("Mumbai", dto.City);
            Assert.Equal("Maharashtra", dto.State);
            Assert.True(dto.IsActive);
            Assert.Equal(createdDate, dto.CreatedDate);
        }

        // ==================== UserCreateDto Tests ====================

        [Fact]
        public void UserCreateDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new UserCreateDto();

            // Assert
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.FirstName);
            Assert.Equal(string.Empty, dto.LastName);
            Assert.Equal(string.Empty, dto.Gender);
            Assert.Equal(string.Empty, dto.Password);
            Assert.Equal(string.Empty, dto.Street);
            Assert.Equal(string.Empty, dto.City);
            Assert.Equal(string.Empty, dto.State);
        }

        [Fact]
        public void UserCreateDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);

            // Act
            var dto = new UserCreateDto
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

            // Assert
            Assert.Equal("john@example.com", dto.Email);
            Assert.Equal("John", dto.FirstName);
            Assert.Equal("Doe", dto.LastName);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal("Password123!", dto.Password);
            Assert.Equal(dob, dto.DateOfBirth);
            Assert.Equal("123 Main St", dto.Street);
            Assert.Equal("Mumbai", dto.City);
            Assert.Equal("Maharashtra", dto.State);
        }

        // ==================== UserUpdateDto Tests ====================

        [Fact]
        public void UserUpdateDto_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var dto = new UserUpdateDto();

            // Assert
            Assert.Equal(string.Empty, dto.FirstName);
            Assert.Equal(string.Empty, dto.LastName);
            Assert.Equal(string.Empty, dto.Gender);
            Assert.Equal(string.Empty, dto.City);
            Assert.Equal(string.Empty, dto.Street);
            Assert.Equal(string.Empty, dto.State);
            Assert.True(dto.IsActive); // Default true
        }

        [Fact]
        public void UserUpdateDto_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var dto = new UserUpdateDto
            {
                FirstName = "Johnny",
                LastName = "Doe Updated",
                Gender = "Male",
                City = "Delhi",
                Street = "456 New St",
                State = "Delhi",
                IsActive = false
            };

            // Assert
            Assert.Equal("Johnny", dto.FirstName);
            Assert.Equal("Doe Updated", dto.LastName);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal("Delhi", dto.City);
            Assert.Equal("456 New St", dto.Street);
            Assert.Equal("Delhi", dto.State);
            Assert.False(dto.IsActive);
        }
    }
}
