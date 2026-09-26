using BankTask.Application.DTOs.Transactions;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Services;
using BankTask.Domain.Entities;

namespace BankTask.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionResponse?> GetByIdAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);

        if (transaction is null)
        {
            return null;
        }

        return MapToResponse(transaction);
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();

        return transactions.Select(MapToResponse);
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

        return MapToResponse(createdTransaction);
    }

    private static void ValidateTransaction(
        CreateTransactionRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "Transaction amount must be greater than zero.");
        }

        switch (request.TransactionType)
        {
            case Domain.Enums.TransactionType.Transfer:

                if (request.SourceAccountId is null ||
                    request.DestinationAccountId is null)
                {
                    throw new ArgumentException(
                        "Transfer requires both source and destination accounts.");
                }

                if (request.SourceAccountId ==
                    request.DestinationAccountId)
                {
                    throw new ArgumentException(
                        "Source and destination accounts must be different.");
                }

                break;

            case Domain.Enums.TransactionType.Deposit:

                if (request.DestinationAccountId is null)
                {
                    throw new ArgumentException(
                        "Deposit requires a destination account.");
                }

                break;

            case Domain.Enums.TransactionType.Withdrawal:

                if (request.SourceAccountId is null)
                {
                    throw new ArgumentException(
                        "Withdrawal requires a source account.");
                }

                break;

            default:

                throw new ArgumentException(
                    "Invalid transaction type.");
        }

        if (string.IsNullOrWhiteSpace(request.Currency) ||
            request.Currency.Trim().Length != 3)
        {
            throw new ArgumentException(
                "Currency must contain exactly 3 characters.");
        }
    }

    private static string GenerateReferenceNumber()
    {
        return $"TX-{Guid.NewGuid():N}";
    }

    private static TransactionResponse MapToResponse(
        Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            EventId = transaction.EventId,
            SourceAccountId = transaction.SourceAccountId,
            DestinationAccountId = transaction.DestinationAccountId,
            TransactionType =
                (Domain.Enums.TransactionType)transaction.TransactionType,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            ReferenceNumber = transaction.ReferenceNumber,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }
}