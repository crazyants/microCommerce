namespace microCommerce.Common.Configurations
{
    public interface IAppConfiguration
    {
        /// <summary>
        /// Gets or sets data caching enabled for database performance
        /// </summary>
        bool CachingEnabled { get; set; }

        /// <summary>
        /// Gets or sets redis caching enabled
        /// </summary>
        bool UseRedisCaching { get; set; }

        /// <summary>
        /// Gets or sets redis connection string
        /// </summary>
        string RedisConnectionString { get; set; }

        /// <summary>
        /// Gets or sets redis persist data proctection key
        /// </summary>
        string PersistDataProtectionKeys { get; set; }

        /// <summary>
        /// Logging error, info, warning messages enabled
        /// </summary>
        bool LoggingEnabled { get; set; }

        /// <summary>
        /// Use no sql for logging
        /// </summary>
        bool UseNoSqlLogging { get; set; }

        /// <summary>
        /// Gets or sets no sql connection string
        /// </summary>
        string NoSqlConnectionString { get; set; }
    }
}