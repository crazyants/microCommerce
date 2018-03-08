namespace microCommerce.Logging
{
    public class NullLogger : ILogger
    {
        public virtual void Log(LogLevel logLevel,
        string shortMessage,
        string fullMessage = null,
        string ipAddress = null,
        string pageUrl = null,
        string referrerUrl = null)
        {
        }
    }
}