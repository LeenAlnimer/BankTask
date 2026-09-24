using BankTask.Application.DTOs.Accounts;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Services;
using BankTask.Domain.Entities;
using BankTask.Domain.Enums;

namespace BankTask.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponse?> GetByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);

        if (account is null)
        {
            return null;
        }

        return MapToResponse(account);
    }

    public async Task<IEnumerable<AccountResponse>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();

        return accounts.Select(MapToResponse);
    }

    public async Task<AccountResponse> CreateAsync(
        CreateAccountRequest request)
    {
        var accountNumber =
            await _accountRepository.GetNextAccountNumberAsync();

        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            AccountNumber = accountNumber,
            Balance = 0,
            Currency = request.Currency,
            Status = AccountStatus.Active,
            AccountType = request.AccountType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var createdAccount =
            await _accountRepository.CreateAsync(account);

        return MapToResponse(createdAccount);
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateAccountRequest request)
    {
        var existingAccount =
            await _accountRepository.GetByIdAsync(id);

        if (existingAccount is null)
        {
            return false;
        }

        existingAccount.Status = request.Status;
        existingAccount.UpdatedAt = DateTime.UtcNow;

        return await _accountRepository.UpdateAsync(
            existingAccount);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingAccount =
            await _accountRepository.GetByIdAsync(id);

        if (existingAccount is null)
        {
            return false;
        }

        existingAccount.Status = AccountStatus.Closed;
        existingAccount.UpdatedAt = DateTime.UtcNow;

        return await _accountRepository.UpdateAsync(
            existingAccount);
    }

    private static AccountResponse MapToResponse(
        Account account)
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