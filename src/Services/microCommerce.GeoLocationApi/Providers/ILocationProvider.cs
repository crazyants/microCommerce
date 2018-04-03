using System.Threading.Tasks;

namespace microCommerce.GeoLocationApi.Services
{
    public interface ILocationProvider
    {
        /// <summary>
        /// Find country code by ip address from provider data source
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<string> FindCountryCode(string ipAddress);
    }
}