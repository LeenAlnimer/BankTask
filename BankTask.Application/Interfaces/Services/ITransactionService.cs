using BankTask.Application.DTOs.Transactions;

namespace BankTask.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionResponse?> GetByIdAsync(Guid id);

    Task<IEnumerable<TransactionResponse>> GetAllAsync();

    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request);
}