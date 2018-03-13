using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.MediaApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("MediaApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}