using microCommerce.Common;
using microCommerce.Dapper.Providers.MySql;
using microCommerce.Dapper.Providers.PostgreSql;
using microCommerce.Dapper.Providers.SqlServer;

namespace microCommerce.Dapper.Providers
{
    public class ProviderFactory
    {
        public static IProvider GetProvider(string providerName)
        {
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return new SqlServerProvider();
                case "MySql.Data.SqlClient":
                    return new MySqlProvider();
                case "NpgSql.Data.SqlClient":
                    return new PostgreSqlProvider();
                default:
                    throw new CustomException("Database provider does not supported!");
            }
        }
    }
}