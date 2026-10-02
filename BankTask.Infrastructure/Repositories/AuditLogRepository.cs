using System.Data;
using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly IConnectionFactory _connectionFactory;

    private const string GetByIdFunction = "get_audit_log_by_id";
    private const string GetAllFunction = "get_all_audit_logs";
    private const string CreateProcedure = "create_audit_log";
    
    public AuditLogRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private IDbConnection CreateConnection() =>
        _connectionFactory.CreateConnection(DatabaseType.PostgreSQL);

    public async Task<AuditLog?> GetByIdAsync(Guid id)
    {
        using var connection = CreateConnection();

        const string sql = $"""
            SELECT
                id AS Id,
                event_id AS EventId,
                user_id AS UserId,
                action AS Action,
                entity_type AS EntityType,
                entity_id AS EntityId,
                old_values AS OldValues,
                new_values AS NewValues,
                ip_address::text AS IpAddress,
                created_at AS CreatedAt
            FROM {GetByIdFunction}(@Id);
            """;

        return await connection.QuerySingleOrDefaultAsync<AuditLog>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync(int offset, int limit)
    {
        using var connection = CreateConnection();

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
                ip_address::text AS IpAddress,
                created_at AS CreatedAt
            FROM audit_logs
            ORDER BY created_at DESC
            LIMIT @Limit OFFSET @Offset;
            """;

        return await connection.QueryAsync<AuditLog>(
            sql,
            new { Limit = limit, Offset = offset });
    }

    public async Task<AuditLog> CreateAsync(AuditLog auditLog)
    {
        using var connection = CreateConnection();

        const string sql = $"""
            CALL {CreateProcedure}(
                @Id,
                @EventId,
                @UserId,
                @Action,
                @EntityType,
                @EntityId,
                @OldValues,
                @NewValues,
                @IpAddress,
                @CreatedAt
            );
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                auditLog.Id,
                auditLog.EventId,
                auditLog.UserId,
                auditLog.Action,
                auditLog.EntityType,
                auditLog.EntityId,
                auditLog.OldValues,
                auditLog.NewValues,
                auditLog.IpAddress,
                auditLog.CreatedAt
            });

        return await GetByIdAsync(auditLog.Id)
            ?? throw new BankTask.Application.Exceptions.AppException(
                "AUDIT_LOG_CREATION_RETRIEVAL_FAILED");
    }
}