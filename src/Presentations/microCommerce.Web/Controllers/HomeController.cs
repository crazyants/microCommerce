using microCommerce.Localization;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class HomeController : FrontBaseController
    {
        private readonly ILocalizationService _localizationService;

        public HomeController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public IActionResult Index()
        {
            //_localizationService.InsertLocalizationResource("Customer.Login", "Giriş Yap", "tr-TR");
            //_localizationService.InsertLocalizationResource("Customer.Register", "Hesap Oluştur", "tr-TR");

            return View();
        }
    }
}