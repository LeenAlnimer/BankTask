using System.Data;
using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
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
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<Account>(
            "GetAccountById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QueryAsync<Account>(
            "GetAllAccounts",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<string> GetNextAccountNumberAsync()
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var accountNumber =
            await connection.QuerySingleAsync<long>(
                "GetNextAccountNumber",
                commandType: CommandType.StoredProcedure);

        return accountNumber.ToString();
    }

    public async Task<Account> CreateAsync(Account account)
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        await connection.ExecuteAsync(
            "CreateAccount",
            new
            {
                account.Id,
                account.UserId,
                account.AccountNumber,
                account.Balance,
                account.Currency,
                account.Status,
                account.AccountType,
                account.CreatedAt,
                account.UpdatedAt
            },
            commandType: CommandType.StoredProcedure);

        return account;
    }

    public async Task<bool> UpdateAsync(Account account)
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var rowsAffected =
            await connection.QuerySingleAsync<int>(
                "UpdateAccount",
                new
                {
                    account.Id,
                    account.Status,
                    account.UpdatedAt
                },
                commandType: CommandType.StoredProcedure);

        return rowsAffected == 1;
    }
}