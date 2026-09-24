using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}