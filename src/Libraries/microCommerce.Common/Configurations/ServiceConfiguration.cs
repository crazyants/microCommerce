namespace microCommerce.Common.Configurations
{
    public class ServiceConfiguration : IAppConfiguration
    {
        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the aplication description
        /// </summary>
        public string ApplicationDescription { get; set; }

        /// <summary>
        /// Gets or sets the api current working version
        /// </summary>
        public int CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the connection strings
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database provider
        /// </summary>
        public string DatabaseProviderName { get; set; }

        /// <summary>
        /// Gets or sets data caching enabled for database performance
        /// </summary>
        public bool CachingEnabled { get; set; }

        /// <summary>
        /// Gets or sets redis caching enabled
        /// </summary>
        public bool UseRedisCaching { get; set; }

        /// <summary>
        /// Gets or sets redis database index
        /// </summary>
        public int CacheDatabaseIndex { get; set; }

        /// <summary>
        /// Gets or sets redis connection string
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Gets or sets redis persist data proctection key
        /// </summary>
        public string PersistDataProtectionKeys { get; set; }

        /// <summary>
        /// Gets or sets redis session storage
        /// </summary>
        public bool UseRedisSession { get; set; }

        /// <summary>
        /// Gets or sets redis session database index
        /// </summary>
        public int SessionDatabaseIndex { get; set; }

        /// <summary>
        /// Logging error, info, warning messages enabled
        /// </summary>
        public bool LoggingEnabled { get; set; }

        /// <summary>
        /// Use no sql for logging
        /// </summary>
        public bool UseNoSqlLogging { get; set; }

        /// <summary>
        /// Gets or sets logging database name
        /// </summary>
        public bool LoggingDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets no sql connection string
        /// </summary>
        public string NoSqlConnectionString { get; set; }
    }
}