using BankTask.Application.DTOs.AuditLogs;
using BankTask.Domain.Entities;

namespace BankTask.Application.Mappers;

public static class AuditLogMapper
{
    public static AuditLogResponse ToResponse(AuditLog auditLog)
    {
        return new AuditLogResponse
        {
            Id = auditLog.Id,
            EventId = auditLog.EventId,
            UserId = auditLog.UserId,
            Action = auditLog.Action,
            EntityType = auditLog.EntityType,
            EntityId = auditLog.EntityId,
            OldValues = auditLog.OldValues,
            NewValues = auditLog.NewValues,
            IpAddress = auditLog.IpAddress,
            CreatedAt = auditLog.CreatedAt
        };
    }
}