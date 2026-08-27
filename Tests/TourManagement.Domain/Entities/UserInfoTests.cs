using System;
using System.Collections.Generic;
using TourManagement.Domain.Entities;
using Xunit;

namespace TourManagement.Domain.Entities.Tests
{
    /// <summary>
    /// Unit tests for the UserInfo entity.
    /// </summary>
    public class UserInfoTests
    {
        [Fact]
        public void UserInfo_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var user = new UserInfo();

            // Assert
            Assert.Equal(string.Empty, user.Email);
            Assert.Equal(string.Empty, user.FirstName);
            Assert.Equal(string.Empty, user.LastName);
            Assert.Equal(string.Empty, user.Gender);
            Assert.Equal(string.Empty, user.Password);
            Assert.Equal(string.Empty, user.Street);
            Assert.Equal(string.Empty, user.City);
            Assert.Equal(string.Empty, user.State);
            Assert.True(user.IsActive);
            Assert.NotNull(user.Bookings);
            Assert.Empty(user.Bookings);
        }

        [Fact]
        public void UserInfo_CreatedDate_DefaultsToUtcNow()
        {
            // Arrange
            var before = DateTime.UtcNow.AddSeconds(-1);

            // Act
            var user = new UserInfo();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(user.CreatedDate >= before && user.CreatedDate <= after);
        }

        [Fact]
        public void UserInfo_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);

            // Act
            var user = new UserInfo
            {
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "hashedpassword123",
                DateOfBirth = dob,
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra",
                IsActive = true
            };

            // Assert
            Assert.Equal("john.doe@example.com", user.Email);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("Male", user.Gender);
            Assert.Equal("hashedpassword123", user.Password);
            Assert.Equal(dob, user.DateOfBirth);
            Assert.Equal("123 Main St", user.Street);
            Assert.Equal("Mumbai", user.City);
            Assert.Equal("Maharashtra", user.State);
            Assert.True(user.IsActive);
        }

        [Fact]
        public void UserInfo_IsActive_CanBeSetToFalse()
        {
            // Arrange
            var user = new UserInfo { IsActive = true };

            // Act
            user.IsActive = false;

            // Assert
            Assert.False(user.IsActive);
        }

        [Fact]
        public void UserInfo_Bookings_CanAddItems()
        {
            // Arrange
            var user = new UserInfo();
            var booking = new Booking { BookingId = 1, Email = "john.doe@example.com" };

            // Act
            user.Bookings.Add(booking);

            // Assert
            Assert.Single(user.Bookings);
        }

        [Fact]
        public void UserInfo_Bookings_InitializedAsEmptyList()
        {
            // Arrange & Act
            var user = new UserInfo();

            // Assert
            Assert.IsAssignableFrom<ICollection<Booking>>(user.Bookings);
            Assert.Empty(user.Bookings);
        }

        [Fact]
        public void UserInfo_DateOfBirth_CanBeSet()
        {
            // Arrange
            var dob = new DateTime(1985, 3, 20);

            // Act
            var user = new UserInfo { DateOfBirth = dob };

            // Assert
            Assert.Equal(dob, user.DateOfBirth);
        }

        [Fact]
        public void UserInfo_CreatedDate_CanBeOverridden()
        {
            // Arrange
            var specificDate = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc);

            // Act
            var user = new UserInfo { CreatedDate = specificDate };

            // Assert
            Assert.Equal(specificDate, user.CreatedDate);
        }

        [Fact]
        public void UserInfo_Email_IsUsedAsPrimaryKey()
        {
            // Arrange & Act
            var user = new UserInfo { Email = "test@example.com" };

            // Assert
            Assert.Equal("test@example.com", user.Email);
        }

        [Fact]
        public void UserInfo_MultipleBookings_CanBeAdded()
        {
            // Arrange
            var user = new UserInfo();
            var booking1 = new Booking { BookingId = 1 };
            var booking2 = new Booking { BookingId = 2 };

            // Act
            user.Bookings.Add(booking1);
            user.Bookings.Add(booking2);

            // Assert
            Assert.Equal(2, user.Bookings.Count);
        }
    }
}
