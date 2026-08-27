using System;
using System.Collections.Generic;
using TourManagement.Domain.Exceptions;
using Xunit;

namespace TourManagement.Domain.Exceptions.Tests
{
    /// <summary>
    /// Unit tests for domain exception classes.
    /// </summary>
    public class DomainExceptionsTests
    {
        // ==================== NotFoundException Tests ====================

        [Fact]
        public void NotFoundException_WithEntityNameAndKey_SetsCorrectMessage()
        {
            // Arrange & Act
            var ex = new NotFoundException("Tour", 42);

            // Assert
            Assert.Equal("Tour with key '42' was not found.", ex.Message);
        }

        [Fact]
        public void NotFoundException_WithCustomMessage_SetsCorrectMessage()
        {
            // Arrange & Act
            var ex = new NotFoundException("Custom error message");

            // Assert
            Assert.Equal("Custom error message", ex.Message);
        }

        [Fact]
        public void NotFoundException_IsExceptionType()
        {
            // Arrange & Act
            var ex = new NotFoundException("Tour", 1);

            // Assert
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void NotFoundException_WithStringKey_SetsCorrectMessage()
        {
            // Arrange & Act
            var ex = new NotFoundException("UserInfo", "john@example.com");

            // Assert
            Assert.Equal("UserInfo with key 'john@example.com' was not found.", ex.Message);
        }

        [Fact]
        public void NotFoundException_CanBeCaught_AsException()
        {
            // Arrange
            Exception? caught = null;

            // Act
            try
            {
                throw new NotFoundException("Tour", 1);
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            // Assert
            Assert.NotNull(caught);
            Assert.IsType<NotFoundException>(caught);
        }

        // ==================== ValidationException Tests ====================

        [Fact]
        public void ValidationException_WithErrors_SetsCorrectMessage()
        {
            // Arrange
            var errors = new Dictionary<string, string[]>
            {
                { "TourName", new[] { "Tour name is required" } },
                { "Price", new[] { "Price must be positive" } }
            };

            // Act
            var ex = new ValidationException(errors);

            // Assert
            Assert.Equal("One or more validation errors occurred.", ex.Message);
            Assert.Equal(2, ex.Errors.Count);
        }

        [Fact]
        public void ValidationException_WithErrors_ContainsCorrectErrors()
        {
            // Arrange
            var errors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Email is required", "Invalid email format" } }
            };

            // Act
            var ex = new ValidationException(errors);

            // Assert
            Assert.True(ex.Errors.ContainsKey("Email"));
            Assert.Equal(2, ex.Errors["Email"].Length);
        }

        [Fact]
        public void ValidationException_WithCustomMessage_SetsCorrectMessage()
        {
            // Arrange & Act
            var ex = new ValidationException("Validation failed");

            // Assert
            Assert.Equal("Validation failed", ex.Message);
            Assert.Empty(ex.Errors);
        }

        [Fact]
        public void ValidationException_IsExceptionType()
        {
            // Arrange & Act
            var ex = new ValidationException("Error");

            // Assert
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void ValidationException_WithEmptyErrors_HasEmptyErrorsDictionary()
        {
            // Arrange & Act
            var ex = new ValidationException("Error");

            // Assert
            Assert.NotNull(ex.Errors);
            Assert.Empty(ex.Errors);
        }

        [Fact]
        public void ValidationException_CanBeCaught_AsException()
        {
            // Arrange
            Exception? caught = null;

            // Act
            try
            {
                throw new ValidationException("Test error");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            // Assert
            Assert.NotNull(caught);
            Assert.IsType<ValidationException>(caught);
        }

        // ==================== DuplicateEntityException Tests ====================

        [Fact]
        public void DuplicateEntityException_WithEntityNameAndKey_SetsCorrectMessage()
        {
            // Arrange & Act
            var ex = new DuplicateEntityException("UserInfo", "john@example.com");

            // Assert
            Assert.Equal("UserInfo with key 'john@example.com' already exists.", ex.Message);
        }

        [Fact]
        public void DuplicateEntityException_WithIntKey_SetsCorrectMessage()
        {
            // Arrange & Act
            var ex = new DuplicateEntityException("Tour", 5);

            // Assert
            Assert.Equal("Tour with key '5' already exists.", ex.Message);
        }

        [Fact]
        public void DuplicateEntityException_IsExceptionType()
        {
            // Arrange & Act
            var ex = new DuplicateEntityException("Tour", 1);

            // Assert
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void DuplicateEntityException_CanBeCaught_AsException()
        {
            // Arrange
            Exception? caught = null;

            // Act
            try
            {
                throw new DuplicateEntityException("UserInfo", "test@test.com");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            // Assert
            Assert.NotNull(caught);
            Assert.IsType<DuplicateEntityException>(caught);
        }
    }
}
