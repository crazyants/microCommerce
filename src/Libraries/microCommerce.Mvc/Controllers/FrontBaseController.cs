using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Mvc.Controllers
{
    public abstract class FrontBaseController : BaseController
    {
        public virtual IActionResult HomePage()
        {
            return RedirectToRoute("HomePage");
        }
    }
}