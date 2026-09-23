using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly SqlServerDbManager _dbManager;

    public AccountRepository(SqlServerDbManager dbManager)
    {
        _dbManager = dbManager;
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

        using var connection = _dbManager.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Account>(
            sql,
            new { Id = id });
    }
}