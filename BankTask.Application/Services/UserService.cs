using System.Security.Claims;
using System.Text.Json;
using BankTask.Application.DTOs.Users;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Security;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Mappers;
using BankTask.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BankTask.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IAuditLogRepository auditLogRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "USER_NOT_FOUND");
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

        await CreateAuditLogAsync(
            action: "USER_CREATED",
            entityId: createdUser.Id,
            oldValues: null,
            newValues: new
            {
                createdUser.Id,
                createdUser.FullName,
                createdUser.Email,
                createdUser.CreatedAt
            });

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

        var oldValues = new
        {
            existingUser.Id,
            existingUser.FullName,
            existingUser.Email,
            existingUser.UpdatedAt
        };

        existingUser.FullName = request.FullName;
        existingUser.Email = request.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        var updated =
            await _userRepository.UpdateAsync(existingUser);

        if (updated)
        {
            await CreateAuditLogAsync(
                action: "USER_UPDATED",
                entityId: existingUser.Id,
                oldValues: oldValues,
                newValues: new
                {
                    existingUser.Id,
                    existingUser.FullName,
                    existingUser.Email,
                    existingUser.UpdatedAt
                });
        }

        return updated;
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

        var deleted =
            await _userRepository.DeleteAsync(id);

        if (deleted)
        {
            await CreateAuditLogAsync(
                action: "USER_DELETED",
                entityId: existingUser.Id,
                oldValues: new
                {
                    existingUser.Id,
                    existingUser.FullName,
                    existingUser.Email,
                    existingUser.CreatedAt,
                    existingUser.UpdatedAt
                },
                newValues: null);
        }

        return deleted;
    }

    private async Task CreateAuditLogAsync(
        string action,
        Guid entityId,
        object? oldValues,
        object? newValues)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var userIdClaim =
            httpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            UserId =
                Guid.TryParse(userIdClaim?.Value, out var userId)
                    ? userId
                    : null,
            Action = action,
            EntityType = "User",
            EntityId = entityId,
            OldValues =
                oldValues is null
                    ? null
                    : JsonSerializer.Serialize(oldValues),
            NewValues =
                newValues is null
                    ? null
                    : JsonSerializer.Serialize(newValues),
            IpAddress =
                httpContext?.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _auditLogRepository.CreateAsync(auditLog);
    }
}