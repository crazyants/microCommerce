using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Mvc.UI
{
    public class ModuleViewLocationExpander : IViewLocationExpander
    {
        private const string MODULES_KEY = "application_module_name";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
                //From the type info you should be able to get the assembly
                var controllerAssemblyName = controllerTypeInfo.AsType().Assembly;
            }
        }

        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine potential locations for a view.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="viewLocations">View locations</param>
        /// <returns>iew locations</returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(MODULES_KEY, out string moduleName))
            {
                viewLocations = new[] {
                        $"/Modules/{moduleName}/Views/{{0}}.cshtml",
                        $"/Modules/{moduleName}/Views/Shared/{{0}}.cshtml",
                    }.Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}