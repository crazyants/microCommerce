using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.GeoLocationApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("GeoLocationApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}