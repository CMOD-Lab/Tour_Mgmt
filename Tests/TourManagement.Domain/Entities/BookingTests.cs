using System;
using TourManagement.Domain.Entities;
using Xunit;

namespace TourManagement.Domain.Entities.Tests
{
    /// <summary>
    /// Unit tests for the Booking entity.
    /// </summary>
    public class BookingTests
    {
        [Fact]
        public void Booking_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var booking = new Booking();

            // Assert
            Assert.Equal(0, booking.BookingId);
            Assert.Equal(string.Empty, booking.TourName);
            Assert.Equal(string.Empty, booking.Place);
            Assert.Equal(string.Empty, booking.Email);
            Assert.Equal(string.Empty, booking.FirstName);
            Assert.Null(booking.TourId);
            Assert.True(booking.IsActive);
            Assert.Null(booking.Tour);
        }

        [Fact]
        public void Booking_BookingDate_DefaultsToUtcNow()
        {
            // Arrange
            var before = DateTime.UtcNow.AddSeconds(-1);

            // Act
            var booking = new Booking();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(booking.BookingDate >= before && booking.BookingDate <= after);
        }

        [Fact]
        public void Booking_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);

            // Act
            var booking = new Booking
            {
                BookingId = 1,
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john.doe@example.com",
                FirstName = "John",
                TourId = 5,
                BookingDate = bookingDate,
                IsActive = true
            };

            // Assert
            Assert.Equal(1, booking.BookingId);
            Assert.Equal("Goa Tour", booking.TourName);
            Assert.Equal("Mumbai", booking.Place);
            Assert.Equal("john.doe@example.com", booking.Email);
            Assert.Equal("John", booking.FirstName);
            Assert.Equal(5, booking.TourId);
            Assert.Equal(bookingDate, booking.BookingDate);
            Assert.True(booking.IsActive);
        }

        [Fact]
        public void Booking_IsActive_CanBeSetToFalse()
        {
            // Arrange
            var booking = new Booking { IsActive = true };

            // Act
            booking.IsActive = false;

            // Assert
            Assert.False(booking.IsActive);
        }

        [Fact]
        public void Booking_TourId_CanBeNull()
        {
            // Arrange & Act
            var booking = new Booking { TourId = null };

            // Assert
            Assert.Null(booking.TourId);
        }

        [Fact]
        public void Booking_TourId_CanBeSet()
        {
            // Arrange & Act
            var booking = new Booking { TourId = 10 };

            // Assert
            Assert.Equal(10, booking.TourId);
        }

        [Fact]
        public void Booking_Tour_NavigationProperty_CanBeSet()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Goa Tour" };

            // Act
            var booking = new Booking { Tour = tour };

            // Assert
            Assert.NotNull(booking.Tour);
            Assert.Equal(1, booking.Tour.TourId);
            Assert.Equal("Goa Tour", booking.Tour.TourName);
        }

        [Fact]
        public void Booking_Tour_NavigationProperty_DefaultsToNull()
        {
            // Arrange & Act
            var booking = new Booking();

            // Assert
            Assert.Null(booking.Tour);
        }

        [Fact]
        public void Booking_BookingDate_CanBeOverridden()
        {
            // Arrange
            var specificDate = new DateTime(2024, 3, 10, 8, 30, 0, DateTimeKind.Utc);

            // Act
            var booking = new Booking { BookingDate = specificDate };

            // Assert
            Assert.Equal(specificDate, booking.BookingDate);
        }

        [Fact]
        public void Booking_Email_CanBeSet()
        {
            // Arrange & Act
            var booking = new Booking { Email = "user@test.com" };

            // Assert
            Assert.Equal("user@test.com", booking.Email);
        }
    }
}
