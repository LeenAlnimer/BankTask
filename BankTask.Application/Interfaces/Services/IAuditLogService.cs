using BankTask.Application.DTOs.AuditLogs;

namespace BankTask.Application.Interfaces.Services;

public interface IAuditLogService
{
    Task<AuditLogResponse?> GetByIdAsync(Guid id);

    Task<IEnumerable<AuditLogResponse>> GetAllAsync();
}