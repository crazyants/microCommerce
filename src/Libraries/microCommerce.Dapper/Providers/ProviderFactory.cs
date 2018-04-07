using microCommerce.Common;
using microCommerce.Dapper.Providers.MySql;
using microCommerce.Dapper.Providers.PostgreSql;
using microCommerce.Dapper.Providers.SqlServer;

namespace microCommerce.Dapper.Providers
{
    public class ProviderFactory
    {
        public static IDataProvider GetProvider(string providerName)
        {
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return new SqlServerDataProvider();
                case "MySql.Data.SqlClient":
                    return new MySqlDataProvider();
                case "NpgSql.Data.SqlClient":
                    return new PostgreSqlDataProvider();
                default:
                    throw new CustomException("Database provider does not supported!");
            }
        }
    }
}