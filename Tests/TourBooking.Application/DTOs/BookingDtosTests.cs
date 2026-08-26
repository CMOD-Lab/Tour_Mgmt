using System;
using Xunit;
using TourBooking.Application.DTOs;

namespace TourBooking.Tests.Application.DTOs
{
    /// <summary>
    /// Unit tests for Booking DTOs.
    /// </summary>
    public class BookingDtosTests
    {
        [Fact]
        public void BookingDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new BookingDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void BookingDto_TourId_ShouldGetAndSet()
        {
            var dto = new BookingDto();
            dto.TourId = 5;
            Assert.Equal(5, dto.TourId);
        }

        [Fact]
        public void BookingDto_TourName_ShouldGetAndSet()
        {
            var dto = new BookingDto();
            dto.TourName = "Goa Tour";
            Assert.Equal("Goa Tour", dto.TourName);
        }

        [Fact]
        public void BookingDto_Place_ShouldGetAndSet()
        {
            var dto = new BookingDto();
            dto.Place = "Goa";
            Assert.Equal("Goa", dto.Place);
        }

        [Fact]
        public void BookingDto_Email_ShouldGetAndSet()
        {
            var dto = new BookingDto();
            dto.Email = "user@example.com";
            Assert.Equal("user@example.com", dto.Email);
        }

        [Fact]
        public void BookingDto_FirstName_ShouldGetAndSet()
        {
            var dto = new BookingDto();
            dto.FirstName = "John";
            Assert.Equal("John", dto.FirstName);
        }

        [Fact]
        public void BookingDto_NullableProperties_ShouldDefaultToNull()
        {
            var dto = new BookingDto();
            Assert.Null(dto.TourName);
            Assert.Null(dto.Place);
            Assert.Null(dto.Email);
            Assert.Null(dto.FirstName);
        }

        [Fact]
        public void BookingCreateDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new BookingCreateDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void BookingCreateDto_AllProperties_ShouldGetAndSet()
        {
            var dto = new BookingCreateDto
            {
                TourName = "Kashmir Tour",
                Place = "Kashmir",
                Email = "test@test.com",
                FirstName = "Alice"
            };
            Assert.Equal("Kashmir Tour", dto.TourName);
            Assert.Equal("Kashmir", dto.Place);
            Assert.Equal("test@test.com", dto.Email);
            Assert.Equal("Alice", dto.FirstName);
        }

        [Fact]
        public void BookingCreateDto_NullableProperties_ShouldDefaultToNull()
        {
            var dto = new BookingCreateDto();
            Assert.Null(dto.TourName);
            Assert.Null(dto.Place);
            Assert.Null(dto.Email);
            Assert.Null(dto.FirstName);
        }

        [Fact]
        public void BookingUpdateDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new BookingUpdateDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void BookingUpdateDto_AllProperties_ShouldGetAndSet()
        {
            var dto = new BookingUpdateDto
            {
                TourName = "Updated Tour",
                Place = "Updated Place",
                Email = "updated@test.com",
                FirstName = "UpdatedName"
            };
            Assert.Equal("Updated Tour", dto.TourName);
            Assert.Equal("Updated Place", dto.Place);
            Assert.Equal("updated@test.com", dto.Email);
            Assert.Equal("UpdatedName", dto.FirstName);
        }

        [Fact]
        public void BookingUpdateDto_NullableProperties_ShouldDefaultToNull()
        {
            var dto = new BookingUpdateDto();
            Assert.Null(dto.TourName);
            Assert.Null(dto.Place);
            Assert.Null(dto.Email);
            Assert.Null(dto.FirstName);
        }
    }
}
