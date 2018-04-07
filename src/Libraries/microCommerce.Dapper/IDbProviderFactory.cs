using microCommerce.Dapper.Providers;

namespace microCommerce.Dapper
{
    public interface IDbProviderFactory
    {
        IDataProvider Create(string providerName);
    }
}