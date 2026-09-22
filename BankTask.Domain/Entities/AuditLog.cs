namespace BankTask.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    public Guid? EventId { get; set; }

    public Guid? UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityType { get; set; } = string.Empty;

    public Guid? EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}