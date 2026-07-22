using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Tests.ViewModels
{
    public class UserViewModelTests
    {
        // ─── UserViewModel ───────────────────────────────────────────────────────

        [Fact]
        public void UserViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new UserViewModel();

            // Assert
            Assert.Equal(0, vm.Id);
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.LastName);
            Assert.Null(vm.Gender);
            Assert.Null(vm.DateOfBirth);
            Assert.Null(vm.Street);
            Assert.Null(vm.City);
            Assert.Null(vm.State);
            Assert.False(vm.IsAdmin);
            Assert.False(vm.IsActive);
        }

        [Fact]
        public void UserViewModel_FullName_ConcatenatesFirstAndLastName()
        {
            // Arrange
            var vm = new UserViewModel { FirstName = "John", LastName = "Doe" };

            // Act
            var fullName = vm.FullName;

            // Assert
            Assert.Equal("John Doe", fullName);
        }

        [Fact]
        public void UserViewModel_FullName_WithEmptyNames_ReturnsSpace()
        {
            // Arrange
            var vm = new UserViewModel { FirstName = string.Empty, LastName = string.Empty };

            // Act
            var fullName = vm.FullName;

            // Assert
            Assert.Equal(" ", fullName);
        }

        [Fact]
        public void UserViewModel_Properties_CanBeSet()
        {
            // Arrange & Act
            var dob = new DateTime(1990, 1, 1);
            var created = DateTime.UtcNow;
            var vm = new UserViewModel
            {
                Id = 5,
                Email = "test@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                Gender = "Female",
                DateOfBirth = dob,
                Street = "123 Main St",
                City = "Springfield",
                State = "IL",
                IsAdmin = true,
                CreatedDate = created,
                IsActive = true
            };

            // Assert
            Assert.Equal(5, vm.Id);
            Assert.Equal("test@example.com", vm.Email);
            Assert.Equal("Jane", vm.FirstName);
            Assert.Equal("Smith", vm.LastName);
            Assert.Equal("Female", vm.Gender);
            Assert.Equal(dob, vm.DateOfBirth);
            Assert.Equal("123 Main St", vm.Street);
            Assert.Equal("Springfield", vm.City);
            Assert.Equal("IL", vm.State);
            Assert.True(vm.IsAdmin);
            Assert.Equal(created, vm.CreatedDate);
            Assert.True(vm.IsActive);
        }

        // ─── UserRegisterViewModel ────────────────────────────────────────────────

        [Fact]
        public void UserRegisterViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new UserRegisterViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.LastName);
            Assert.Null(vm.Gender);
            Assert.Equal(string.Empty, vm.Password);
            Assert.Equal(string.Empty, vm.ConfirmPassword);
            Assert.Null(vm.DateOfBirth);
            Assert.Null(vm.Street);
            Assert.Null(vm.City);
            Assert.Null(vm.State);
        }

        [Fact]
        public void UserRegisterViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new UserRegisterViewModel
            {
                Email = "user@example.com",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void UserRegisterViewModel_MissingEmail_FailsValidation()
        {
            // Arrange
            var vm = new UserRegisterViewModel
            {
                Email = "",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void UserRegisterViewModel_InvalidEmail_FailsValidation()
        {
            // Arrange
            var vm = new UserRegisterViewModel
            {
                Email = "not-an-email",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void UserRegisterViewModel_ShortPassword_FailsValidation()
        {
            // Arrange
            var vm = new UserRegisterViewModel
            {
                Email = "user@example.com",
                FirstName = "Alice",
                LastName = "Wonder",
                Password = "abc",
                ConfirmPassword = "abc"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Password"));
        }

        [Fact]
        public void UserRegisterViewModel_MissingFirstName_FailsValidation()
        {
            // Arrange
            var vm = new UserRegisterViewModel
            {
                Email = "user@example.com",
                FirstName = "",
                LastName = "Wonder",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("FirstName"));
        }

        [Fact]
        public void UserRegisterViewModel_MissingLastName_FailsValidation()
        {
            // Arrange
            var vm = new UserRegisterViewModel
            {
                Email = "user@example.com",
                FirstName = "Alice",
                LastName = "",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("LastName"));
        }

        // ─── UserLoginViewModel ───────────────────────────────────────────────────

        [Fact]
        public void UserLoginViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new UserLoginViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.Password);
        }

        [Fact]
        public void UserLoginViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new UserLoginViewModel
            {
                Email = "user@example.com",
                Password = "password123"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void UserLoginViewModel_MissingEmail_FailsValidation()
        {
            // Arrange
            var vm = new UserLoginViewModel { Email = "", Password = "password123" };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void UserLoginViewModel_InvalidEmail_FailsValidation()
        {
            // Arrange
            var vm = new UserLoginViewModel { Email = "bad-email", Password = "password123" };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void UserLoginViewModel_MissingPassword_FailsValidation()
        {
            // Arrange
            var vm = new UserLoginViewModel { Email = "user@example.com", Password = "" };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Password"));
        }

        // ─── UserEditViewModel ────────────────────────────────────────────────────

        [Fact]
        public void UserEditViewModel_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var vm = new UserEditViewModel();

            // Assert
            Assert.Equal(0, vm.Id);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.LastName);
            Assert.Null(vm.Gender);
            Assert.Null(vm.DateOfBirth);
            Assert.Null(vm.Street);
            Assert.Null(vm.City);
            Assert.Null(vm.State);
            Assert.True(vm.IsActive);
        }

        [Fact]
        public void UserEditViewModel_ValidModel_PassesValidation()
        {
            // Arrange
            var vm = new UserEditViewModel
            {
                Id = 1,
                FirstName = "Bob",
                LastName = "Builder",
                IsActive = true
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void UserEditViewModel_MissingFirstName_FailsValidation()
        {
            // Arrange
            var vm = new UserEditViewModel { Id = 1, FirstName = "", LastName = "Builder" };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("FirstName"));
        }

        [Fact]
        public void UserEditViewModel_MissingLastName_FailsValidation()
        {
            // Arrange
            var vm = new UserEditViewModel { Id = 1, FirstName = "Bob", LastName = "" };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("LastName"));
        }

        [Fact]
        public void UserEditViewModel_Properties_CanBeSet()
        {
            // Arrange
            var dob = new DateTime(1985, 6, 15);
            var vm = new UserEditViewModel
            {
                Id = 10,
                FirstName = "Carol",
                LastName = "King",
                Gender = "Female",
                DateOfBirth = dob,
                Street = "456 Oak Ave",
                City = "Shelbyville",
                State = "TN",
                IsActive = false
            };

            // Assert
            Assert.Equal(10, vm.Id);
            Assert.Equal("Carol", vm.FirstName);
            Assert.Equal("King", vm.LastName);
            Assert.Equal("Female", vm.Gender);
            Assert.Equal(dob, vm.DateOfBirth);
            Assert.Equal("456 Oak Ave", vm.Street);
            Assert.Equal("Shelbyville", vm.City);
            Assert.Equal("TN", vm.State);
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
