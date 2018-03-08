using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Mvc.Controllers
{
    public abstract class AdminBaseController : BaseController
    {
        public virtual IActionResult DashBoard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}