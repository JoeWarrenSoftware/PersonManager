using FluentValidation;
using UKParliament.CodeTest.Web.Contracts.Requests;

namespace UKParliament.CodeTest.Web.Validators;
public class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
{
    public UpdatePersonRequestValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(p => p.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");

        RuleFor(p => p.DepartmentId)
            .GreaterThan(0).WithMessage("Department ID must be greater than zero.");

        RuleFor(p => p.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .When(p => !string.IsNullOrEmpty(p.Email));

        RuleFor(p => p.PhoneNumber)
            .Matches(@"^\+?\d{7,15}$")
            .When(p => !string.IsNullOrEmpty(p.PhoneNumber))
            .WithMessage("Invalid phone number format (must be 7-15 digits).");

        RuleFor(p => p.ProfileImageUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .When(p => !string.IsNullOrEmpty(p.ProfileImageUrl))
            .WithMessage("Invalid profile image URL.");

        RuleFor(p => p.IsActive)
            .NotNull().WithMessage("Active status must be provided.");
    }
}