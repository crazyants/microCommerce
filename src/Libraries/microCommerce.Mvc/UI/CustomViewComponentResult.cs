using microCommerce.Ioc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace microCommerce.Mvc.UI
{
    public class CustomViewComponentResult : IViewComponentResult
    {
        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewDataDictionary"/>.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITempDataDictionary"/> instance.
        /// </summary>
        public ITempDataDictionary TempData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewEngine"/>.
        /// </summary>
        public IViewEngine ViewEngine { get; set; }

        /// <summary>
        /// Locates and renders a view specified by <see cref="ViewName"/>. If <see cref="ViewName"/> is <c>null</c>,
        /// then the view name searched for is<c>&quot;Default&quot;</c>.
        /// </summary>
        /// <param name="context">The <see cref="ViewComponentContext"/> for the current component execution.</param>
        /// <remarks>
        /// This method synchronously calls and blocks on <see cref="ExecuteAsync(ViewComponentContext)"/>.
        /// </remarks>
        public void Execute(ViewComponentContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            
            ExecuteAsync(context).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Locates and renders a view specified by <see cref="ViewName"/>. If <see cref="ViewName"/> is <c>null</c>,
        /// then the view name searched for is<c>&quot;Default&quot;</c>.
        /// </summary>
        /// <param name="context">The <see cref="ViewComponentContext"/> for the current component execution.</param>
        /// <returns>A <see cref="Task"/> which will complete when view rendering is completed.</returns>
        public async Task ExecuteAsync(ViewComponentContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            string viewName = ViewName;
            if (string.IsNullOrEmpty(viewName))
                viewName = context.ViewComponentDescriptor.ShortName;

            var viewEngine = ViewEngine ?? ResolveViewEngine(context);
            var viewContext = context.ViewContext;
            
            // If view name was passed in is already a path, the view engine will handle this.
            ViewEngineResult result = viewEngine.GetView(viewContext.ExecutingFilePath, viewName, false);

            if (result == null || !result.Success)
            {
                // This will produce a string like:
                //
                //  Views/Shared/Components/Cart.cshtml
                //
                
                result = viewEngine.FindView(viewContext, string.Format(CultureInfo.InvariantCulture, "Components/{0}", viewName), false);
            }

            var view = result.EnsureSuccessful(result.SearchedLocations).View;
            using (view as IDisposable)
            {
                var childViewContext = new ViewContext(
                    viewContext,
                    view,
                    ViewData ?? context.ViewData,
                    context.Writer);
                await view.RenderAsync(childViewContext);
            }
        }

        private static IViewEngine ResolveViewEngine(ViewComponentContext context)
        {
            return EngineContext.Current.Resolve<ICompositeViewEngine>();
        }
    }
}