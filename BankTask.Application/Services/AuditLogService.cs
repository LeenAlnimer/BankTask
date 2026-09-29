using BankTask.Application.DTOs.AuditLogs;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Mappers;

namespace BankTask.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<AuditLogResponse?> GetByIdAsync(Guid id)
    {
        var auditLog =
            await _auditLogRepository.GetByIdAsync(id);

        if (auditLog is null)
        {
            throw new BankTask.Application.Exceptions.NotFoundException(
                "AUDIT_LOG_NOT_FOUND");
        }

        return AuditLogMapper.ToResponse(auditLog);
    }

    public async Task<IEnumerable<AuditLogResponse>> GetAllAsync()
    {
        var auditLogs =
            await _auditLogRepository.GetAllAsync();

        return auditLogs.Select(AuditLogMapper.ToResponse);
    }
}