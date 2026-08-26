using System;
using System.Collections.Generic;
using Xunit;
using TourBooking.Domain.Entities;

namespace TourBooking.Tests.Domain.Entities
{
    /// <summary>
    /// Unit tests for the UserInfo entity.
    /// </summary>
    public class UserInfoTests
    {
        [Fact]
        public void UserInfo_DefaultConstructor_ShouldCreateInstance()
        {
            var user = new UserInfo();
            Assert.NotNull(user);
        }

        [Fact]
        public void UserInfo_Email_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.Email);
        }

        [Fact]
        public void UserInfo_Email_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.Email = "john.doe@example.com";
            Assert.Equal("john.doe@example.com", user.Email);
        }

        [Fact]
        public void UserInfo_FirstName_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.FirstName);
        }

        [Fact]
        public void UserInfo_FirstName_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.FirstName = "John";
            Assert.Equal("John", user.FirstName);
        }

        [Fact]
        public void UserInfo_LastName_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.LastName);
        }

        [Fact]
        public void UserInfo_LastName_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.LastName = "Doe";
            Assert.Equal("Doe", user.LastName);
        }

        [Fact]
        public void UserInfo_Gender_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.Gender);
        }

        [Fact]
        public void UserInfo_Gender_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.Gender = "Male";
            Assert.Equal("Male", user.Gender);
        }

        [Fact]
        public void UserInfo_Password_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.Password);
        }

        [Fact]
        public void UserInfo_Password_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.Password = "hashedpassword123";
            Assert.Equal("hashedpassword123", user.Password);
        }

        [Fact]
        public void UserInfo_Dob_ShouldGetAndSet()
        {
            var user = new UserInfo();
            var dob = new DateTime(1990, 5, 15);
            user.Dob = dob;
            Assert.Equal(dob, user.Dob);
        }

        [Fact]
        public void UserInfo_Street_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.Street);
        }

        [Fact]
        public void UserInfo_Street_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.Street = "123 Main Street";
            Assert.Equal("123 Main Street", user.Street);
        }

        [Fact]
        public void UserInfo_City_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.City);
        }

        [Fact]
        public void UserInfo_City_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.City = "Mumbai";
            Assert.Equal("Mumbai", user.City);
        }

        [Fact]
        public void UserInfo_State_DefaultValue_ShouldBeEmptyString()
        {
            var user = new UserInfo();
            Assert.Equal(string.Empty, user.State);
        }

        [Fact]
        public void UserInfo_State_ShouldGetAndSet()
        {
            var user = new UserInfo();
            user.State = "Maharashtra";
            Assert.Equal("Maharashtra", user.State);
        }

        [Fact]
        public void UserInfo_Bookings_DefaultValue_ShouldBeEmptyCollection()
        {
            var user = new UserInfo();
            Assert.NotNull(user.Bookings);
            Assert.Empty(user.Bookings);
        }

        [Fact]
        public void UserInfo_Bookings_ShouldAllowAddingBookings()
        {
            var user = new UserInfo { Email = "test@test.com" };
            var booking = new Booking { TourId = 1, TourName = "Goa Tour" };
            user.Bookings.Add(booking);
            Assert.Single(user.Bookings);
        }

        [Fact]
        public void UserInfo_AllProperties_ShouldBeSetCorrectly()
        {
            var dob = new DateTime(1985, 3, 20);
            var user = new UserInfo
            {
                Email = "alice@example.com",
                FirstName = "Alice",
                LastName = "Smith",
                Gender = "Female",
                Password = "securepass",
                Dob = dob,
                Street = "456 Oak Ave",
                City = "Delhi",
                State = "Delhi"
            };
            Assert.Equal("alice@example.com", user.Email);
            Assert.Equal("Alice", user.FirstName);
            Assert.Equal("Smith", user.LastName);
            Assert.Equal("Female", user.Gender);
            Assert.Equal("securepass", user.Password);
            Assert.Equal(dob, user.Dob);
            Assert.Equal("456 Oak Ave", user.Street);
            Assert.Equal("Delhi", user.City);
            Assert.Equal("Delhi", user.State);
        }
    }
}
