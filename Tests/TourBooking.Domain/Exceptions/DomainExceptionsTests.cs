using System;
using System.Collections.Generic;
using Xunit;
using TourBooking.Domain.Exceptions;

namespace TourBooking.Tests.Domain.Exceptions
{
    /// <summary>
    /// Unit tests for domain exception classes.
    /// </summary>
    public class DomainExceptionsTests
    {
        [Fact]
        public void NotFoundException_WithMessage_ShouldSetMessage()
        {
            var ex = new NotFoundException("Entity not found");
            Assert.Equal("Entity not found", ex.Message);
        }

        [Fact]
        public void NotFoundException_WithEntityNameAndKey_ShouldFormatMessage()
        {
            var ex = new NotFoundException("Tour", 42);
            Assert.Contains("Tour", ex.Message);
            Assert.Contains("42", ex.Message);
        }

        [Fact]
        public void NotFoundException_ShouldInheritFromException()
        {
            var ex = new NotFoundException("Not found");
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void NotFoundException_WithEntityNameAndStringKey_ShouldFormatMessage()
        {
            var ex = new NotFoundException("UserInfo", "user@example.com");
            Assert.Contains("UserInfo", ex.Message);
            Assert.Contains("user@example.com", ex.Message);
        }

        [Fact]
        public void NotFoundException_CanBeCaught_AsException()
        {
            Exception? caught = null;
            try { throw new NotFoundException("Test entity", 1); }
            catch (Exception ex) { caught = ex; }
            Assert.NotNull(caught);
            Assert.IsType<NotFoundException>(caught);
        }

        [Fact]
        public void DuplicateEntityException_WithMessage_ShouldSetMessage()
        {
            var ex = new DuplicateEntityException("Duplicate entity found");
            Assert.Equal("Duplicate entity found", ex.Message);
        }

        [Fact]
        public void DuplicateEntityException_ShouldInheritFromException()
        {
            var ex = new DuplicateEntityException("Duplicate");
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void DuplicateEntityException_CanBeCaught_AsException()
        {
            Exception? caught = null;
            try { throw new DuplicateEntityException("Duplicate user"); }
            catch (Exception ex) { caught = ex; }
            Assert.NotNull(caught);
            Assert.IsType<DuplicateEntityException>(caught);
        }

        [Fact]
        public void ValidationException_WithErrors_ShouldSetErrors()
        {
            var errors = new List<string> { "Field is required", "Invalid email" };
            var ex = new TourBooking.Domain.Exceptions.ValidationException(errors);
            Assert.NotNull(ex.Errors);
            Assert.Equal(2, ((List<string>)ex.Errors).Count);
        }

        [Fact]
        public void ValidationException_ShouldHaveDefaultMessage()
        {
            var errors = new List<string> { "Error 1" };
            var ex = new TourBooking.Domain.Exceptions.ValidationException(errors);
            Assert.Equal("One or more validation errors occurred.", ex.Message);
        }

        [Fact]
        public void ValidationException_ShouldInheritFromException()
        {
            var ex = new TourBooking.Domain.Exceptions.ValidationException(new List<string> { "Error" });
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void ValidationException_WithEmptyErrors_ShouldHaveEmptyErrorsList()
        {
            var errors = new List<string>();
            var ex = new TourBooking.Domain.Exceptions.ValidationException(errors);
            Assert.NotNull(ex.Errors);
            Assert.Empty(ex.Errors);
        }

        [Fact]
        public void ValidationException_WithSingleError_ShouldContainError()
        {
            var errors = new List<string> { "Name is required" };
            var ex = new TourBooking.Domain.Exceptions.ValidationException(errors);
            Assert.Contains("Name is required", ex.Errors);
        }

        [Fact]
        public void ValidationException_CanBeCaught_AsException()
        {
            Exception? caught = null;
            try { throw new TourBooking.Domain.Exceptions.ValidationException(new List<string> { "Validation failed" }); }
            catch (Exception ex) { caught = ex; }
            Assert.NotNull(caught);
            Assert.IsType<TourBooking.Domain.Exceptions.ValidationException>(caught);
        }
    }
}
