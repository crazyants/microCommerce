using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Web.Controllers
{
    public class CustomerController : FrontBaseController
    {
        public virtual IActionResult Login()
        {
            return View();
        }

        public virtual IActionResult Register()
        {
            return View();
        }
    }
}