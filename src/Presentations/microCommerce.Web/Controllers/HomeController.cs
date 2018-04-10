using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace microCommerce.Web.Controllers
{
    public class HomeController : FrontBaseController
    {
        public IActionResult Index()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            return View(assemblies);
        }
    }
}