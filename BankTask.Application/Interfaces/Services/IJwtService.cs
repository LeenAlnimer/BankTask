using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}