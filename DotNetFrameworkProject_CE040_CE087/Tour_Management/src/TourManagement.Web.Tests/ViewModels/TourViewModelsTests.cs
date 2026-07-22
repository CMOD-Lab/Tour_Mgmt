using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.ViewModels
{
    public class TourViewModelsTests
    {
        // ─── TourViewModel ────────────────────────────────────────────────────────

        [Fact]
        public void TourViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new TourViewModel();

            // Assert
            Assert.Equal(0, vm.Id);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(0, vm.Days);
            Assert.Equal(0m, vm.Price);
            Assert.Equal(string.Empty, vm.Locations);
            Assert.Equal(string.Empty, vm.TourInfo);
            Assert.Null(vm.Pic);
            Assert.False(vm.IsActive);
        }

        [Fact]
        public void TourViewModel_Properties_CanBeSet()
        {
            // Arrange
            var created = DateTime.UtcNow;
            var vm = new TourViewModel
            {
                Id = 1,
                TourName = "Paris Adventure",
                Place = "Paris",
                Days = 7,
                Price = 1500.00m,
                Locations = "Eiffel Tower, Louvre",
                TourInfo = "A wonderful trip to Paris",
                Pic = "paris.jpg",
                CreatedDate = created,
                IsActive = true
            };

            // Assert
            Assert.Equal(1, vm.Id);
            Assert.Equal("Paris Adventure", vm.TourName);
            Assert.Equal("Paris", vm.Place);
            Assert.Equal(7, vm.Days);
            Assert.Equal(1500.00m, vm.Price);
            Assert.Equal("Eiffel Tower, Louvre", vm.Locations);
            Assert.Equal("A wonderful trip to Paris", vm.TourInfo);
            Assert.Equal("paris.jpg", vm.Pic);
            Assert.Equal(created, vm.CreatedDate);
            Assert.True(vm.IsActive);
        }

        // ─── TourCreateViewModel ──────────────────────────────────────────────────

        [Fact]
        public void TourCreateViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new TourCreateViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(0, vm.Days);
            Assert.Equal(0m, vm.Price);
            Assert.Equal(string.Empty, vm.Locations);
            Assert.Equal(string.Empty, vm.TourInfo);
            Assert.Null(vm.PicFile);
        }

        [Fact]
        public void TourCreateViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Days = 5,
                Price = 1200.00m,
                Locations = "Colosseum, Vatican",
                TourInfo = "Explore ancient Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void TourCreateViewModel_MissingTourName_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "",
                Place = "Rome",
                Days = 5,
                Price = 1200.00m,
                Locations = "Colosseum",
                TourInfo = "Explore Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("TourName"));
        }

        [Fact]
        public void TourCreateViewModel_MissingPlace_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "",
                Days = 5,
                Price = 1200.00m,
                Locations = "Colosseum",
                TourInfo = "Explore Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Place"));
        }

        [Fact]
        public void TourCreateViewModel_ZeroDays_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Days = 0,
                Price = 1200.00m,
                Locations = "Colosseum",
                TourInfo = "Explore Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Days"));
        }

        [Fact]
        public void TourCreateViewModel_DaysExceedingMax_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Days = 400,
                Price = 1200.00m,
                Locations = "Colosseum",
                TourInfo = "Explore Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Days"));
        }

        [Fact]
        public void TourCreateViewModel_ZeroPrice_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Days = 5,
                Price = 0m,
                Locations = "Colosseum",
                TourInfo = "Explore Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Price"));
        }

        [Fact]
        public void TourCreateViewModel_MissingLocations_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Days = 5,
                Price = 1200.00m,
                Locations = "",
                TourInfo = "Explore Rome"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Locations"));
        }

        [Fact]
        public void TourCreateViewModel_MissingTourInfo_FailsValidation()
        {
            // Arrange
            var vm = new TourCreateViewModel
            {
                TourName = "Rome Tour",
                Place = "Rome",
                Days = 5,
                Price = 1200.00m,
                Locations = "Colosseum",
                TourInfo = ""
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("TourInfo"));
        }

        // ─── TourEditViewModel ────────────────────────────────────────────────────

        [Fact]
        public void TourEditViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new TourEditViewModel();

            // Assert
            Assert.Equal(0, vm.Id);
            Assert.Equal(string.Empty, vm.TourName);
            Assert.Equal(string.Empty, vm.Place);
            Assert.Equal(0, vm.Days);
            Assert.Equal(0m, vm.Price);
            Assert.Equal(string.Empty, vm.Locations);
            Assert.Equal(string.Empty, vm.TourInfo);
            Assert.Null(vm.CurrentPic);
            Assert.Null(vm.PicFile);
            Assert.True(vm.IsActive);
        }

        [Fact]
        public void TourEditViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new TourEditViewModel
            {
                Id = 1,
                TourName = "Updated Tour",
                Place = "London",
                Days = 3,
                Price = 800.00m,
                Locations = "Big Ben, Tower Bridge",
                TourInfo = "Explore London",
                IsActive = true
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void TourEditViewModel_MissingTourName_FailsValidation()
        {
            // Arrange
            var vm = new TourEditViewModel
            {
                Id = 1,
                TourName = "",
                Place = "London",
                Days = 3,
                Price = 800.00m,
                Locations = "Big Ben",
                TourInfo = "Explore London"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("TourName"));
        }

        [Fact]
        public void TourEditViewModel_Properties_CanBeSet()
        {
            // Arrange
            var vm = new TourEditViewModel
            {
                Id = 42,
                TourName = "Tokyo Tour",
                Place = "Tokyo",
                Days = 10,
                Price = 3000.00m,
                Locations = "Shibuya, Shinjuku",
                TourInfo = "Explore Tokyo",
                CurrentPic = "tokyo_old.jpg",
                IsActive = false
            };

            // Assert
            Assert.Equal(42, vm.Id);
            Assert.Equal("Tokyo Tour", vm.TourName);
            Assert.Equal("Tokyo", vm.Place);
            Assert.Equal(10, vm.Days);
            Assert.Equal(3000.00m, vm.Price);
            Assert.Equal("Shibuya, Shinjuku", vm.Locations);
            Assert.Equal("Explore Tokyo", vm.TourInfo);
            Assert.Equal("tokyo_old.jpg", vm.CurrentPic);
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
