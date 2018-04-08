using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace microCommerce.BasketApi.Controllers
{
    [Route("/")]
    public class HomeController : ServiceBaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("BasketApi is a live", "text/plain", Encoding.UTF8);
        }
    }
}