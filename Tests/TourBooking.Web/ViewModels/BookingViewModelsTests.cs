using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using TourBooking.Web.ViewModels;

namespace TourBooking.Tests.Web.ViewModels
{
    /// <summary>
    /// Unit tests for Booking ViewModels.
    /// </summary>
    public class BookingViewModelsTests
    {
        [Fact]
        public void BookingViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new BookingViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void BookingViewModel_TourId_ShouldGetAndSet()
        {
            var vm = new BookingViewModel();
            vm.TourId = 10;
            Assert.Equal(10, vm.TourId);
        }

        [Fact]
        public void BookingViewModel_TourName_ShouldGetAndSet()
        {
            var vm = new BookingViewModel();
            vm.TourName = "Goa Tour";
            Assert.Equal("Goa Tour", vm.TourName);
        }

        [Fact]
        public void BookingViewModel_Place_ShouldGetAndSet()
        {
            var vm = new BookingViewModel();
            vm.Place = "Goa";
            Assert.Equal("Goa", vm.Place);
        }

        [Fact]
        public void BookingViewModel_Email_ShouldGetAndSet()
        {
            var vm = new BookingViewModel();
            vm.Email = "user@example.com";
            Assert.Equal("user@example.com", vm.Email);
        }

        [Fact]
        public void BookingViewModel_FirstName_ShouldGetAndSet()
        {
            var vm = new BookingViewModel();
            vm.FirstName = "John";
            Assert.Equal("John", vm.FirstName);
        }

        [Fact]
        public void BookingViewModel_NullableProperties_ShouldDefaultToNull()
        {
            var vm = new BookingViewModel();
            Assert.Null(vm.TourName);
            Assert.Null(vm.Place);
            Assert.Null(vm.Email);
            Assert.Null(vm.FirstName);
        }

        [Fact]
        public void BookingViewModel_AllProperties_ShouldBeSetCorrectly()
        {
            var vm = new BookingViewModel
            {
                TourId = 3,
                TourName = "Kashmir Tour",
                Place = "Kashmir",
                Email = "user@test.com",
                FirstName = "Alice"
            };
            Assert.Equal(3, vm.TourId);
            Assert.Equal("Kashmir Tour", vm.TourName);
            Assert.Equal("Kashmir", vm.Place);
            Assert.Equal("user@test.com", vm.Email);
            Assert.Equal("Alice", vm.FirstName);
        }

        [Fact]
        public void BookingCreateViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new BookingCreateViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void BookingCreateViewModel_TourName_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new BookingCreateViewModel();
            Assert.Equal(string.Empty, vm.TourName);
        }

        [Fact]
        public void BookingCreateViewModel_Place_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new BookingCreateViewModel();
            Assert.Equal(string.Empty, vm.Place);
        }

        [Fact]
        public void BookingCreateViewModel_Email_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new BookingCreateViewModel();
            Assert.Equal(string.Empty, vm.Email);
        }

        [Fact]
        public void BookingCreateViewModel_FirstName_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new BookingCreateViewModel();
            Assert.Equal(string.Empty, vm.FirstName);
        }

        [Fact]
        public void BookingCreateViewModel_WithValidData_ShouldPassValidation()
        {
            var vm = new BookingCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Email = "user@example.com",
                FirstName = "John"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.True(isValid);
        }

        [Fact]
        public void BookingCreateViewModel_WithEmptyTourName_ShouldFailValidation()
        {
            var vm = new BookingCreateViewModel
            {
                TourName = "",
                Place = "Goa",
                Email = "user@example.com",
                FirstName = "John"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void BookingCreateViewModel_WithInvalidEmail_ShouldFailValidation()
        {
            var vm = new BookingCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Email = "not-an-email",
                FirstName = "John"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void BookingCreateViewModel_WithEmptyEmail_ShouldFailValidation()
        {
            var vm = new BookingCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Email = "",
                FirstName = "John"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void BookingCreateViewModel_WithEmptyFirstName_ShouldFailValidation()
        {
            var vm = new BookingCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Email = "user@example.com",
                FirstName = ""
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void BookingCreateViewModel_AllProperties_ShouldBeSetCorrectly()
        {
            var vm = new BookingCreateViewModel
            {
                TourName = "Kerala Tour",
                Place = "Kerala",
                Email = "booking@test.com",
                FirstName = "Bob"
            };
            Assert.Equal("Kerala Tour", vm.TourName);
            Assert.Equal("Kerala", vm.Place);
            Assert.Equal("booking@test.com", vm.Email);
            Assert.Equal("Bob", vm.FirstName);
        }
    }
}
