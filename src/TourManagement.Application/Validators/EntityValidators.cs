using FluentValidation;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Validators;

/// <summary>
/// Validator for UserInfo entity.
/// </summary>
public class UserValidator : AbstractValidator<UserInfo>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(50).WithMessage("Email must not exceed 50 characters.");

        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(u => u.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(g => g == "Male" || g == "Female").WithMessage("Gender must be 'Male' or 'Female'.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");

        RuleFor(u => u.Dob)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(u => u.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(50).WithMessage("Street must not exceed 50 characters.");

        RuleFor(u => u.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

        RuleFor(u => u.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");
    }
}

/// <summary>
/// Validator for Tour entity.
/// </summary>
public class TourValidator : AbstractValidator<Tour>
{
    public TourValidator()
    {
        RuleFor(t => t.TourName)
            .NotEmpty().WithMessage("Tour name is required.")
            .MaximumLength(20).WithMessage("Tour name must not exceed 20 characters.");

        RuleFor(t => t.Place)
            .NotEmpty().WithMessage("Place is required.")
            .MaximumLength(20).WithMessage("Place must not exceed 20 characters.");

        RuleFor(t => t.Days)
            .GreaterThan(0).WithMessage("Days must be greater than 0.");

        RuleFor(t => t.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(t => t.Locations)
            .NotEmpty().WithMessage("Locations are required.")
            .MaximumLength(100).WithMessage("Locations must not exceed 100 characters.");

        RuleFor(t => t.TourInfo)
            .NotEmpty().WithMessage("Tour information is required.")
            .MaximumLength(200).WithMessage("Tour information must not exceed 200 characters.");
    }
}

/// <summary>
/// Validator for Booking entity.
/// </summary>
public class BookingValidator : AbstractValidator<Booking>
{
    public BookingValidator()
    {
        RuleFor(b => b.TourName)
            .NotEmpty().WithMessage("Tour name is required.")
            .MaximumLength(50).WithMessage("Tour name must not exceed 50 characters.");

        RuleFor(b => b.Place)
            .NotEmpty().WithMessage("Place is required.")
            .MaximumLength(50).WithMessage("Place must not exceed 50 characters.");

        RuleFor(b => b.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(50).WithMessage("Email must not exceed 50 characters.");

        RuleFor(b => b.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
    }
}
