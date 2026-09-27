using BankTask.Application.DTOs.Accounts;

namespace BankTask.Application.Interfaces.Services;

public interface IAccountService
{
    Task<AccountResponse?> GetByIdAsync(Guid id);

    Task<IEnumerable<AccountResponse>> GetAllAsync();

    Task<AccountResponse> CreateAsync(CreateAccountRequest request);

    Task<bool> UpdateAsync(Guid id, UpdateAccountRequest request);

    Task<bool> DeleteAsync(Guid id);
}