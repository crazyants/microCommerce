using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.ViewComponents
{
    public class TopMenuViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}