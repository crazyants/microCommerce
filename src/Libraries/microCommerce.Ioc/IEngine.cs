using microCommerce.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace microCommerce.Ioc
{
    public interface IEngine
    {
        /// <summary>
        /// Register dependencies using Autofac
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        IServiceProvider RegisterDependencies(IServiceCollection services, IConfigurationRoot configuration, IAppConfiguration config);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type of service</param>
        /// <returns>Resolved service</returns>
        object ResolveUnregistered(Type type);
    }
}