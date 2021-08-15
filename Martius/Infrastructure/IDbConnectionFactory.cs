using System.Data;

namespace Martius.Infrastructure
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();

        IDbCommand CreateCommand();
    }
}