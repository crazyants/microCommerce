using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.Mvc.Infrastructure;
using microCommerce.Mvc.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Mvc.Builders
{
    public static class WebBuilderExtensions
    {
        public static void ConfigurePipeline(this IApplicationBuilder app, IHostingEnvironment env)
        {
            //exception handling
            app.UseCustomExceptionHandler();

            //handle 400 errors (bad request)
            app.UseCustomBadRequest();

            //handle 404 errors (not found)
            app.UseCustomPageNotFound();

            app.UseMvc(RegisterRoutes);

            app.UseStaticFiles();
            
            //set culture by user data
            //app.UseCulture();
        }

        private static void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            var assemblyHelper = EngineContext.Current.Resolve<IAssemblyHelper>();
            var routeProviders = assemblyHelper.FindOfType<IRouteProvider>();

            var instances = routeProviders
            .Select(rp => (IRouteProvider)Activator.CreateInstance(rp))
            .OrderBy(rp => rp.Priority);

            foreach (var instance in instances)
                instance.RegisterRoutes(routeBuilder);
        }

        private static void UseCustomExceptionHandler(this IApplicationBuilder application)
        {
            var hostingEnvironment = EngineContext.Current.Resolve<IHostingEnvironment>();
            if (hostingEnvironment.IsDevelopment())
            {
                //get detailed exceptions for developing and testing purposes
                application.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                application.UseExceptionHandler("/error.html");
            }

            //log errors
            application.UseExceptionHandler(handler =>
            {
                handler.Run(context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return Task.CompletedTask;

                    try
                    {
                        var logger = EngineContext.Current.Resolve<ILogger>();
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        logger.Error(exception.Message, exception, webHelper.GetCurrentIpAddress(), webHelper.GetThisPageUrl(true), webHelper.GetUrlReferrer());
                    }
                    finally
                    {
                        //rethrow the exception to show the error page
                        throw exception;
                    }
                });
            });
        }

        private static void UseCustomBadRequest(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(context =>
            {
                //handle 404 (Bad request)
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    var logger = EngineContext.Current.Resolve<ILogger>();
                    logger.Error("Error 400. Bad request");
                }

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 404 status code that do not have a body
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        private static void UseCustomPageNotFound(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                //handle 404 Not Found
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    if (!webHelper.IsStaticResource())
                    {
                        //get original path and query
                        var originalPath = context.HttpContext.Request.Path;
                        var originalQueryString = context.HttpContext.Request.QueryString;

                        //get new path
                        context.HttpContext.Request.Path = "/notfound.html";
                        context.HttpContext.Request.QueryString = QueryString.Empty;

                        try
                        {
                            //re-execute request with new path
                            await context.Next(context.HttpContext);
                        }
                        finally
                        {
                            //return original path to request
                            context.HttpContext.Request.QueryString = originalQueryString;
                            context.HttpContext.Request.Path = originalPath;
                            context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null);
                        }
                    }
                }
            });
        }

        private static void UseCulture(this IApplicationBuilder application)
        {
            application.UseMiddleware<CultureMiddleware>();
        }
    }
}