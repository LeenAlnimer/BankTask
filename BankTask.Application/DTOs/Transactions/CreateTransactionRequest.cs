using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Transactions;

public class CreateTransactionRequest
{
    public Guid? SourceAccountId { get; set; }

    public Guid? DestinationAccountId { get; set; }

    public TransactionType TransactionType { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public string? Description { get; set; }
}