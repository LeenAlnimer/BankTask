using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlServerDbManager _dbManager;

    public UserRepository(SqlServerDbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = """
            SELECT
                Id,
                FullName,
                Email,
                PasswordHash,
                CreatedAt,
                UpdatedAt
            FROM Users
            WHERE Id = @Id;
            """;

        using var connection = _dbManager.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new { Id = id });
    }
}