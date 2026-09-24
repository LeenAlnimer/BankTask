using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
}