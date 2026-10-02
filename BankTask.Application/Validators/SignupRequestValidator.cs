using BankTask.Application.DTOs.Authentication;
using FluentValidation;

namespace BankTask.Application.Validators;

public class SignupRequestValidator : AbstractValidator<SignupRequest>
{
    public SignupRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithErrorCode("FULL_NAME_REQUIRED")
            .MinimumLength(3)
            .WithErrorCode("FULL_NAME_MIN_LENGTH")
            .MaximumLength(100)
            .WithErrorCode("FULL_NAME_MAX_LENGTH");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("EMAIL_REQUIRED")
            .EmailAddress()
            .WithErrorCode("EMAIL_INVALID")
            .MaximumLength(150)
            .WithErrorCode("EMAIL_MAX_LENGTH");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode("PASSWORD_REQUIRED")
            .MinimumLength(8)
            .WithErrorCode("PASSWORD_MIN_LENGTH")
            .MaximumLength(100)
            .WithErrorCode("PASSWORD_MAX_LENGTH");
    }
}
