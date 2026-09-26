using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);

    Task<IEnumerable<Transaction>> GetAllAsync();

    Task<Transaction> CreateAsync(Transaction transaction);
}