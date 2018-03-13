using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.ShippingApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("ShippingApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}