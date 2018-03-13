using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.ProductApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("ProductApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}