using microCommerce.Dapper.Providers;

namespace microCommerce.Dapper
{
    public interface IDbProviderFactory
    {
        IProvider Create(string providerName);
    }
}