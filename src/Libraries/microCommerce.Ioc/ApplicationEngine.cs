using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Domain.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        
        public IServiceProvider RegisterDependencies(IServiceCollection services, IConfigurationRoot configuration, IAppConfiguration config)
        {
            var containerBuilder = new ContainerBuilder();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register assembly finder
            var assemblyFinder = new AssemblyFinder();
            containerBuilder.RegisterInstance(assemblyFinder).As<IAssemblyHelper>().SingleInstance();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = assemblyFinder.FindOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Priority);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, assemblyFinder, configuration, config);

            //register settings
            containerBuilder.RegisterSource(new SettingsSource());

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //return service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            var startupTasks = assemblyFinder.FindOfType<IStartupTask>();
            var taskInstances = startupTasks
                .Select(task => (IStartupTask)Activator.CreateInstance(task))
                .OrderBy(task => task.Priority);

            foreach (var task in taskInstances)
                task.Execute();

            return _serviceProvider;
        }

        /// <summary>
        /// Setting source
        /// </summary>
        public class SettingsSource : IRegistrationSource
        {
            static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
                "BuildRegistration",
                BindingFlags.Static | BindingFlags.NonPublic);

            /// <summary>
            /// Is adapter for individual components
            /// </summary>
            public bool IsAdapterForIndividualComponents { get { return false; } }

            /// <summary>
            /// Settings registrations
            /// </summary>
            /// <param name="service"></param>
            /// <param name="registrations"></param>
            /// <returns></returns>
            public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
            {
                var ts = service as TypedService;
                if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
                {
                    var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                    yield return (IComponentRegistration)buildMethod.Invoke(null, null);
                }
            }

            static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
            {
                return RegistrationBuilder
                    .ForDelegate((c, p) => c.Resolve<ISettingService>().LoadSetting<TSettings>())
                    .InstancePerLifetimeScope()
                    .CreateRegistration();
            }
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