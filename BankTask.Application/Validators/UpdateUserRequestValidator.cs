using BankTask.Application.DTOs.Users;
using FluentValidation;

namespace BankTask.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
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
    }
}
