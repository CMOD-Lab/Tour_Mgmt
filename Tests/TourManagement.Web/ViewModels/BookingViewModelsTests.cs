using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TourManagement.Web.ViewModels;
using Xunit;

namespace TourManagement.Web.ViewModels.Tests
{
    /// <summary>
    /// Unit tests for Booking ViewModels.
    /// </summary>
    public class BookingViewModelsTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        // ==================== BookingListViewModel Tests ====================

        [Fact]
        public void BookingListViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new BookingListViewModel();

            // Assert
            Assert.Equal(0, vm.BookingId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
        }

        [Fact]
        public void BookingListViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 15);

            // Act
            var vm = new BookingListViewModel
            {
                BookingId = 1,
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john@example.com",
                FirstName = "John",
                BookingDate = bookingDate
            };

            // Assert
            Assert.Equal(1, vm.BookingId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("Mumbai", vm.Place);
            Assert.Equal("john@example.com", vm.Email);
            Assert.Equal("John", vm.FirstName);
            Assert.Equal(bookingDate, vm.BookingDate);
        }

        // ==================== BookingDetailsViewModel Tests ====================

        [Fact]
        public void BookingDetailsViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new BookingDetailsViewModel();

            // Assert
            Assert.Equal(0, vm.BookingId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Null(vm.TourId);
        }

        [Fact]
        public void BookingDetailsViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 15);

            // Act
            var vm = new BookingDetailsViewModel
            {
                BookingId = 1,
                TourName = "Goa Tour",
                Place = "Mumbai",
                Email = "john@example.com",
                FirstName = "John",
                TourId = 5,
                BookingDate = bookingDate
            };

            // Assert
            Assert.Equal(1, vm.BookingId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("Mumbai", vm.Place);
            Assert.Equal("john@example.com", vm.Email);
            Assert.Equal("John", vm.FirstName);
            Assert.Equal(5, vm.TourId);
            Assert.Equal(bookingDate, vm.BookingDate);
        }

        // ==================== BookingCreateViewModel Tests ====================

        [Fact]
        public void BookingCreateViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new BookingCreateViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Null(vm.TourId);
        }

        [Fact]
        public void BookingCreateViewModel_WithValidData_PassesValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                FirstName = "John",
                Place = "Mumbai",
                TourName = "Goa Tour",
                Email = "john@example.com",
                TourId = 1
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void BookingCreateViewModel_WithEmptyFirstName_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                FirstName = "",
                Place = "Mumbai",
                TourName = "Goa Tour",
                Email = "john@example.com"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void BookingCreateViewModel_WithInvalidEmail_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                FirstName = "John",
                Place = "Mumbai",
                TourName = "Goa Tour",
                Email = "not-an-email"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void BookingCreateViewModel_WithEmptyTourName_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                FirstName = "John",
                Place = "Mumbai",
                TourName = "",
                Email = "john@example.com"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void BookingCreateViewModel_WithEmptyPlace_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                FirstName = "John",
                Place = "",
                TourName = "Goa Tour",
                Email = "john@example.com"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void BookingCreateViewModel_TourId_CanBeNull()
        {
            // Arrange & Act
            var vm = new BookingCreateViewModel { TourId = null };

            // Assert
            Assert.Null(vm.TourId);
        }

        [Fact]
        public void BookingCreateViewModel_TourId_CanBeSet()
        {
            // Arrange & Act
            var vm = new BookingCreateViewModel { TourId = 10 };

            // Assert
            Assert.Equal(10, vm.TourId);
        }

        // ==================== BookingDeleteViewModel Tests ====================

        [Fact]
        public void BookingDeleteViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new BookingDeleteViewModel();

            // Assert
            Assert.Equal(0, vm.BookingId);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.Email);
        }

        [Fact]
        public void BookingDeleteViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 15);

            // Act
            var vm = new BookingDeleteViewModel
            {
                BookingId = 1,
                TourName = "Goa Tour",
                FirstName = "John",
                Email = "john@example.com",
                BookingDate = bookingDate
            };

            // Assert
            Assert.Equal(1, vm.BookingId);
            Assert.Equal("Goa Tour", vm.TourName);
            Assert.Equal("John", vm.FirstName);
            Assert.Equal("john@example.com", vm.Email);
            Assert.Equal(bookingDate, vm.BookingDate);
        }
    }
}
