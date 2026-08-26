using System;
using Xunit;
using TourBooking.Application.DTOs;

namespace TourBooking.Tests.Application.DTOs
{
    /// <summary>
    /// Unit tests for Tour DTOs.
    /// </summary>
    public class TourDtosTests
    {
        [Fact]
        public void TourDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new TourDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void TourDto_TourId_ShouldGetAndSet()
        {
            var dto = new TourDto();
            dto.TourId = 3;
            Assert.Equal(3, dto.TourId);
        }

        [Fact]
        public void TourDto_TourName_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new TourDto();
            Assert.Equal(string.Empty, dto.TourName);
        }

        [Fact]
        public void TourDto_TourName_ShouldGetAndSet()
        {
            var dto = new TourDto();
            dto.TourName = "Goa Tour";
            Assert.Equal("Goa Tour", dto.TourName);
        }

        [Fact]
        public void TourDto_Place_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new TourDto();
            Assert.Equal(string.Empty, dto.Place);
        }

        [Fact]
        public void TourDto_Days_ShouldGetAndSet()
        {
            var dto = new TourDto();
            dto.Days = 5;
            Assert.Equal(5, dto.Days);
        }

        [Fact]
        public void TourDto_Price_ShouldGetAndSet()
        {
            var dto = new TourDto();
            dto.Price = 12500.75m;
            Assert.Equal(12500.75m, dto.Price);
        }

        [Fact]
        public void TourDto_Locations_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new TourDto();
            Assert.Equal(string.Empty, dto.Locations);
        }

        [Fact]
        public void TourDto_TourInfo_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new TourDto();
            Assert.Equal(string.Empty, dto.TourInfo);
        }

        [Fact]
        public void TourDto_Pic_ShouldDefaultToNull()
        {
            var dto = new TourDto();
            Assert.Null(dto.Pic);
        }

        [Fact]
        public void TourDto_AllProperties_ShouldBeSetCorrectly()
        {
            var dto = new TourDto
            {
                TourId = 1,
                TourName = "Kerala Tour",
                Place = "Kerala",
                Days = 6,
                Price = 20000m,
                Locations = "Munnar, Alleppey",
                TourInfo = "Beautiful backwaters",
                Pic = "kerala.jpg"
            };
            Assert.Equal(1, dto.TourId);
            Assert.Equal("Kerala Tour", dto.TourName);
            Assert.Equal("Kerala", dto.Place);
            Assert.Equal(6, dto.Days);
            Assert.Equal(20000m, dto.Price);
            Assert.Equal("Munnar, Alleppey", dto.Locations);
            Assert.Equal("Beautiful backwaters", dto.TourInfo);
            Assert.Equal("kerala.jpg", dto.Pic);
        }

        [Fact]
        public void TourCreateDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new TourCreateDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void TourCreateDto_AllProperties_ShouldGetAndSet()
        {
            var dto = new TourCreateDto
            {
                TourName = "New Tour",
                Place = "New Place",
                Days = 3,
                Price = 5000m,
                Locations = "Location A, Location B",
                TourInfo = "Tour description",
                Pic = "tour.jpg"
            };
            Assert.Equal("New Tour", dto.TourName);
            Assert.Equal("New Place", dto.Place);
            Assert.Equal(3, dto.Days);
            Assert.Equal(5000m, dto.Price);
            Assert.Equal("Location A, Location B", dto.Locations);
            Assert.Equal("Tour description", dto.TourInfo);
            Assert.Equal("tour.jpg", dto.Pic);
        }

        [Fact]
        public void TourCreateDto_DefaultValues_ShouldBeEmptyStrings()
        {
            var dto = new TourCreateDto();
            Assert.Equal(string.Empty, dto.TourName);
            Assert.Equal(string.Empty, dto.Place);
            Assert.Equal(string.Empty, dto.Locations);
            Assert.Equal(string.Empty, dto.TourInfo);
            Assert.Null(dto.Pic);
        }

        [Fact]
        public void TourUpdateDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new TourUpdateDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void TourUpdateDto_AllProperties_ShouldGetAndSet()
        {
            var dto = new TourUpdateDto
            {
                TourName = "Updated Tour",
                Place = "Updated Place",
                Days = 4,
                Price = 8000m,
                Locations = "Updated Locations",
                TourInfo = "Updated Info",
                Pic = "updated.jpg"
            };
            Assert.Equal("Updated Tour", dto.TourName);
            Assert.Equal("Updated Place", dto.Place);
            Assert.Equal(4, dto.Days);
            Assert.Equal(8000m, dto.Price);
            Assert.Equal("Updated Locations", dto.Locations);
            Assert.Equal("Updated Info", dto.TourInfo);
            Assert.Equal("updated.jpg", dto.Pic);
        }
    }
}
