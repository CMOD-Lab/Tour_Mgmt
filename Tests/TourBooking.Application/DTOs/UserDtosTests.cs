using System;
using Xunit;
using TourBooking.Application.DTOs;

namespace TourBooking.Tests.Application.DTOs
{
    /// <summary>
    /// Unit tests for User DTOs.
    /// </summary>
    public class UserDtosTests
    {
        [Fact]
        public void UserDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new UserDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void UserDto_Email_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserDto();
            Assert.Equal(string.Empty, dto.Email);
        }

        [Fact]
        public void UserDto_Email_ShouldGetAndSet()
        {
            var dto = new UserDto();
            dto.Email = "user@example.com";
            Assert.Equal("user@example.com", dto.Email);
        }

        [Fact]
        public void UserDto_FirstName_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserDto();
            Assert.Equal(string.Empty, dto.FirstName);
        }

        [Fact]
        public void UserDto_LastName_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserDto();
            Assert.Equal(string.Empty, dto.LastName);
        }

        [Fact]
        public void UserDto_Gender_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserDto();
            Assert.Equal(string.Empty, dto.Gender);
        }

        [Fact]
        public void UserDto_Dob_ShouldGetAndSet()
        {
            var dto = new UserDto();
            var dob = new DateTime(1990, 1, 1);
            dto.Dob = dob;
            Assert.Equal(dob, dto.Dob);
        }

        [Fact]
        public void UserDto_AllProperties_ShouldBeSetCorrectly()
        {
            var dob = new DateTime(1988, 7, 15);
            var dto = new UserDto
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Dob = dob,
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            Assert.Equal("john@example.com", dto.Email);
            Assert.Equal("John", dto.FirstName);
            Assert.Equal("Doe", dto.LastName);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal(dob, dto.Dob);
            Assert.Equal("123 Main St", dto.Street);
            Assert.Equal("Mumbai", dto.City);
            Assert.Equal("Maharashtra", dto.State);
        }

        [Fact]
        public void UserCreateDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new UserCreateDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void UserCreateDto_Password_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserCreateDto();
            Assert.Equal(string.Empty, dto.Password);
        }

        [Fact]
        public void UserCreateDto_AllProperties_ShouldGetAndSet()
        {
            var dob = new DateTime(1995, 3, 10);
            var dto = new UserCreateDto
            {
                Email = "new@example.com",
                FirstName = "New",
                LastName = "User",
                Gender = "Female",
                Password = "password123",
                Dob = dob,
                Street = "789 Elm St",
                City = "Bangalore",
                State = "Karnataka"
            };
            Assert.Equal("new@example.com", dto.Email);
            Assert.Equal("New", dto.FirstName);
            Assert.Equal("User", dto.LastName);
            Assert.Equal("Female", dto.Gender);
            Assert.Equal("password123", dto.Password);
            Assert.Equal(dob, dto.Dob);
            Assert.Equal("789 Elm St", dto.Street);
            Assert.Equal("Bangalore", dto.City);
            Assert.Equal("Karnataka", dto.State);
        }

        [Fact]
        public void UserUpdateDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new UserUpdateDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void UserUpdateDto_AllProperties_ShouldGetAndSet()
        {
            var dob = new DateTime(1992, 6, 25);
            var dto = new UserUpdateDto
            {
                FirstName = "Updated",
                LastName = "Name",
                Gender = "Male",
                Dob = dob,
                Street = "Updated Street",
                City = "Updated City",
                State = "Updated State"
            };
            Assert.Equal("Updated", dto.FirstName);
            Assert.Equal("Name", dto.LastName);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal(dob, dto.Dob);
            Assert.Equal("Updated Street", dto.Street);
            Assert.Equal("Updated City", dto.City);
            Assert.Equal("Updated State", dto.State);
        }

        [Fact]
        public void UserLoginDto_DefaultConstructor_ShouldCreateInstance()
        {
            var dto = new UserLoginDto();
            Assert.NotNull(dto);
        }

        [Fact]
        public void UserLoginDto_Email_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserLoginDto();
            Assert.Equal(string.Empty, dto.Email);
        }

        [Fact]
        public void UserLoginDto_Password_DefaultValue_ShouldBeEmptyString()
        {
            var dto = new UserLoginDto();
            Assert.Equal(string.Empty, dto.Password);
        }

        [Fact]
        public void UserLoginDto_AllProperties_ShouldGetAndSet()
        {
            var dto = new UserLoginDto
            {
                Email = "login@example.com",
                Password = "mypassword"
            };
            Assert.Equal("login@example.com", dto.Email);
            Assert.Equal("mypassword", dto.Password);
        }
    }
}
