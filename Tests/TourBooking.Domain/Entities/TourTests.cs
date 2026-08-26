using System;
using System.Collections.Generic;
using Xunit;
using TourBooking.Domain.Entities;

namespace TourBooking.Tests.Domain.Entities
{
    /// <summary>
    /// Unit tests for the Tour entity.
    /// </summary>
    public class TourTests
    {
        [Fact]
        public void Tour_DefaultConstructor_ShouldCreateInstance()
        {
            var tour = new Tour();
            Assert.NotNull(tour);
        }

        [Fact]
        public void Tour_TourId_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.TourId = 10;
            Assert.Equal(10, tour.TourId);
        }

        [Fact]
        public void Tour_TourName_DefaultValue_ShouldBeEmptyString()
        {
            var tour = new Tour();
            Assert.Equal(string.Empty, tour.TourName);
        }

        [Fact]
        public void Tour_TourName_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.TourName = "Rajasthan Tour";
            Assert.Equal("Rajasthan Tour", tour.TourName);
        }

        [Fact]
        public void Tour_Place_DefaultValue_ShouldBeEmptyString()
        {
            var tour = new Tour();
            Assert.Equal(string.Empty, tour.Place);
        }

        [Fact]
        public void Tour_Place_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.Place = "Jaipur";
            Assert.Equal("Jaipur", tour.Place);
        }

        [Fact]
        public void Tour_Days_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.Days = 7;
            Assert.Equal(7, tour.Days);
        }

        [Fact]
        public void Tour_Price_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.Price = 25000.50m;
            Assert.Equal(25000.50m, tour.Price);
        }

        [Fact]
        public void Tour_Locations_DefaultValue_ShouldBeEmptyString()
        {
            var tour = new Tour();
            Assert.Equal(string.Empty, tour.Locations);
        }

        [Fact]
        public void Tour_Locations_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.Locations = "Jaipur, Jodhpur, Udaipur";
            Assert.Equal("Jaipur, Jodhpur, Udaipur", tour.Locations);
        }

        [Fact]
        public void Tour_TourInfo_DefaultValue_ShouldBeEmptyString()
        {
            var tour = new Tour();
            Assert.Equal(string.Empty, tour.TourInfo);
        }

        [Fact]
        public void Tour_TourInfo_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.TourInfo = "A wonderful tour through Rajasthan";
            Assert.Equal("A wonderful tour through Rajasthan", tour.TourInfo);
        }

        [Fact]
        public void Tour_Pic_ShouldDefaultToNull()
        {
            var tour = new Tour();
            Assert.Null(tour.Pic);
        }

        [Fact]
        public void Tour_Pic_ShouldGetAndSet()
        {
            var tour = new Tour();
            tour.Pic = "rajasthan.jpg";
            Assert.Equal("rajasthan.jpg", tour.Pic);
        }

        [Fact]
        public void Tour_Bookings_DefaultValue_ShouldBeEmptyCollection()
        {
            var tour = new Tour();
            Assert.NotNull(tour.Bookings);
            Assert.Empty(tour.Bookings);
        }

        [Fact]
        public void Tour_Bookings_ShouldAllowAddingBookings()
        {
            var tour = new Tour();
            var booking = new Booking { TourId = 1, TourName = "Rajasthan Tour" };
            tour.Bookings.Add(booking);
            Assert.Single(tour.Bookings);
        }

        [Fact]
        public void Tour_AllProperties_ShouldBeSetCorrectly()
        {
            var tour = new Tour
            {
                TourId = 5,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "North Goa, South Goa",
                TourInfo = "Beach paradise",
                Pic = "goa.jpg"
            };
            Assert.Equal(5, tour.TourId);
            Assert.Equal("Goa Tour", tour.TourName);
            Assert.Equal("Goa", tour.Place);
            Assert.Equal(5, tour.Days);
            Assert.Equal(15000m, tour.Price);
            Assert.Equal("North Goa, South Goa", tour.Locations);
            Assert.Equal("Beach paradise", tour.TourInfo);
            Assert.Equal("goa.jpg", tour.Pic);
        }

        [Fact]
        public void Tour_Price_ZeroValue_ShouldBeAllowed()
        {
            var tour = new Tour();
            tour.Price = 0m;
            Assert.Equal(0m, tour.Price);
        }

        [Fact]
        public void Tour_Days_ZeroValue_ShouldBeAllowed()
        {
            var tour = new Tour();
            tour.Days = 0;
            Assert.Equal(0, tour.Days);
        }
    }
}
