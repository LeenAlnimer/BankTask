using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using BankTask.Domain.Enums;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public AccountRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        const string sql = """
            SELECT
                Id,
                UserId,
                AccountNumber,
                Balance,
                Currency,
                Status,
                AccountType,
                CreatedAt,
                UpdatedAt
            FROM Accounts
            WHERE Id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<Account>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        const string sql = """
            SELECT
                Id,
                UserId,
                AccountNumber,
                Balance,
                Currency,
                Status,
                AccountType,
                CreatedAt,
                UpdatedAt
            FROM Accounts
            WHERE Status <> @ClosedStatus;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QueryAsync<Account>(
            sql,
            new
            {
                ClosedStatus = (byte)AccountStatus.Closed
            });
    }

    public async Task<string> GetNextAccountNumberAsync()
    {
        const string sql = """
            SELECT NEXT VALUE FOR AccountNumberSequence;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var accountNumber =
            await connection.QuerySingleAsync<long>(sql);

        return accountNumber.ToString();
    }

    public async Task<Account> CreateAsync(Account account)
    {
        const string sql = """
            INSERT INTO Accounts
            (
                Id,
                UserId,
                AccountNumber,
                Balance,
                Currency,
                Status,
                AccountType,
                CreatedAt,
                UpdatedAt
            )
            VALUES
            (
                @Id,
                @UserId,
                @AccountNumber,
                @Balance,
                @Currency,
                @Status,
                @AccountType,
                @CreatedAt,
                @UpdatedAt
            );
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, account);

        return account;
    }

    public async Task<bool> UpdateAsync(Account account)
    {
        const string sql = """
            UPDATE Accounts
            SET
                Status = @Status,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            account);

        return rowsAffected == 1;
    }
}