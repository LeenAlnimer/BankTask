using BankTask.Application.DTOs.Transactions;
using FluentValidation;

namespace BankTask.Application.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.TransactionType)
            .IsInEnum()
            .WithErrorCode("TRANSACTION_TYPE_INVALID");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithErrorCode("TRANSACTION_AMOUNT_INVALID");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithErrorCode("TRANSACTION_CURRENCY_REQUIRED")
            .Length(3)
            .WithErrorCode("CURRENCY_INVALID");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithErrorCode("TRANSACTION_DESCRIPTION_MAX_LENGTH");
    }
}
