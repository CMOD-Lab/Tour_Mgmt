using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TourManagement.Web.ViewModels;
using Xunit;

namespace TourManagement.Web.ViewModels.Tests
{
    /// <summary>
    /// Unit tests for User ViewModels.
    /// </summary>
    public class UserViewModelsTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        // ==================== LoginViewModel Tests ====================

        [Fact]
        public void LoginViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new LoginViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.Password);
        }

        [Fact]
        public void LoginViewModel_WithValidData_PassesValidation()
        {
            // Arrange
            var vm = new LoginViewModel
            {
                Email = "john@example.com",
                Password = "Password123!"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void LoginViewModel_WithEmptyEmail_FailsValidation()
        {
            // Arrange
            var vm = new LoginViewModel
            {
                Email = "",
                Password = "Password123!"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void LoginViewModel_WithInvalidEmail_FailsValidation()
        {
            // Arrange
            var vm = new LoginViewModel
            {
                Email = "not-an-email",
                Password = "Password123!"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void LoginViewModel_WithEmptyPassword_FailsValidation()
        {
            // Arrange
            var vm = new LoginViewModel
            {
                Email = "john@example.com",
                Password = ""
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        // ==================== RegisterViewModel Tests ====================

        [Fact]
        public void RegisterViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new RegisterViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.LastName);
            Assert.Equal(string.Empty, vm.Gender);
            Assert.Equal(string.Empty, vm.Password);
            Assert.Equal(string.Empty, vm.ConfirmPassword);
            Assert.Equal(string.Empty, vm.Street);
            Assert.Equal(string.Empty, vm.City);
            Assert.Equal(string.Empty, vm.State);
        }

        [Fact]
        public void RegisterViewModel_WithValidData_PassesValidation()
        {
            // Arrange
            var vm = new RegisterViewModel
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void RegisterViewModel_WithEmptyEmail_FailsValidation()
        {
            // Arrange
            var vm = new RegisterViewModel
            {
                Email = "",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void RegisterViewModel_WithShortPassword_FailsValidation()
        {
            // Arrange
            var vm = new RegisterViewModel
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "abc",
                ConfirmPassword = "abc",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void RegisterViewModel_WithMismatchedPasswords_FailsValidation()
        {
            // Arrange
            var vm = new RegisterViewModel
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void RegisterViewModel_WithEmptyFirstName_FailsValidation()
        {
            // Arrange
            var vm = new RegisterViewModel
            {
                Email = "john@example.com",
                FirstName = "",
                LastName = "Doe",
                Gender = "Male",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = new DateTime(1990, 1, 1),
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        // ==================== UserProfileViewModel Tests ====================

        [Fact]
        public void UserProfileViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new UserProfileViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.LastName);
            Assert.Equal(string.Empty, vm.Gender);
            Assert.Equal(string.Empty, vm.Street);
            Assert.Equal(string.Empty, vm.City);
            Assert.Equal(string.Empty, vm.State);
        }

        [Fact]
        public void UserProfileViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var dob = new DateTime(1990, 5, 15);

            // Act
            var vm = new UserProfileViewModel
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = dob,
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Assert
            Assert.Equal("john@example.com", vm.Email);
            Assert.Equal("John", vm.FirstName);
            Assert.Equal("Doe", vm.LastName);
            Assert.Equal("Male", vm.Gender);
            Assert.Equal(dob, vm.DateOfBirth);
            Assert.Equal("123 Main St", vm.Street);
            Assert.Equal("Mumbai", vm.City);
            Assert.Equal("Maharashtra", vm.State);
        }

        // ==================== UserEditViewModel Tests ====================

        [Fact]
        public void UserEditViewModel_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var vm = new UserEditViewModel();

            // Assert
            Assert.Equal(string.Empty, vm.Email);
            Assert.Equal(string.Empty, vm.FirstName);
            Assert.Equal(string.Empty, vm.LastName);
            Assert.Equal(string.Empty, vm.Gender);
            Assert.Equal(string.Empty, vm.Street);
            Assert.Equal(string.Empty, vm.City);
            Assert.Equal(string.Empty, vm.State);
        }

        [Fact]
        public void UserEditViewModel_WithValidData_PassesValidation()
        {
            // Arrange
            var vm = new UserEditViewModel
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void UserEditViewModel_WithEmptyFirstName_FailsValidation()
        {
            // Arrange
            var vm = new UserEditViewModel
            {
                Email = "john@example.com",
                FirstName = "",
                LastName = "Doe",
                Gender = "Male",
                Street = "123 Main St",
                City = "Mumbai",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void UserEditViewModel_WithEmptyCity_FailsValidation()
        {
            // Arrange
            var vm = new UserEditViewModel
            {
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Street = "123 Main St",
                City = "",
                State = "Maharashtra"
            };

            // Act
            var results = ValidateModel(vm);

            // Assert
            Assert.NotEmpty(results);
        }
    }
}
