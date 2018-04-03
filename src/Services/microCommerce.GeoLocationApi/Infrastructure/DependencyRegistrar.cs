using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.GeoLocationApi.Services;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.MongoDb;
using microCommerce.Redis;
using Microsoft.Extensions.Configuration;

namespace microCommerce.GeoLocationApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfigurationRoot configuration, IAppConfiguration config)
        {
            var serviceConfig = config as ServiceConfiguration;
            
            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            if (serviceConfig.UseRedisCaching)
            {
                //register redis cache manager
                builder.RegisterInstance(new RedisConnectionWrapper(serviceConfig.RedisConnectionString)).As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            }
            //register memory cache manager
            else
                builder.RegisterType<MemoryCacheManager>().As<IStaticCacheManager>().SingleInstance();

            //register logging
            if (serviceConfig.LoggingEnabled)
            {
                //register mongodb logging
                if (serviceConfig.UseNoSqlLogging)
                {
                    builder.RegisterInstance(new MongoDbContext(serviceConfig.NoSqlConnectionString)).As<IMongoDbContext>().SingleInstance();
                    builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>)).InstancePerLifetimeScope();
                    builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
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