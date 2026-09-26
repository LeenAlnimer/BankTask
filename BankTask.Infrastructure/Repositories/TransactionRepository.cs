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
            SELECT *
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
            SELECT *
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