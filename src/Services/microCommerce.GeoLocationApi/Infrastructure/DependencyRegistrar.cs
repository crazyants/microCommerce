using Autofac;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.GeoLocationApi.Services;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.MongoDb;
using Microsoft.Extensions.Configuration;

namespace microCommerce.GeoLocationApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfigurationRoot configuration, IAppConfiguration config)
        {
            //register logging
            if (config.LoggingEnabled)
            {
                //register mongodb logging
                if (config.UseNoSqlLogging)
                {
                    builder.RegisterInstance(new MongoDbContext(config.NoSqlConnectionString)).As<IMongoDbContext>().SingleInstance();
                    builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>)).InstancePerLifetimeScope();
                    builder.RegisterType<MongoDbLogger>().As<ILogger>().InstancePerLifetimeScope();
                }
                else
                    builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();
            }
            //register null logger
            else
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();

            //register services
            builder.RegisterType<MaxMindProvider>().As<ILocationProvider>().InstancePerLifetimeScope();
        }

        public int Priority => 1;
    }
}