using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.ViewModels
{
    public class BookingViewModelsTests
    {
        // ─── BookingViewModel ─────────────────────────────────────────────────────

        [Fact]
        public void BookingViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new BookingViewModel();

            // Assert
            Assert.Equal(0, vm.Id);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Null(vm.TourId);
            Assert.False(vm.IsActive);
        }

        [Fact]
        public void BookingViewModel_Properties_CanBeSet()
        {
            // Arrange
            var bookingDate = new DateTime(2024, 6, 1);
            var createdDate = DateTime.UtcNow;
            var vm = new BookingViewModel
            {
                Id = 10,
                TourName = "Paris Tour",
                Place = "Paris",
                Email = "user@example.com",
                FirstName = "Alice",
                TourId = 5,
                BookingDate = bookingDate,
                CreatedDate = createdDate,
                IsActive = true
            };

            // Assert
            Assert.Equal(10, vm.Id);
            Assert.Equal("Paris Tour", vm.TourName);
            Assert.Equal("Paris", vm.Place);
            Assert.Equal("user@example.com", vm.Email);
            Assert.Equal("Alice", vm.FirstName);
            Assert.Equal(5, vm.TourId);
            Assert.Equal(bookingDate, vm.BookingDate);
            Assert.Equal(createdDate, vm.CreatedDate);
            Assert.True(vm.IsActive);
        }

        [Fact]
        public void BookingViewModel_TourId_CanBeNull()
        {
            // Arrange & Act
            var vm = new BookingViewModel { TourId = null };

            // Assert
            Assert.Null(vm.TourId);
        }

        // ─── BookingCreateViewModel ───────────────────────────────────────────────

        [Fact]
        public void BookingCreateViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new BookingCreateViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Null(vm.TourId);
        }

        [Fact]
        public void BookingCreateViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Email = "user@example.com",
                FirstName = "Bob",
                TourId = 3
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void BookingCreateViewModel_MissingTourName_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "",
                Place = "Rome",
                Email = "user@example.com",
                FirstName = "Bob"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("TourName"));
        }

        [Fact]
        public void BookingCreateViewModel_MissingPlace_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "",
                Email = "user@example.com",
                FirstName = "Bob"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Place"));
        }

        [Fact]
        public void BookingCreateViewModel_MissingEmail_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Email = "",
                FirstName = "Bob"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void BookingCreateViewModel_InvalidEmail_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Email = "not-an-email",
                FirstName = "Bob"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void BookingCreateViewModel_MissingFirstName_FailsValidation()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Email = "user@example.com",
                FirstName = ""
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("FirstName"));
        }

        [Fact]
        public void BookingCreateViewModel_TourId_CanBeNull()
        {
            // Arrange
            var vm = new BookingCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Email = "user@example.com",
                FirstName = "Bob",
                TourId = null
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
            Assert.Null(vm.TourId);
        }

        // ─── BookingEditViewModel ─────────────────────────────────────────────────

        [Fact]
        public void BookingEditViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new BookingEditViewModel();

            // Assert
            Assert.Equal(0, vm.Id);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.True(vm.IsActive);
        }

        [Fact]
        public void BookingEditViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new BookingEditViewModel
            {
                Id = 1,
                TourName = "London Tour",
                Place = "London",
                Email = "user@example.com",
                FirstName = "Carol",
                IsActive = true
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void BookingEditViewModel_MissingTourName_FailsValidation()
        {
            // Arrange
            var vm = new BookingEditViewModel
            {
                Id = 1,
                TourName = "",
                Place = "London",
                Email = "user@example.com",
                FirstName = "Carol"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("TourName"));
        }

        [Fact]
        public void BookingEditViewModel_InvalidEmail_FailsValidation()
        {
            // Arrange
            var vm = new BookingEditViewModel
            {
                Id = 1,
                TourName = "London Tour",
                Place = "London",
                Email = "bad-email",
                FirstName = "Carol"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void BookingEditViewModel_Properties_CanBeSet()
        {
            // Arrange
            var vm = new BookingEditViewModel
            {
                Id = 99,
                TourName = "Tokyo Tour",
                Place = "Tokyo",
                Email = "admin@example.com",
                FirstName = "Dave",
                IsActive = false
            };

            // Assert
            Assert.Equal(99, vm.Id);
            Assert.Equal("Tokyo Tour", vm.TourName);
            Assert.Equal("Tokyo", vm.Place);
            Assert.Equal("admin@example.com", vm.Email);
            Assert.Equal("Dave", vm.FirstName);
            Assert.False(vm.IsActive);
        }

        // ─── Helper ──────────────────────────────────────────────────────────────

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }
    }
}
