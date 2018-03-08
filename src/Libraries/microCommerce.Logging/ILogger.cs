namespace microCommerce.Logging
{
    public interface ILogger
    {
        void Log(LogLevel logLevel,
        string shortMessage,
        string fullMessage = null,
        string ipAddress = null,
        string pageUrl = null,
        string referrerUrl = null);
    }
}