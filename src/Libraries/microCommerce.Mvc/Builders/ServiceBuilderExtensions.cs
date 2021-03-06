﻿using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace microCommerce.Mvc.Builders
{
    public static class ServiceBuilderExtensions
    {
        public static void ConfigureApiPipeline(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();

            app.UseCustomizedSwagger();
        }
        
        private static void UseCustomizedSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(s =>
            {
                s.RouteTemplate = "docs/{documentName}/endpoints.json";
            });

            var config = EngineContext.Current.Resolve<ServiceConfiguration>();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint(string.Format("/docs/v{0}/endpoints.json", config.CurrentVersion), config.ApplicationName);
                s.RoutePrefix = "docs";
                s.DocumentTitle = config.ApplicationName;
            });
        }
    }
}