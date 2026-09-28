using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id);

    Task<IEnumerable<AuditLog>> GetAllAsync();

    Task<AuditLog> CreateAsync(AuditLog auditLog);
}