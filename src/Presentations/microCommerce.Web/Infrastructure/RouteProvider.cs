using microCommerce.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using microCommerce.Mvc.Routing;

namespace microCommerce.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "Login",
                template: "Customer/Login",
                defaults: new { controller = "Customer", action = "Login" });

            routeBuilder.MapRoute(
                name: "Register",
                template: "Customer/Register",
                defaults: new { controller = "Customer", action = "Register" });

            routeBuilder.MapRoute(
                name: "HomePage",
                template: "");
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