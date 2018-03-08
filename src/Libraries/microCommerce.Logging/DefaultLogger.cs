using System;
using microCommerce.MongoDb;

namespace microCommerce.Logging
{
    public class DefaultLogger : ILogger
    {
        private readonly IMongoRepository<Log> _logRepository;

        public DefaultLogger(IMongoRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        public virtual void Log(LogLevel logLevel,
            string shortMessage,
            string fullMessage = null,
            string ipAddress = null,
            string pageUrl = null,
            string referrerUrl = null)
        {
            if (string.IsNullOrEmpty(shortMessage))
                return;

            var log = new Log
            {
                LogLevel = logLevel.ToString(),
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                PageUrl = pageUrl,
                ReferrerUrl = referrerUrl,
                IpAddress = ipAddress,
                CreatedDateUtc = DateTime.UtcNow
            };

            _logRepository.Insert(log);
        }
    }
}