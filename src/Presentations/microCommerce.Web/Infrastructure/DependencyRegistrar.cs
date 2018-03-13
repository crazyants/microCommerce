using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.MongoDb;
using microCommerce.Mvc;
using microCommerce.Mvc.Infrastructure;
using microCommerce.Redis;
using Microsoft.Extensions.Configuration;

namespace microCommerce.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfigurationRoot configuration, IAppConfiguration config)
        {
            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            if (config.UseRedisCaching)
            {
                builder.RegisterInstance(new RedisConnectionWrapper(config.RedisConnectionString)).As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            }
            else
                builder.RegisterType<MemoryCacheManager>().As<IStaticCacheManager>().SingleInstance();

            if (config.LoggingEnabled)
            {
                if (config.UseNoSqlLogging)
                {
                    builder.RegisterInstance(new MongoDbContext(config.NoSqlConnectionString)).As<IMongoDbContext>().SingleInstance();
                    builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>)).InstancePerLifetimeScope();
                    builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
                }
            }
            else
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();
        }

        public int Priority => 1;
    }
}