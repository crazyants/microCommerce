using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.CategoryApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("CategoryApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}