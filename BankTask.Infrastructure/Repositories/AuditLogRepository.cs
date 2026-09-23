using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly PostgreSqlDbManager _dbManager;

    public AuditLogRepository(PostgreSqlDbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task<AuditLog?> GetByIdAsync(Guid id)
    {
        const string sql = """
            SELECT
                id AS Id,
                event_id AS EventId,
                user_id AS UserId,
                action AS Action,
                entity_type AS EntityType,
                entity_id AS EntityId,
                old_values AS OldValues,
                new_values AS NewValues,
                ip_address AS IpAddress,
                created_at AS CreatedAt
            FROM audit_logs
            WHERE id = @Id;
            """;

        using var connection = _dbManager.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<AuditLog>(
            sql,
            new { Id = id });
    }
}