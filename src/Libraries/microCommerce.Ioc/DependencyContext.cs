using Autofac;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using Microsoft.Extensions.Configuration;

namespace microCommerce.Ioc
{
    public class DependencyContext
    {
        /// <summary>
        /// Gets or sets the ioc container
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; set; }

        /// <summary>
        /// Gets or sets the assembly helper
        /// </summary>
        public IAssemblyHelper AssemblyHelper { get; set; }

        /// <summary>
        /// Gets or sets the app configuration root
        /// </summary>
        public IConfigurationRoot ConfigurationRoot { get; set; }

        /// <summary>
        /// Gets or sets the application config
        /// </summary>
        public IAppConfiguration AppConfig { get; set; }
    }
}