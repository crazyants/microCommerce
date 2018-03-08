using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

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
        }
    }
}