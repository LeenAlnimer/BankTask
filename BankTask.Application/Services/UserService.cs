using BankTask.Application.DTOs.Users;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Security;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Mappers;
using BankTask.Domain.Entities;

namespace BankTask.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return UserMapper.ToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(UserMapper.ToResponse);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var existingUser =
            await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new BankTask.Application.Exceptions.ConflictException(
                "USER_EMAIL_ALREADY_EXISTS");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var createdUser =
            await _userRepository.CreateAsync(user);

        return UserMapper.ToResponse(createdUser);
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateUserRequest request)
    {
        var existingUser =
            await _userRepository.GetByIdAsync(id);

        if (existingUser is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "USER_NOT_FOUND");
        }

        existingUser.FullName = request.FullName;
        existingUser.Email = request.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        return await _userRepository.UpdateAsync(existingUser);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingUser =
            await _userRepository.GetByIdAsync(id);

        if (existingUser is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "USER_NOT_FOUND");
        }

        return await _userRepository.DeleteAsync(id);
    }
}