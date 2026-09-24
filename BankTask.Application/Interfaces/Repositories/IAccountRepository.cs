using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
}