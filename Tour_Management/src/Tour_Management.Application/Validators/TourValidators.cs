using FluentValidation;
using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Validators;

/// <summary>Validator for TourCreateDto.</summary>
public class TourCreateDtoValidator : AbstractValidator<TourCreateDto>
{
    public TourCreateDtoValidator()
    {
        RuleFor(x => x.TourName)
            .NotEmpty().WithMessage("Tour name is required.")
            .MaximumLength(20).WithMessage("Tour name cannot exceed 20 characters.");

        RuleFor(x => x.Place)
            .NotEmpty().WithMessage("Place is required.")
            .MaximumLength(20).WithMessage("Place cannot exceed 20 characters.");

        RuleFor(x => x.Days)
            .GreaterThan(0).WithMessage("Days must be greater than 0.")
            .LessThanOrEqualTo(99).WithMessage("Days cannot exceed 99.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Locations)
            .NotEmpty().WithMessage("Locations are required.")
            .MaximumLength(100).WithMessage("Locations cannot exceed 100 characters.");

        RuleFor(x => x.TourInfo)
            .NotEmpty().WithMessage("Tour information is required.")
            .MaximumLength(200).WithMessage("Tour information cannot exceed 200 characters.");
    }
}

/// <summary>Validator for TourUpdateDto.</summary>
public class TourUpdateDtoValidator : AbstractValidator<TourUpdateDto>
{
    public TourUpdateDtoValidator()
    {
        RuleFor(x => x.TourId).GreaterThan(0).WithMessage("Valid Tour ID is required.");
        RuleFor(x => x.TourName).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Place).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Days).GreaterThan(0).LessThanOrEqualTo(99);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Locations).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TourInfo).NotEmpty().MaximumLength(200);
    }
}
