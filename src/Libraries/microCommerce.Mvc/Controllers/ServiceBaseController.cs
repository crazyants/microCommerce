using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace microCommerce.Mvc.Controllers
{
    public abstract class ServiceBaseController : ControllerBase
    {
        [NonAction]
        public virtual JsonResult Json(object value)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return new JsonResult(value, serializerSettings);
        }
    }
}