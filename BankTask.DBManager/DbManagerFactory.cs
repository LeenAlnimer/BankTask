namespace BankTask.DBManager;

public static class DbManagerFactory
{
    public static IDbManager Create(
        string databaseType,
        string connectionString)
    {
        return databaseType.ToLower() switch
        {
            "sqlserver" =>
                new SqlServerDbManager(connectionString),

            "postgresql" =>
                new PostgreSqlDbManager(connectionString),

            _ => throw new ArgumentException(
                $"Unsupported database type: {databaseType}")
        };
    }
}