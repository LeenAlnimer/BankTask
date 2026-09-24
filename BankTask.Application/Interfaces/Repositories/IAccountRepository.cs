using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);

    Task<IEnumerable<Account>> GetAllAsync();

    Task<string> GetNextAccountNumberAsync();

    Task<Account> CreateAsync(Account account);

    Task<bool> UpdateAsync(Account account);
}