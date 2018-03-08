using System.Threading.Tasks;
using microCommerce.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace microCommerce.Mvc.Middlewares
{
    public class ResponseCompressionMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseCompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IWebHelper webHelper)
        {
            var accept = httpContext.Request.Headers[HeaderNames.AcceptEncoding];
            if (!StringValues.IsNullOrEmpty(accept))
            {
                httpContext.Response.Headers.Append(HeaderNames.Vary, HeaderNames.AcceptEncoding);
            }
            await _next(httpContext);
        }
    }
}