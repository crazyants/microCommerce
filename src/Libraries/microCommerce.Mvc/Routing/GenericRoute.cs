using microCommerce.Ioc;
using microCommerce.Setting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Mvc.Routing
{
    public class GenericRoute : LocalizedRoute
    {
        #region Fields
        private readonly IRouter _target;
        #endregion

        #region Ctor
        public GenericRoute(IRouter target, string routeTemplate, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeTemplate, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public GenericRoute(IRouter target, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints,
            RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public GenericRoute(IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults,
            IDictionary<string, object> constraints,
            RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }
        #endregion

        #region Utilities
        protected RouteValueDictionary GetRouteValues(RouteContext context)
        {
            var seoSettings = EngineContext.Current.Resolve<SeoSettings>();
            //remove language code from the path if it's localized URL
            var path = context.HttpContext.Request.Path.Value;
            if (seoSettings.SeoFriendlyUrlsForLanguagesEnabled && path.IsLocalizedUrl(context.HttpContext.Request.PathBase, false, out string _))
                path = path.RemoveLanguageSeoCodeFromUrl(context.HttpContext.Request.PathBase, false);

            //parse route data
            var routeValues = new RouteValueDictionary(this.ParsedTemplate.Parameters
                .Where(parameter => parameter.DefaultValue != null)
                .ToDictionary(parameter => parameter.Name, parameter => parameter.DefaultValue));
            var matcher = new TemplateMatcher(this.ParsedTemplate, routeValues);
            matcher.TryMatch(path, routeValues);

            return routeValues;
        }
        #endregion

        #region Methods
        public override Task RouteAsync(RouteContext context)
        {
            var routeValues = GetRouteValues(context);
            if (!routeValues.TryGetValue("GenericSeName", out object slugValue) || string.IsNullOrEmpty(slugValue as string))
                return Task.CompletedTask;

            var slug = slugValue as string;

            //since we are here, all is ok with the slug, so process URL
            var currentRouteData = new RouteData(context.RouteData);

            context.RouteData = currentRouteData;

            //route request
            return _target.RouteAsync(context);
        }
        #endregion
    }
}