using microCommerce.Domain;
using System.Collections.Generic;
using System.Data;

namespace microCommerce.Dapper.Providers
{
    public interface IProvider
    {
        IDbConnection CreateConnection(string connectionString);
        string InsertQuery(string tableName, object entity, IEnumerable<string> columns);
        string InsertBulkQuery(string tableName, IEnumerable<object> entities, IEnumerable<string> columns);
        string UpdateQuery(string tableName, object entity, IEnumerable<string> columns);
        string UpdateBulkQuery(string tableName, IEnumerable<object> entities, IEnumerable<string> columns);
        string DeleteQuery(string tableName);
        string DeleteBulkQuery(string tableName);
        string SelectFirstQuery<T>(string tableName, IEnumerable<string> columns) where T : BaseEntity;
        string ExistingQuery(string tableName);
        string CountQuery(string tableName);
        IDbDataParameter CreateParameter();
    }
}