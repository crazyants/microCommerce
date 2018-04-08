using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using microCommerce.Web.Models;
using microCommerce.Mvc.Controllers;

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
