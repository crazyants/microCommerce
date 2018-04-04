using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.ViewComponents
{
    public class HeaderViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}