using Microsoft.AspNetCore.Routing;

namespace microCommerce.Mvc
{
    public interface IRouteProvider
    {
        void RegisterRoutes(IRouteBuilder routeBuilder);
        int Priority { get; }
    }
}
