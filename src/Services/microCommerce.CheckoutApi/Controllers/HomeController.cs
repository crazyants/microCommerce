using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.CheckoutApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("CheckoutApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}