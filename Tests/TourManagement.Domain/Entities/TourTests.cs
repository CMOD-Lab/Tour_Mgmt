using System;
using System.Collections.Generic;
using TourManagement.Domain.Entities;
using Xunit;

namespace TourManagement.Domain.Entities.Tests
{
    /// <summary>
    /// Unit tests for the Tour entity.
    /// </summary>
    public class TourTests
    {
        [Fact]
        public void Tour_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var tour = new Tour();

            // Assert
            Assert.Equal(0, tour.TourId);
            Assert.Equal(string.Empty, tour.TourName);
            Assert.Equal(string.Empty, tour.Place);
            Assert.Equal(0, tour.Days);
            Assert.Equal(0m, tour.Price);
            Assert.Equal(string.Empty, tour.Locations);
            Assert.Equal(string.Empty, tour.TourInfo);
            Assert.Null(tour.Pic);
            Assert.True(tour.IsActive);
            Assert.NotNull(tour.Bookings);
            Assert.Empty(tour.Bookings);
        }

        [Fact]
        public void Tour_CreatedDate_DefaultsToUtcNow()
        {
            // Arrange
            var before = DateTime.UtcNow.AddSeconds(-1);

            // Act
            var tour = new Tour();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(tour.CreatedDate >= before && tour.CreatedDate <= after);
        }

        [Fact]
        public void Tour_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var tour = new Tour
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000.00m,
                Locations = "Goa Beach, Panjim",
                TourInfo = "Beautiful beaches and nightlife",
                Pic = "goa.jpg",
                IsActive = true
            };

            // Assert
            Assert.Equal(1, tour.TourId);
            Assert.Equal("Goa Tour", tour.TourName);
            Assert.Equal("Goa", tour.Place);
            Assert.Equal(5, tour.Days);
            Assert.Equal(15000.00m, tour.Price);
            Assert.Equal("Goa Beach, Panjim", tour.Locations);
            Assert.Equal("Beautiful beaches and nightlife", tour.TourInfo);
            Assert.Equal("goa.jpg", tour.Pic);
            Assert.True(tour.IsActive);
        }

        [Fact]
        public void Tour_IsActive_CanBeSetToFalse()
        {
            // Arrange
            var tour = new Tour { IsActive = true };

            // Act
            tour.IsActive = false;

            // Assert
            Assert.False(tour.IsActive);
        }

        [Fact]
        public void Tour_Bookings_CanAddItems()
        {
            // Arrange
            var tour = new Tour();
            var booking = new Booking { BookingId = 1, TourName = "Goa Tour" };

            // Act
            tour.Bookings.Add(booking);

            // Assert
            Assert.Single(tour.Bookings);
        }

        [Fact]
        public void Tour_Price_CanBeZero()
        {
            // Arrange & Act
            var tour = new Tour { Price = 0m };

            // Assert
            Assert.Equal(0m, tour.Price);
        }

        [Fact]
        public void Tour_Price_CanBeHighValue()
        {
            // Arrange & Act
            var tour = new Tour { Price = 999999.99m };

            // Assert
            Assert.Equal(999999.99m, tour.Price);
        }

        [Fact]
        public void Tour_Days_CanBeSetToMaximum()
        {
            // Arrange & Act
            var tour = new Tour { Days = 365 };

            // Assert
            Assert.Equal(365, tour.Days);
        }

        [Fact]
        public void Tour_Pic_CanBeNull()
        {
            // Arrange & Act
            var tour = new Tour { Pic = null };

            // Assert
            Assert.Null(tour.Pic);
        }

        [Fact]
        public void Tour_Pic_CanBeSet()
        {
            // Arrange & Act
            var tour = new Tour { Pic = "tour_image.jpg" };

            // Assert
            Assert.Equal("tour_image.jpg", tour.Pic);
        }

        [Fact]
        public void Tour_Bookings_InitializedAsEmptyList()
        {
            // Arrange & Act
            var tour = new Tour();

            // Assert
            Assert.IsAssignableFrom<ICollection<Booking>>(tour.Bookings);
            Assert.Empty(tour.Bookings);
        }

        [Fact]
        public void Tour_CreatedDate_CanBeOverridden()
        {
            // Arrange
            var specificDate = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc);

            // Act
            var tour = new Tour { CreatedDate = specificDate };

            // Assert
            Assert.Equal(specificDate, tour.CreatedDate);
        }
    }
}
