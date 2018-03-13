using Autofac;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using Microsoft.Extensions.Configuration;

namespace microCommerce.Mvc.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfigurationRoot configuration, IAppConfiguration config)
        {
        }

        public int Priority => 0;
    }
}