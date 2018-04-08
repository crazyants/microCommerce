using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

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
        
        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exception">Exception</param>
        [NonAction]
        protected virtual void LogException(Exception exception)
        {
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.Error(exception.Message, exception, webHelper.GetCurrentIpAddress(), webHelper.GetThisPageUrl(true), webHelper.GetUrlReferrer());
        }
    }
}