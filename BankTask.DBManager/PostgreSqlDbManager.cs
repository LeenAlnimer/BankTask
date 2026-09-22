using System.Data;
using Npgsql;

namespace BankTask.DBManager;

public class PostgreSqlDbManager : IDbManager
{
    private readonly string _connectionString;

    public PostgreSqlDbManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}