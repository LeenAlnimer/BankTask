using BankTask.Domain.Enums;

namespace BankTask.Application.DTOs.Accounts;

public class CreateAccountRequest
{
    public Guid UserId { get; set; }

    public Currency Currency { get; set; }

    public AccountType AccountType { get; set; }
}