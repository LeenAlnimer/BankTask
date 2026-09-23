using BankTask.Application.Interfaces.Repositories;
using BankTask.DBManager;
using BankTask.Domain.Entities;
using Dapper;

namespace BankTask.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public UserRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = """
            SELECT
                Id,
                FullName,
                Email,
                CreatedAt,
                UpdatedAt
            FROM Users
            WHERE Id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
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
            WHERE Email = @Email;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new { Email = email });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = """
            SELECT
                Id,
                FullName,
                Email,
                CreatedAt,
                UpdatedAt
            FROM Users;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User> CreateAsync(User user)
    {
        const string sql = """
            INSERT INTO Users
            (
                Id,
                FullName,
                Email,
                PasswordHash,
                CreatedAt,
                UpdatedAt
            )
            VALUES
            (
                @Id,
                @FullName,
                @Email,
                @PasswordHash,
                @CreatedAt,
                @UpdatedAt
            );
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, user);

        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        const string sql = """
            UPDATE Users
            SET
                FullName = @FullName,
                Email = @Email,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            user);

        return rowsAffected == 1;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string sql = """
            DELETE FROM Users
            WHERE Id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new { Id = id });

        return rowsAffected == 1;
    }
}