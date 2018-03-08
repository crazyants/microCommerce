using Autofac;
using Autofac.Extensions.DependencyInjection;
using microCommerce.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Ioc
{
    public class ApplicationEngine : IEngine
    {
        #region Fields
        private IServiceProvider _serviceProvider { get; set; }
        #endregion

        #region Utilities
        protected IServiceProvider GetServiceProvider
        {
            get
            {
                var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
                var context = accessor.HttpContext;

                return context != null ? context.RequestServices : ServiceProvider;
            }
        }
        #endregion

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public virtual void Initialize(IServiceCollection services)
        {
            //set base application path
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            CommonHelper.BaseDirectory = hostingEnvironment.ContentRootPath;
        }

        public IServiceProvider RegisterDependencies(IServiceCollection services, IConfigurationRoot configuration)
        {
            var containerBuilder = new ContainerBuilder();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register assembly helper
            var assemblyHelper = new AssemblyHelper();
            containerBuilder.RegisterInstance(assemblyHelper).As<IAssemblyHelper>().SingleInstance();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = assemblyHelper.FindOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Priority);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, assemblyHelper, configuration);

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //return service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            return _serviceProvider;
        }

        public T Resolve<T>() where T : class
        {
            return GetServiceProvider.GetRequiredService(typeof(T)) as T;
        }

        public object Resolve(Type type)
        {
            return GetServiceProvider.GetRequiredService(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return GetServiceProvider.GetServices(typeof(T)) as IEnumerable<T>;
        }

        public object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new CustomException("Unknown dependency");

                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new CustomException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #region Properties
        public virtual IServiceProvider ServiceProvider => _serviceProvider;
        #endregion
    }
}