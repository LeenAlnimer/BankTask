using BankTask.Application.DTOs.Accounts;
using FluentValidation;

namespace BankTask.Application.Validators;

public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithErrorCode("ACCOUNT_STATUS_INVALID");
    }
}
