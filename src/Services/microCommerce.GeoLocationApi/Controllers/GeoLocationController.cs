using microCommerce.GeoLocationApi.Services;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace microCommerce.GeoLocationApi.Controllers
{
    public class GeoLocationController : ServiceBaseController
    {
        #region Fields
        private readonly ILocationProvider _locationProvider;
        #endregion

        #region Ctor
        public GeoLocationController(ILocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }
        #endregion

        #region Methods
        [HttpGet("/geolocation")]
        public virtual async Task<IActionResult> FindGeoLocation(string ipAddress)
        {
            return Json(new
            {
                countryCode = await _locationProvider.FindCountryCode(ipAddress)
            });
        }
        #endregion
    }
}