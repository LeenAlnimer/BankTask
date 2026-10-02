using System.Security.Claims;
using System.Text.Json;
using BankTask.Application.DTOs.Accounts;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Mappers;
using BankTask.Domain.Entities;
using BankTask.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BankTask.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(
        IAccountRepository accountRepository,
        IAuditLogRepository auditLogRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _accountRepository = accountRepository;
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AccountResponse?> GetByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);

        if (account is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "ACCOUNT_NOT_FOUND");
        }

        return AccountMapper.ToResponse(account);
    }

    public async Task<IEnumerable<AccountResponse>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();

        return accounts.Select(AccountMapper.ToResponse);
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

        await CreateAuditLogAsync(
            action: "ACCOUNT_CREATED",
            entityId: createdAccount.Id,
            oldValues: null,
            newValues: new
            {
                createdAccount.Id,
                createdAccount.UserId,
                createdAccount.AccountNumber,
                createdAccount.Balance,
                createdAccount.Currency,
                createdAccount.Status,
                createdAccount.AccountType,
                createdAccount.CreatedAt
            });

        return AccountMapper.ToResponse(createdAccount);
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateAccountRequest request)
    {
        var existingAccount =
            await _accountRepository.GetByIdAsync(id);

        if (existingAccount is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "ACCOUNT_NOT_FOUND");
        }

        var oldStatus = existingAccount.Status;

        existingAccount.Status = request.Status;
        existingAccount.UpdatedAt = DateTime.UtcNow;

        var updated =
            await _accountRepository.UpdateAsync(existingAccount);

        if (updated)
        {
            await CreateAuditLogAsync(
                action: "ACCOUNT_UPDATED",
                entityId: existingAccount.Id,
                oldValues: new
                {
                    existingAccount.Id,
                    Status = oldStatus
                },
                newValues: new
                {
                    existingAccount.Id,
                    existingAccount.Status,
                    existingAccount.UpdatedAt
                });
        }

        return updated;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingAccount =
            await _accountRepository.GetByIdAsync(id);

        if (existingAccount is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "ACCOUNT_NOT_FOUND");
        }

        var oldStatus = existingAccount.Status;

        existingAccount.Status = AccountStatus.Closed;
        existingAccount.UpdatedAt = DateTime.UtcNow;

        var deleted =
            await _accountRepository.UpdateAsync(existingAccount);

        if (deleted)
        {
            await CreateAuditLogAsync(
                action: "ACCOUNT_DELETED",
                entityId: existingAccount.Id,
                oldValues: new
                {
                    existingAccount.Id,
                    existingAccount.UserId,
                    existingAccount.AccountNumber,
                    existingAccount.Balance,
                    existingAccount.Currency,
                    Status = oldStatus,
                    existingAccount.AccountType
                },
                newValues: new
                {
                    existingAccount.Id,
                    existingAccount.Status,
                    existingAccount.UpdatedAt
                });
        }

        return deleted;
    }

    private async Task CreateAuditLogAsync(
        string action,
        Guid entityId,
        object? oldValues,
        object? newValues)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var userIdClaim =
            httpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            UserId =
                Guid.TryParse(userIdClaim?.Value, out var userId)
                    ? userId
                    : null,
            Action = action,
            EntityType = "Account",
            EntityId = entityId,
            OldValues =
                oldValues is null
                    ? null
                    : JsonSerializer.Serialize(oldValues),
            NewValues =
                newValues is null
                    ? null
                    : JsonSerializer.Serialize(newValues),
            IpAddress =
                httpContext?.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _auditLogRepository.CreateAsync(auditLog);
    }
}