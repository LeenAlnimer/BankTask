using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Accounts;

public class AccountResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public Currency Currency { get; set; }

    public AccountStatus Status { get; set; }

    public AccountType AccountType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}