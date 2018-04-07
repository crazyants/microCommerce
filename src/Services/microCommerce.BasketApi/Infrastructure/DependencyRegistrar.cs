using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Dapper;
using microCommerce.Dapper.Providers;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.MongoDb;
using microCommerce.Mvc.Infrastructure;
using microCommerce.Redis;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace microCommerce.BasketApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfigurationRoot configuration, IAppConfiguration config)
        {
            var serviceConfig = config as ServiceConfiguration;

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

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
            }
            //register null logger
            else
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();

            //register dapper data context
            var provider = ProviderFactory.GetProvider(serviceConfig.DatabaseProviderName);
            var connection = provider.CreateConnection(serviceConfig.ConnectionString);
            builder.RegisterInstance(connection).As<IDbConnection>().SingleInstance();
            builder.RegisterInstance(provider).As<IDataProvider>().SingleInstance();
            builder.RegisterInstance(new DataContext(provider, connection)).As<IDataContext>().SingleInstance();
        }

        public int Priority => 1;
    }
}