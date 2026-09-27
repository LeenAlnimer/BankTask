using BankTask.Application.DTOs.Transactions;
using BankTask.Domain.Entities;
using BankTask.Domain.Enums;

namespace BankTask.Application.Mappers;

public static class TransactionMapper
{
    public static TransactionResponse ToResponse(
        Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            EventId = transaction.EventId,
            SourceAccountId = transaction.SourceAccountId,
            DestinationAccountId = transaction.DestinationAccountId,
            TransactionType =
                (TransactionType)transaction.TransactionType,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            ReferenceNumber = transaction.ReferenceNumber,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }
}