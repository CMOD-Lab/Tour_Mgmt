using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using TourBooking.Web.ViewModels;

namespace TourBooking.Tests.Web.ViewModels
{
    /// <summary>
    /// Unit tests for User ViewModels.
    /// </summary>
    public class UserViewModelsTests
    {
        [Fact]
        public void LoginViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new LoginViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void LoginViewModel_Email_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new LoginViewModel();
            Assert.Equal(string.Empty, vm.Email);
        }

        [Fact]
        public void LoginViewModel_Password_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new LoginViewModel();
            Assert.Equal(string.Empty, vm.Password);
        }

        [Fact]
        public void LoginViewModel_Email_ShouldGetAndSet()
        {
            var vm = new LoginViewModel();
            vm.Email = "user@example.com";
            Assert.Equal("user@example.com", vm.Email);
        }

        [Fact]
        public void LoginViewModel_Password_ShouldGetAndSet()
        {
            var vm = new LoginViewModel();
            vm.Password = "mypassword";
            Assert.Equal("mypassword", vm.Password);
        }

        [Fact]
        public void LoginViewModel_WithValidData_ShouldPassValidation()
        {
            var vm = new LoginViewModel
            {
                Email = "user@example.com",
                Password = "password123"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void LoginViewModel_WithEmptyEmail_ShouldFailValidation()
        {
            var vm = new LoginViewModel { Email = "", Password = "password123" };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void LoginViewModel_WithInvalidEmail_ShouldFailValidation()
        {
            var vm = new LoginViewModel { Email = "not-an-email", Password = "password123" };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void LoginViewModel_WithEmptyPassword_ShouldFailValidation()
        {
            var vm = new LoginViewModel { Email = "user@example.com", Password = "" };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void RegisterViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new RegisterViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void RegisterViewModel_Email_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new RegisterViewModel();
            Assert.Equal(string.Empty, vm.Email);
        }

        [Fact]
        public void RegisterViewModel_WithValidData_ShouldPassValidation()
        {
            var vm = new RegisterViewModel
            {
                Email = "newuser@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "password123",
                Dob = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.True(isValid);
        }

        [Fact]
        public void RegisterViewModel_WithEmptyFirstName_ShouldFailValidation()
        {
            var vm = new RegisterViewModel
            {
                Email = "user@example.com",
                FirstName = "",
                LastName = "Doe",
                Gender = "Male",
                Password = "password123",
                Dob = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void RegisterViewModel_AllProperties_ShouldGetAndSet()
        {
            var dob = new DateTime(1990, 5, 15);
            var vm = new RegisterViewModel
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                Gender = "Female",
                Password = "testpass",
                Dob = dob,
                Street = "Test Street",
                City = "Test City",
                State = "Test State"
            };
            Assert.Equal("test@test.com", vm.Email);
            Assert.Equal("Test", vm.FirstName);
            Assert.Equal("User", vm.LastName);
            Assert.Equal("Female", vm.Gender);
            Assert.Equal("testpass", vm.Password);
            Assert.Equal(dob, vm.Dob);
            Assert.Equal("Test Street", vm.Street);
            Assert.Equal("Test City", vm.City);
            Assert.Equal("Test State", vm.State);
        }

        [Fact]
        public void UserProfileViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new UserProfileViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void UserProfileViewModel_Email_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new UserProfileViewModel();
            Assert.Equal(string.Empty, vm.Email);
        }

        [Fact]
        public void UserProfileViewModel_AllProperties_ShouldGetAndSet()
        {
            var dob = new DateTime(1985, 3, 20);
            var vm = new UserProfileViewModel
            {
                Email = "profile@test.com",
                FirstName = "Profile",
                LastName = "User",
                Gender = "Male",
                Dob = dob,
                Street = "Profile Street",
                City = "Profile City",
                State = "Profile State"
            };
            Assert.Equal("profile@test.com", vm.Email);
            Assert.Equal("Profile", vm.FirstName);
            Assert.Equal("User", vm.LastName);
            Assert.Equal("Male", vm.Gender);
            Assert.Equal(dob, vm.Dob);
            Assert.Equal("Profile Street", vm.Street);
            Assert.Equal("Profile City", vm.City);
            Assert.Equal("Profile State", vm.State);
        }
    }
}
