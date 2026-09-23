using System.Data.Common;

namespace BankTask.DBManager;

public interface IConnectionFactory
{
    DbConnection CreateConnection(DatabaseType databaseType);
}