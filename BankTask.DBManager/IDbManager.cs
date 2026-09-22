using System.Data;

namespace BankTask.DBManager;

public interface IDbManager
{
    IDbConnection CreateConnection();
}