using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.OrderApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("OrderApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}