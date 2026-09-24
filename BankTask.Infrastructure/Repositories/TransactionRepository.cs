using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public TransactionRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        const string sql = """
            SELECT
                id AS Id,
                event_id AS EventId,
                source_account_id AS SourceAccountId,
                destination_account_id AS DestinationAccountId,
                transaction_type AS TransactionType,
                amount AS Amount,
                currency AS Currency,
                reference_number AS ReferenceNumber,
                description AS Description,
                created_at AS CreatedAt
            FROM transactions
            WHERE id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.PostgreSQL);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<Transaction>(
            sql,
            new { Id = id });
    }
}