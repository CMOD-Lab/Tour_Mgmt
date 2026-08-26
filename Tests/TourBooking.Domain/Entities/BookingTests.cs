using System;
using System.Collections.Generic;
using Xunit;
using TourBooking.Domain.Entities;

namespace TourBooking.Tests.Domain.Entities
{
    /// <summary>
    /// Unit tests for the Booking entity.
    /// </summary>
    public class BookingTests
    {
        [Fact]
        public void Booking_DefaultConstructor_ShouldCreateInstance()
        {
            var booking = new Booking();
            Assert.NotNull(booking);
        }

        [Fact]
        public void Booking_TourId_ShouldGetAndSet()
        {
            var booking = new Booking();
            booking.TourId = 42;
            Assert.Equal(42, booking.TourId);
        }

        [Fact]
        public void Booking_TourName_ShouldGetAndSet()
        {
            var booking = new Booking();
            booking.TourName = "Goa Tour";
            Assert.Equal("Goa Tour", booking.TourName);
        }

        [Fact]
        public void Booking_Place_ShouldGetAndSet()
        {
            var booking = new Booking();
            booking.Place = "Goa";
            Assert.Equal("Goa", booking.Place);
        }

        [Fact]
        public void Booking_Email_ShouldGetAndSet()
        {
            var booking = new Booking();
            booking.Email = "user@example.com";
            Assert.Equal("user@example.com", booking.Email);
        }

        [Fact]
        public void Booking_FirstName_ShouldGetAndSet()
        {
            var booking = new Booking();
            booking.FirstName = "John";
            Assert.Equal("John", booking.FirstName);
        }

        [Fact]
        public void Booking_User_ShouldGetAndSet()
        {
            var booking = new Booking();
            var user = new UserInfo { Email = "user@example.com", FirstName = "John" };
            booking.User = user;
            Assert.NotNull(booking.User);
            Assert.Equal("user@example.com", booking.User.Email);
        }

        [Fact]
        public void Booking_NullableProperties_ShouldDefaultToNull()
        {
            var booking = new Booking();
            Assert.Null(booking.TourName);
            Assert.Null(booking.Place);
            Assert.Null(booking.Email);
            Assert.Null(booking.FirstName);
            Assert.Null(booking.User);
        }

        [Fact]
        public void Booking_TourId_DefaultValue_ShouldBeZero()
        {
            var booking = new Booking();
            Assert.Equal(0, booking.TourId);
        }

        [Fact]
        public void Booking_AllProperties_ShouldBeSetCorrectly()
        {
            var user = new UserInfo { Email = "test@test.com" };
            var booking = new Booking
            {
                TourId = 1,
                TourName = "Kerala Tour",
                Place = "Kerala",
                Email = "test@test.com",
                FirstName = "Alice",
                User = user
            };
            Assert.Equal(1, booking.TourId);
            Assert.Equal("Kerala Tour", booking.TourName);
            Assert.Equal("Kerala", booking.Place);
            Assert.Equal("test@test.com", booking.Email);
            Assert.Equal("Alice", booking.FirstName);
            Assert.Equal(user, booking.User);
        }
    }
}
