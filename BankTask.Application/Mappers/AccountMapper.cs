using BankTask.Application.DTOs.Accounts;
using BankTask.Domain.Entities;

namespace BankTask.Application.Mappers;

public static class AccountMapper
{
    public static AccountResponse ToResponse(Account account)
    {
        return new AccountResponse
        {
            Id = account.Id,
            UserId = account.UserId,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Currency = account.Currency,
            Status = account.Status,
            AccountType = account.AccountType,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }
}