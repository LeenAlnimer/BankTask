namespace BankTask.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public Guid? SourceAccountId { get; set; }

    public Guid? DestinationAccountId { get; set; }

    public short TransactionType { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public string ReferenceNumber { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}