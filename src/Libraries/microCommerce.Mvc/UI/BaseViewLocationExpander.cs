using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace microCommerce.Mvc.UI
{
    public class BaseViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[] {
                "/Views/{1}/{0}.cshtml",
                "/Views/Shared/{0}.cshtml"
            };
        }
    }
}