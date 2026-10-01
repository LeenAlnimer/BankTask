using System.Text.Json;
using BankTask.Application.DTOs.Authentication;
using BankTask.Application.DTOs.Users;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Security;
using BankTask.Application.Interfaces.Services;
using BankTask.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BankTask.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IAuditLogRepository auditLogRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserResponse> SignupAsync(SignupRequest request)
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
            action: "USER_SIGNED_UP",
            userId: createdUser.Id,
            entityId: createdUser.Id,
            newValues: new
            {
                createdUser.Id,
                createdUser.FullName,
                createdUser.Email,
                createdUser.CreatedAt
            });

        return new UserResponse
        {
            Id = createdUser.Id,
            FullName = createdUser.FullName,
            Email = createdUser.Email,
            CreatedAt = createdUser.CreatedAt,
            UpdatedAt = createdUser.UpdatedAt
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user =
            await _userRepository.GetByEmailAsync(request.Email);

        if (user is null ||
            !_passwordHasher.Verify(
                request.Password,
                user.PasswordHash))
        {
            throw new BankTask.Application.Exceptions.UnauthorizedException(
                "AUTH_INVALID_CREDENTIALS");
        }

        var accessToken = _jwtService.GenerateToken(user);

        await CreateAuditLogAsync(
            action: "USER_LOGGED_IN",
            userId: user.Id,
            entityId: user.Id,
            newValues: null);

        return new LoginResponse
        {
            AccessToken = accessToken
        };
    }

    private async Task CreateAuditLogAsync(
        string action,
        Guid userId,
        Guid entityId,
        object? newValues)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            UserId = userId,
            Action = action,
            EntityType = "User",
            EntityId = entityId,
            OldValues = null,
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