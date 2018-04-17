using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class HomeController : FrontBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}