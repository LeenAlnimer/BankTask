using BankTask.Application.DTOs.Users;
using BankTask.Domain.Entities;

namespace BankTask.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}