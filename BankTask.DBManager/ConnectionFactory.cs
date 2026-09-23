using System.Data.Common;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace BankTask.DBManager;

public class ConnectionFactory : IConnectionFactory
{
    private readonly string _sqlServerConnectionString;
    private readonly string _postgreSqlConnectionString;

    public ConnectionFactory(
        string sqlServerConnectionString,
        string postgreSqlConnectionString)
    {
        _sqlServerConnectionString = sqlServerConnectionString;
        _postgreSqlConnectionString = postgreSqlConnectionString;
    }

    public DbConnection CreateConnection(DatabaseType databaseType)
    {
        return databaseType switch
        {
            DatabaseType.SqlServer =>
                new SqlConnection(_sqlServerConnectionString),

            DatabaseType.PostgreSQL =>
                new NpgsqlConnection(_postgreSqlConnectionString),

            _ => throw new ArgumentOutOfRangeException(
                nameof(databaseType),
                databaseType,
                "Unsupported database type.")
        };
    }
}