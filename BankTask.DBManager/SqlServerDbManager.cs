using System.Data;
using Microsoft.Data.SqlClient;

namespace BankTask.DBManager;

public class SqlServerDbManager : IDbManager
{
    private readonly string _connectionString;

    public SqlServerDbManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}