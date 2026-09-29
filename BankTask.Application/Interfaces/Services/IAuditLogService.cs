using BankTask.Application.DTOs.AuditLogs;

namespace BankTask.Application.Interfaces.Services;

public interface IAuditLogService
{
    Task<AuditLogResponse?> GetByIdAsync(Guid id);
    // Get audit logs with pagination (offset, limit)
    Task<IEnumerable<AuditLogResponse>> GetAllAsync(int offset, int limit);
}