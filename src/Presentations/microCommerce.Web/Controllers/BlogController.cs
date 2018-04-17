using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Web.Controllers
{
    public class BlogController : FrontBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}