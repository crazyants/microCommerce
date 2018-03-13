using microCommerce.Mvc;
using microCommerce.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {

        }

        public int Priority => int.MaxValue;
    }
}