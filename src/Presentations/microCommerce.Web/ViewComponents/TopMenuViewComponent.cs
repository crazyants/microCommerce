using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.ViewComponents
{
    public class TopMenuViewComponent : BaseViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View();
        }
    }
}