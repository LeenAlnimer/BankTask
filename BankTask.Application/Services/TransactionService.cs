using System.Security.Claims;
using System.Text.Json;
using BankTask.Application.DTOs.Transactions;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Mappers;
using BankTask.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BankTask.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAuditLogRepository auditLogRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _transactionRepository = transactionRepository;
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TransactionResponse?> GetByIdAsync(Guid id)
    {
        var transaction =
            await _transactionRepository.GetByIdAsync(id);

        if (transaction is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "TRANSACTION_NOT_FOUND");
        }

        return TransactionMapper.ToResponse(transaction);
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllAsync()
    {
        var transactions =
            await _transactionRepository.GetAllAsync();

        return transactions.Select(TransactionMapper.ToResponse);
    }

    public async Task<TransactionResponse> CreateAsync(
        CreateTransactionRequest request)
    {
        ValidateTransaction(request);

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            EventId = Guid.NewGuid(),

            SourceAccountId = request.SourceAccountId,
            DestinationAccountId = request.DestinationAccountId,

            TransactionType = (short)request.TransactionType,

            Amount = request.Amount,
            Currency = request.Currency.Trim().ToUpperInvariant(),

            ReferenceNumber = GenerateReferenceNumber(),

            Description = request.Description?.Trim(),

            CreatedAt = DateTimeOffset.UtcNow
        };

        var createdTransaction =
            await _transactionRepository.CreateAsync(transaction);

        await CreateAuditLogAsync(
            action: "TRANSACTION_CREATED",
            eventId: transaction.EventId,
            entityId: createdTransaction.Id,
            newValues: new
            {
                createdTransaction.Id,
                createdTransaction.EventId,
                createdTransaction.SourceAccountId,
                createdTransaction.DestinationAccountId,
                createdTransaction.TransactionType,
                createdTransaction.Amount,
                createdTransaction.Currency,
                createdTransaction.ReferenceNumber,
                createdTransaction.Description,
                createdTransaction.CreatedAt
            });

        return TransactionMapper.ToResponse(createdTransaction);
    }

    private static void ValidateTransaction(
        CreateTransactionRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new BankTask.Application.Exceptions.ValidationException(
                "TRANSACTION_AMOUNT_INVALID");
        }

        switch (request.TransactionType)
        {
            case Domain.Enums.TransactionType.Transfer:

                if (request.SourceAccountId is null ||
                    request.DestinationAccountId is null)
                {
                    throw new BankTask.Application.Exceptions.ValidationException(
                        "TRANSFER_ACCOUNTS_REQUIRED");
                }

                if (request.SourceAccountId ==
                    request.DestinationAccountId)
                {
                    throw new BankTask.Application.Exceptions.ValidationException(
                        "TRANSFER_ACCOUNTS_MUST_DIFFER");
                }

                break;

            case Domain.Enums.TransactionType.Deposit:

                if (request.DestinationAccountId is null)
                {
                    throw new BankTask.Application.Exceptions.ValidationException(
                        "DEPOSIT_DESTINATION_REQUIRED");
                }

                break;

            case Domain.Enums.TransactionType.Withdrawal:

                if (request.SourceAccountId is null)
                {
                    throw new BankTask.Application.Exceptions.ValidationException(
                        "WITHDRAWAL_SOURCE_REQUIRED");
                }

                break;

            default:

                throw new BankTask.Application.Exceptions.ValidationException(
                    "TRANSACTION_TYPE_INVALID");
        }

        if (string.IsNullOrWhiteSpace(request.Currency) ||
            request.Currency.Trim().Length != 3)
        {
            throw new BankTask.Application.Exceptions.ValidationException(
                "CURRENCY_INVALID");
        }
    }

    private static string GenerateReferenceNumber()
    {
        return $"TX-{Guid.NewGuid():N}";
    }

    private async Task CreateAuditLogAsync(
        string action,
        Guid eventId,
        Guid entityId,
        object newValues)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var userIdClaim =
            httpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            UserId =
                Guid.TryParse(userIdClaim?.Value, out var userId)
                    ? userId
                    : null,
            Action = action,
            EntityType = "Transaction",
            EntityId = entityId,
            OldValues = null,
            NewValues = JsonSerializer.Serialize(newValues),
            IpAddress =
                httpContext?.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _auditLogRepository.CreateAsync(auditLog);
    }
}