using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.MediaApi.Controllers
{
    [Route("/")]
    public class HomeController : ServiceBaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("MediaApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}