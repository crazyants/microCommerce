using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using microCommerce.Common;
using microCommerce.Logging;
using microCommerce.Setting;
using System;
using System.Threading.Tasks;

namespace microCommerce.GeoLocationApi.Services
{
    public class MaxMindProvider : ILocationProvider
    {
        #region Fields
        private readonly ILogger _logger;
        private readonly GeoLocationSettings _geoLocationSettings;
        #endregion

        #region Ctor
        public MaxMindProvider(ILogger logger,
            GeoLocationSettings geoLocationSettings)
        {
            _logger = logger;
            _geoLocationSettings = geoLocationSettings;
        }
        #endregion
        
        #region Utilities
        protected virtual async Task<string> FindLocation(string ipAddress)
        {
            try
            {
                var databasePath = CommonHelper.MapContentPath("~/Country.mmdb");
                var reader = new DatabaseReader(databasePath);
                var response = reader.Country(ipAddress);

                if (response != null && response.Country != null && string.IsNullOrEmpty(response.Country.IsoCode))
                    throw new GeoIP2Exception("Country not found");

                return await Task.FromResult(response.Country?.IsoCode);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while finding country.", ex, ipAddress);
                return null;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Find country code by ip address from provider data source
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public virtual async Task<string> FindCountryCode(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return string.Empty;

            var response =await FindLocation(ipAddress);
            if (response != null)
                return response;

            return _geoLocationSettings.DefaultCountryCode;
        }
        #endregion
    }
}