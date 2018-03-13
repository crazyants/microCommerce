using microCommerce.Ioc;
using microCommerce.Mvc.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net;

namespace microCommerce.Mvc.Builders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureApiServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            //add application configuration parameters
            var config = services.ConfigureStartupConfig<ServiceConfiguration>(configuration.GetSection("Service"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfiguration>(configuration.GetSection("Hosting"));

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            engine.Initialize(services);

            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //add mvc engine
            services.AddMvcCore().AddApiExplorer();

            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            services.AddCustomizedSwagger(config);

            //register dependencies
            var serviceProvider = engine.RegisterDependencies(services, configuration);

            return serviceProvider;
        }

        private static void AddCustomizedSwagger(this IServiceCollection services, ServiceConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                string currentVersion = string.Format("v{0}", config.CurrentVersion);
                c.SwaggerDoc(currentVersion, new Info
                {
                    Title = config.ApplicationName,
                    Version = currentVersion,
                    Description = config.ApplicationDescription,
                    Contact = new Contact
                    {
                        Email = "info@microcommerce.org",
                        Url = "https://github.com/fsefacan/microCommerce"
                    },
                    License = new License
                    {
                        Name = "MIT License",
                        Url = "https://github.com/fsefacan/microCommerce/blob/master/LICENSE"
                    }
                });
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