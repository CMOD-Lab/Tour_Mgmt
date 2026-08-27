using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TourManagement.Web.ViewModels;
using Xunit;

namespace TourManagement.Web.ViewModels.Tests
{
    /// <summary>
    /// Unit tests for Tour ViewModels.
    /// </summary>
    public class TourViewModelsTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        // ==================== TourListViewModel Tests ====================

        [Fact]
        public void TourListViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new TourListViewModel();

            // Assert
            Assert.Equal(0, vm.TourId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(0, vm.Days);
            Assert.Equal(0m, vm.Price);
            Assert.Equal(string.Empty, vm.Locations);
            Assert.Null(vm.Pic);
        }

        [Fact]
        public void TourListViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var vm = new TourListViewModel
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                Pic = "goa.jpg"
            };

            // Assert
            Assert.Equal(1, vm.TourId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("Goa", vm.Place);
            Assert.Equal(5, vm.Days);
            Assert.Equal(15000m, vm.Price);
            Assert.Equal("Goa Beach", vm.Locations);
            Assert.Equal("goa.jpg", vm.Pic);
        }

        // ==================== TourDetailsViewModel Tests ====================

        [Fact]
        public void TourDetailsViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new TourDetailsViewModel();

            // Assert
            Assert.Equal(0, vm.TourId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(0, vm.Days);
            Assert.Equal(0m, vm.Price);
            Assert.Equal(string.Empty, vm.Locations);
            Assert.Equal(string.Empty, vm.TourInfo);
            Assert.Null(vm.Pic);
        }

        [Fact]
        public void TourDetailsViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            var vm = new TourDetailsViewModel
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches",
                Pic = "goa.jpg",
                CreatedDate = createdDate
            };

            // Assert
            Assert.Equal(1, vm.TourId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("Goa", vm.Place);
            Assert.Equal(5, vm.Days);
            Assert.Equal(15000m, vm.Price);
            Assert.Equal("Goa Beach", vm.Locations);
            Assert.Equal("Beautiful beaches", vm.TourInfo);
            Assert.Equal("goa.jpg", vm.Pic);
            Assert.Equal(createdDate, vm.CreatedDate);
        }

        // ==================== TourCreateViewModel Tests ====================

        [Fact]
        public void TourCreateViewModel_WithValidData_PassesValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void TourCreateViewModel_WithEmptyTourName_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void TourCreateViewModel_WithZeroDays_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 0,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void TourCreateViewModel_WithNegativePrice_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = -100m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void TourCreateViewModel_WithTourNameExceeding100Chars_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = new string('A', 101),
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        // ==================== TourEditViewModel Tests ====================

        [Fact]
        public void TourEditViewModel_WithValidData_PassesValidation()
        {
            // Arrange
            var vm = new TourEditViewModel
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m,
                Locations = "Goa Beach",
                TourInfo = "Beautiful beaches",
                IsActive = true
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void TourEditViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new TourEditViewModel();

            // Assert
            Assert.Equal(0, vm.TourId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.True(vm.IsActive);
        }

        // ==================== TourDeleteViewModel Tests ====================

        [Fact]
        public void TourDeleteViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new TourDeleteViewModel();

            // Assert
            Assert.Equal(0, vm.TourId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(0, vm.Days);
            Assert.Equal(0m, vm.Price);
        }

        [Fact]
        public void TourDeleteViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var vm = new TourDeleteViewModel
            {
                TourId = 1,
                TourName = "Goa Tour",
                Place = "Goa",
                Days = 5,
                Price = 15000m
            };

            // Assert
            Assert.Equal(1, vm.TourId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("Goa", vm.Place);
            Assert.Equal(5, vm.Days);
            Assert.Equal(15000m, vm.Price);
        }
    }
}
