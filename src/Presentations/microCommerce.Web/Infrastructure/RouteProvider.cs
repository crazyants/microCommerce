using microCommerce.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace microCommerce.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //routeBuilder.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
        }

        public int Priority
        {
            get
            {
                return int.MaxValue;
            }
        }
    }
}