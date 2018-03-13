using Autofac;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using Microsoft.Extensions.Configuration;

namespace microCommerce.Ioc
{
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        void Register(ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfigurationRoot configuration, IAppConfiguration config);

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Priority { get; }
    }
}