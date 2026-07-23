using FluentValidation;
using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Validators;

/// <summary>Validator for BookingCreateDto.</summary>
public class BookingCreateDtoValidator : AbstractValidator<BookingCreateDto>
{
    public BookingCreateDtoValidator()
    {
        RuleFor(x => x.TourName)
            .NotEmpty().WithMessage("Tour name is required.")
            .MaximumLength(50).WithMessage("Tour name cannot exceed 50 characters.");

        RuleFor(x => x.Place)
            .NotEmpty().WithMessage("Place is required.")
            .MaximumLength(50).WithMessage("Place cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
    }
}
