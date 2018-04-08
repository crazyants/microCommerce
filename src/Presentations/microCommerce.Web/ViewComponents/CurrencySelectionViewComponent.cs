using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.ViewComponents
{
    public class CurrencySelectionViewComponent : BaseViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View();
        }
    }
}