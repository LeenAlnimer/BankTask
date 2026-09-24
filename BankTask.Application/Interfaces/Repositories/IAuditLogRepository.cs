using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id);
}