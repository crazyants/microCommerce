namespace microCommerce.Common.Configurations
{
    public class WebConfiguration : IAppConfiguration
    {
        /// <summary>
        /// Gets or sets path to database with user agent strings
        /// </summary>
        public string UserAgentFilePath { get; set; }

        /// <summary>
        /// Gets or sets path to database with crawler only user agent strings
        /// </summary>
        public string CrawlerOnlyUserAgentFilePath { get; set; }

        /// <summary>
        /// Gets or sets data caching enabled for database performance
        /// </summary>
        public bool CachingEnabled { get; set; }

        /// <summary>
        /// Gets or sets redis caching enabled
        /// </summary>
        public bool UseRedisCaching { get; set; }

        /// <summary>
        /// Gets or sets redis connection string
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Gets or sets redis persist data proctection key
        /// </summary>
        public string PersistDataProtectionKeys { get; set; }

        /// <summary>
        /// Logging error, info, warning messages enabled
        /// </summary>
        public bool LoggingEnabled { get; set; }

        /// <summary>
        /// Use no sql for logging
        /// </summary>
        public bool UseNoSqlLogging { get; set; }

        /// <summary>
        /// Gets or sets no sql connection string
        /// </summary>
        public string NoSqlConnectionString { get; set; }
    }
}