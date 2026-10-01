using BankTask.Application.DTOs.Accounts;
using FluentValidation;

namespace BankTask.Application.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode("USER_ID_REQUIRED");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithErrorCode("CURRENCY_INVALID");

        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithErrorCode("ACCOUNT_TYPE_INVALID");
    }
}
