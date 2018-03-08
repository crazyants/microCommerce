using microCommerce.Common;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace microCommerce.Mvc.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IWebHelper webHelper, IWorkContext workContext)
        {
            //set working language culture
            var culture = new CultureInfo(workContext.CurrentLanguage.LanguageCulture);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            //call the next middleware in the request pipeline
            return _next(httpContext);
        }
    }
}