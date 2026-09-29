using System.Data;
using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IConnectionFactory _connectionFactory;

    private const string GetByIdFunction = "get_transaction_by_id";
    private const string GetAllFunction = "get_all_transactions";
    private const string CreateProcedure = "create_transaction";

    public TransactionRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private IDbConnection CreateConnection() =>
        _connectionFactory.CreateConnection(DatabaseType.PostgreSQL);

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        var sql = $"""
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
            FROM {GetByIdFunction}(@Id);
            """;

        using var connection = CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Transaction>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        var sql = $"""
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
            FROM {GetAllFunction}();
            """;

        using var connection = CreateConnection();

        return await connection.QueryAsync<Transaction>(sql);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        var sql = $"""
            CALL {CreateProcedure}(
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

        using var connection = CreateConnection();

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
            throw new BankTask.Application.Exceptions.AppException(
                "TRANSACTION_CREATION_RETRIEVAL_FAILED");
        }

        return createdTransaction;
    }
}