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
            SELECT *
            FROM {GetByIdFunction}(@Id);
            """;

        return await connection.QuerySingleOrDefaultAsync<AuditLog>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync()
    {
        using var connection = CreateConnection();

        const string sql = $"""
            SELECT *
            FROM {GetAllFunction}();
            """;

        return await connection.QueryAsync<AuditLog>(sql);
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