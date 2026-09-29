using BankTask.Domain.Entities;

namespace BankTask.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id);

    // Get audit logs with pagination. offset >= 0, limit > 0
    Task<IEnumerable<AuditLog>> GetAllAsync(int offset, int limit);

    Task<AuditLog> CreateAsync(AuditLog auditLog);
}