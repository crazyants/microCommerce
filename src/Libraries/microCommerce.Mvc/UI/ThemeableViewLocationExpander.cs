using System.Collections.Generic;
using System.Linq;
using microCommerce.Common;
using Microsoft.AspNetCore.Mvc.Razor;

namespace microCommerce.Mvc.UI
{
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "application_theme_name";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var serviceProvider = context.ActionContext.HttpContext.RequestServices;
            var workContext = serviceProvider.GetService(typeof(IWorkContext)) as IWorkContext;
            context.Values[THEME_KEY] = workContext.CurrentTheme.Name;
        }

        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine potential locations for a view.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="viewLocations">View locations</param>
        /// <returns>iew locations</returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                viewLocations = new[] {
                        $"/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                    }.Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}