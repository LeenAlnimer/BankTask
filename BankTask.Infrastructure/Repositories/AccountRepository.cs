using System.Data;
using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IConnectionFactory _connectionFactory;

    private const string GetByIdSp = "GetAccountById";
    private const string GetAllSp = "GetAllAccounts";
    private const string GetNextAccountNumberSp = "GetNextAccountNumber";
    private const string CreateSp = "CreateAccount";
    private const string UpdateSp = "UpdateAccount";

    public AccountRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private IDbConnection CreateConnection() =>
        _connectionFactory.CreateConnection(DatabaseType.SqlServer);

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        using var connection = CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Account>(
            GetByIdSp,
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        using var connection = CreateConnection();

        return await connection.QueryAsync<Account>(
            GetAllSp,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<string> GetNextAccountNumberAsync()
    {
        using var connection = CreateConnection();

        var accountNumber =
            await connection.QuerySingleAsync<long>(
                GetNextAccountNumberSp,
                commandType: CommandType.StoredProcedure);

        return accountNumber.ToString();
    }

    public async Task<Account> CreateAsync(Account account)
    {
        using var connection = CreateConnection();

        return await connection.QuerySingleAsync<Account>(
            CreateSp,
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
    }

    public async Task<bool> UpdateAsync(Account account)
    {
        using var connection = CreateConnection();

        var rowsAffected =
            await connection.QuerySingleAsync<int>(
                UpdateSp,
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