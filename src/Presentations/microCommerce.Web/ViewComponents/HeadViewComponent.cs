using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.ViewComponents
{
    public class HeadViewComponent : BaseViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View();
        }
    }
}