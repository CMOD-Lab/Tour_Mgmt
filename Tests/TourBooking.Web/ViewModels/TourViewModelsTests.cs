using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using TourBooking.Web.ViewModels;

namespace TourBooking.Tests.Web.ViewModels
{
    /// <summary>
    /// Unit tests for Tour ViewModels.
    /// </summary>
    public class TourViewModelsTests
    {
        [Fact]
        public void TourViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new TourViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void TourViewModel_TourId_ShouldGetAndSet()
        {
            var vm = new TourViewModel();
            vm.TourId = 5;
            Assert.Equal(5, vm.TourId);
        }

        [Fact]
        public void TourViewModel_TourName_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new TourViewModel();
            Assert.Equal(string.Empty, vm.TourName);
        }

        [Fact]
        public void TourViewModel_Place_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new TourViewModel();
            Assert.Equal(string.Empty, vm.Place);
        }

        [Fact]
        public void TourViewModel_Days_ShouldGetAndSet()
        {
            var vm = new TourViewModel();
            vm.Days = 7;
            Assert.Equal(7, vm.Days);
        }

        [Fact]
        public void TourViewModel_Price_ShouldGetAndSet()
        {
            var vm = new TourViewModel();
            vm.Price = 15000.50m;
            Assert.Equal(15000.50m, vm.Price);
        }

        [Fact]
        public void TourViewModel_Pic_ShouldDefaultToNull()
        {
            var vm = new TourViewModel();
            Assert.Null(vm.Pic);
        }

        [Fact]
        public void TourViewModel_AllProperties_ShouldBeSetCorrectly()
        {
            var vm = new TourViewModel
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "North Goa, South Goa",
                TourInfo = "Beach paradise",
                Pic = "goa.jpg"
            };
            Assert.Equal(1, vm.TourId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("Goa", vm.Place);
            Assert.Equal(5, vm.Days);
            Assert.Equal(15000m, vm.Price);
            Assert.Equal("North Goa, South Goa", vm.Locations);
            Assert.Equal("Beach paradise", vm.TourInfo);
            Assert.Equal("goa.jpg", vm.Pic);
        }

        [Fact]
        public void TourCreateEditViewModel_DefaultConstructor_ShouldCreateInstance()
        {
            var vm = new TourCreateEditViewModel();
            Assert.NotNull(vm);
        }

        [Fact]
        public void TourCreateEditViewModel_TourId_DefaultValue_ShouldBeZero()
        {
            var vm = new TourCreateEditViewModel();
            Assert.Equal(0, vm.TourId);
        }

        [Fact]
        public void TourCreateEditViewModel_TourName_DefaultValue_ShouldBeEmptyString()
        {
            var vm = new TourCreateEditViewModel();
            Assert.Equal(string.Empty, vm.TourName);
        }

        [Fact]
        public void TourCreateEditViewModel_WithValidData_ShouldPassValidation()
        {
            var vm = new TourCreateEditViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "North Goa, South Goa",
                TourInfo = "Beautiful beach tour"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.True(isValid);
        }

        [Fact]
        public void TourCreateEditViewModel_WithEmptyTourName_ShouldFailValidation()
        {
            var vm = new TourCreateEditViewModel
            {
                TourName = "",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "North Goa",
                TourInfo = "Tour info"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void TourCreateEditViewModel_WithInvalidDays_ShouldFailValidation()
        {
            var vm = new TourCreateEditViewModel
            {
                TourName = "Tour",
                Place = "Place",
                Days = 0,
                Price = 15000m,
                Locations = "Locations",
                TourInfo = "Info"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void TourCreateEditViewModel_WithInvalidPrice_ShouldFailValidation()
        {
            var vm = new TourCreateEditViewModel
            {
                TourName = "Tour",
                Place = "Place",
                Days = 5,
                Price = 0m,
                Locations = "Locations",
                TourInfo = "Info"
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(vm, new ValidationContext(vm), validationResults, true);
            Assert.False(isValid);
        }

        [Fact]
        public void TourCreateEditViewModel_ExistingPic_ShouldGetAndSet()
        {
            var vm = new TourCreateEditViewModel();
            vm.ExistingPic = "existing.jpg";
            Assert.Equal("existing.jpg", vm.ExistingPic);
        }

        [Fact]
        public void TourCreateEditViewModel_PicFile_ShouldDefaultToNull()
        {
            var vm = new TourCreateEditViewModel();
            Assert.Null(vm.PicFile);
        }

        [Fact]
        public void TourCreateEditViewModel_AllProperties_ShouldBeSetCorrectly()
        {
            var vm = new TourCreateEditViewModel
            {
                TourId = 2,
                TourName = "Kerala Tour",
                Place = "Kerala",
                Days = 6,
                Price = 20000m,
                Locations = "Munnar, Alleppey",
                TourInfo = "Backwaters tour",
                ExistingPic = "kerala.jpg"
            };
            Assert.Equal(2, vm.TourId);
            Assert.Equal("Kerala Tour", vm.TourName);
            Assert.Equal("Kerala", vm.Place);
            Assert.Equal(6, vm.Days);
            Assert.Equal(20000m, vm.Price);
            Assert.Equal("Munnar, Alleppey", vm.Locations);
            Assert.Equal("Backwaters tour", vm.TourInfo);
            Assert.Equal("kerala.jpg", vm.ExistingPic);
        }
    }
}
