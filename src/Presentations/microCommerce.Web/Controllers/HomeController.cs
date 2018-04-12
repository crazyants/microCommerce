using microCommerce.Module.Core;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class HomeController : FrontBaseController
    {
        private readonly IModuleLoader _moduleLoader;

        public HomeController(IModuleLoader moduleLoader)
        {
            _moduleLoader = moduleLoader;
        }

        public IActionResult Index()
        {
            var paymentMethods = _moduleLoader.LoadModules<IPaymentModule>(false);
            return View(paymentMethods);
        }
    }
}