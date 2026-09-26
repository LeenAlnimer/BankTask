using System.Data;
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
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<User>(
            "GetUserById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<User>(
            "GetUserByEmail",
            new { Email = email },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        return await connection.QueryAsync<User>(
            "GetAllUsers",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        await connection.ExecuteAsync(
            "CreateUser",
            new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.PasswordHash,
                user.CreatedAt,
                user.UpdatedAt
            },
            commandType: CommandType.StoredProcedure);

        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var rowsAffected = await connection.QuerySingleAsync<int>(
            "UpdateUser",
            new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.UpdatedAt
            },
            commandType: CommandType.StoredProcedure);

        return rowsAffected == 1;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection =
            _connectionFactory.CreateConnection(DatabaseType.SqlServer);

        await connection.OpenAsync();

        var rowsAffected = await connection.QuerySingleAsync<int>(
            "DeleteUser",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return rowsAffected == 1;
    }
}