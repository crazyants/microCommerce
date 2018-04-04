using microCommerce.Ioc;
using microCommerce.Common.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Razor;
using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Builder;

namespace microCommerce.Mvc.Builders
{
    public static class WebCollectionExtensions
    {
        public static IServiceProvider ConfigureServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            //add application configuration parameters
            var config = services.ConfigureStartupConfig<WebConfiguration>(configuration.GetSection("Application"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfiguration>(configuration.GetSection("Hosting"));

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            //engine.Initialize(services);

            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //add mvc engine
            services.AddMvc();

            //add response compression
            services.AddResponseCompression();

            //add custom view engine
            services.AddViewEngine();

            //add theme support
            services.AddThemeSupport();

            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //add anti forgery
            services.AddCustomAntiForgery();

            //add custom session
            services.AddCustomHttpSession();

            //register dependencies
            return engine.RegisterDependencies(services, configuration, config);
        }

        private static void AddViewEngine(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Clear();
                options.ViewLocationExpanders.Add(new BaseViewLocationExpander());
            });
        }

        private static void AddThemeSupport(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddCustomAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".micro.Antiforgery";
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddCustomHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = ".micro.Session";
                options.Cookie.HttpOnly = true;
            });
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        private static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }
    }
}