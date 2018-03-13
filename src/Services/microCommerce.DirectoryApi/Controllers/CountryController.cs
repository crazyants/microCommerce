using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.DirectoryApi.Controllers
{
    [Route("/")]
    public class CountryController : ServiceBaseController
    {
        [HttpGet]
        [Route("/countries")]
        public virtual IActionResult Countries()
        {
            return Ok();
        }

        [HttpGet]
        [Route("/countries/{Id:int}")]
        public virtual IActionResult CountryById(int Id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/countries/{twoLetterIsoCode}")]
        public virtual IActionResult CountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            return Ok();
        }
    }
}