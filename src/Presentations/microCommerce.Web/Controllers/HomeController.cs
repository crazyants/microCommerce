using microCommerce.Module.Core;
using microCommerce.Module.Core.Payments;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class HomeController : FrontBaseController
    {
        private readonly IModuleProvider _moduleLoader;

        public HomeController(IModuleProvider moduleLoader)
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