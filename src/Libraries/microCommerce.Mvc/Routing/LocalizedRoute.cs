using microCommerce.Ioc;
using microCommerce.Setting;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microCommerce.Mvc.Routing
{
    public class LocalizedRoute : Route
    {
        #region Fields
        private readonly IRouter _target;
        #endregion

        #region Ctor
        public LocalizedRoute(IRouter target, string routeTemplate, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeTemplate, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public LocalizedRoute(IRouter target, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints,
            RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public LocalizedRoute(IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints, RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }
        #endregion

        #region Methods
        public override VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            var data = base.GetVirtualPath(context);
            if (data == null)
                return null;

            var seoSettings = EngineContext.Current.Resolve<SeoSettings>();
            if (!seoSettings.SeoFriendlyUrlsForLanguagesEnabled)
                return data;

            //add language code to page URL in case if it's localized URL
            var path = context.HttpContext.Request.Path.Value;
            if (path.IsLocalizedUrl(context.HttpContext.Request.PathBase, false, out string uniqueSeoCode))
                data.VirtualPath = $"/{uniqueSeoCode}{data.VirtualPath}";

            return data;
        }

        public override Task RouteAsync(RouteContext context)
        {
            var seoSettings = EngineContext.Current.Resolve<SeoSettings>();
            if (!seoSettings.SeoFriendlyUrlsForLanguagesEnabled)
                return base.RouteAsync(context);

            //if path isn't localized, no special action required
            var path = context.HttpContext.Request.Path.Value;
            if (!path.IsLocalizedUrl(context.HttpContext.Request.PathBase, false, out string _))
                return base.RouteAsync(context);

            //remove language code and application path from the path
            var newPath = path.RemoveLanguageSeoCodeFromUrl(context.HttpContext.Request.PathBase, false);

            //set new request path and try to get route handler
            context.HttpContext.Request.Path = newPath;
            base.RouteAsync(context).Wait();

            //then return the original request path
            context.HttpContext.Request.Path = path;

            return _target.RouteAsync(context);
        }
        #endregion
    }
}