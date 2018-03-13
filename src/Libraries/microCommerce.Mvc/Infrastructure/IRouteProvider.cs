using Microsoft.AspNetCore.Routing;

namespace microCommerce.Mvc.Infrastructure
{
    public interface IRouteProvider
    {
        void RegisterRoutes(IRouteBuilder routeBuilder);
        int Priority { get; }
    }
}
