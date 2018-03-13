using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.CustomerApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("CustomerApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}