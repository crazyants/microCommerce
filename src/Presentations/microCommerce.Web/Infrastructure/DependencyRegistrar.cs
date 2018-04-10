using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using microCommerce.Localization;
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

            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterInstance(new MongoDbContext(config.NoSqlConnectionString)).As<IMongoDbContext>().SingleInstance();
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>)).InstancePerLifetimeScope();

            if (config.CachingEnabled)
            {
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
            }
            else
                builder.RegisterType<NullCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            if (config.LoggingEnabled)
            {
                if (config.UseNoSqlLogging)
                {
                    builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
                }
                else
                    builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();
            }
            else
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();

            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
        }

        public int Priority
        {
            get { return 1; }
        }
    }
}