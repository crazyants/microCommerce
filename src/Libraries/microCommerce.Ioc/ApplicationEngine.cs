using Autofac;
using Autofac.Extensions.DependencyInjection;
using microCommerce.Common;
using microCommerce.Common.Configurations;
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
        protected virtual IServiceProvider GetServiceProvider
        {
            get
            {
                var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
                var context = accessor.HttpContext;

                return context != null ? context.RequestServices : ServiceProvider;
            }
        }
        #endregion

        public virtual IServiceProvider RegisterDependencies(IServiceCollection services, IConfigurationRoot configuration, IAppConfiguration config)
        {
            var containerBuilder = new ContainerBuilder();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register assembly finder
            var assemblyHelper = new AssemblyHelper();
            containerBuilder.RegisterInstance(assemblyHelper).As<IAssemblyHelper>().SingleInstance();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = assemblyHelper.FindOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Priority);

            var dependencyConfig = new DependencyContext
            {
                ContainerBuilder = containerBuilder,
                AssemblyHelper = assemblyHelper,
                ConfigurationRoot = configuration,
                AppConfig = config
            };

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(dependencyConfig);

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //return service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            var startupTasks = assemblyHelper.FindOfType<IStartupTask>();
            var taskInstances = startupTasks
                .Select(task => (IStartupTask)Activator.CreateInstance(task))
                .OrderBy(task => task.Priority);

            foreach (var task in taskInstances)
                task.Execute();

            return _serviceProvider;
        }

        /// <summary>
        /// Gets the instance by generic type from ioc container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Resolve<T>() where T : class
        {
            return GetServiceProvider.GetRequiredService(typeof(T)) as T;
        }

        /// <summary>
        /// Gets the instance by type from ioc container
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object Resolve(Type type)
        {
            return GetServiceProvider.GetRequiredService(type);
        }

        /// <summary>
        /// Gets the multiple instance by generic type from ioc container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return GetServiceProvider.GetServices(typeof(T)) as IEnumerable<T>;
        }

        /// <summary>
        /// Gets the unregistered instance
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object ResolveUnregistered(Type type)
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
        public virtual IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        #endregion
    }
}