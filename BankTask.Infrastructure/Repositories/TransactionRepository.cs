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
            FROM get_transaction_by_id(@Id);
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.PostgreSQL);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<Transaction>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
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
            FROM get_all_transactions();
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.PostgreSQL);

        await connection.OpenAsync();

        return await connection.QueryAsync<Transaction>(sql);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        const string sql = """
            CALL create_transaction(
                @Id,
                @EventId,
                @SourceAccountId,
                @DestinationAccountId,
                @TransactionType,
                @Amount,
                @Currency,
                @ReferenceNumber,
                @Description,
                @CreatedAt
            );
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.PostgreSQL);

        await connection.OpenAsync();

        await connection.ExecuteAsync(
            sql,
            new
            {
                transaction.Id,
                transaction.EventId,
                transaction.SourceAccountId,
                transaction.DestinationAccountId,
                TransactionType = (short)transaction.TransactionType,
                transaction.Amount,
                transaction.Currency,
                transaction.ReferenceNumber,
                transaction.Description,
                transaction.CreatedAt
            });

        var createdTransaction = await GetByIdAsync(transaction.Id);

        if (createdTransaction is null)
        {
            throw new InvalidOperationException(
                "Transaction was created but could not be retrieved.");
        }

        return createdTransaction;
    }
}