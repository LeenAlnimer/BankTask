using BankTask.Application.DTOs.Users;

namespace BankTask.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(Guid id);

    Task<IEnumerable<UserResponse>> GetAllAsync();

    Task<UserResponse> CreateAsync(CreateUserRequest request);

    Task<bool> UpdateAsync(Guid id, UpdateUserRequest request);

    Task<bool> DeleteAsync(Guid id);
}