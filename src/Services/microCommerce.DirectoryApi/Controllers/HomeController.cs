using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.DirectoryApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("DirectoryApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}