using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.Mvc.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace microCommerce.Web
{
    public class Startup
    {
        /// <summary>
        /// Gets the application configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Gets the hosting environments
        /// </summary>
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment env)
        {
            //create configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Environment = env;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.ConfigureServices(Configuration, Environment);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.ConfigurePipeline(Environment);
        }
    }
}