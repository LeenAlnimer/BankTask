namespace BankTask.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string Currency { get; set; } = string.Empty;

    public byte Status { get; set; }

    public byte AccountType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}