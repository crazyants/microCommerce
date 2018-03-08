using System;
using System.Threading;

namespace microCommerce.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string message,
        Exception exception = null, string ipAddress = null,
        string pageUrl = null,string referrerUrl = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            var fullMessage = exception?.ToString() ?? string.Empty;
            logger.Log(LogLevel.Debug, message, fullMessage, ipAddress, pageUrl,referrerUrl);
        }

        public static void Info(this ILogger logger, string message,
        Exception exception = null, string ipAddress = null,
        string pageUrl = null,string referrerUrl = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            var fullMessage = exception?.ToString() ?? string.Empty;
            logger.Log(LogLevel.Info, message, fullMessage, ipAddress, pageUrl,referrerUrl);
        }

        public static void Warn(this ILogger logger, string message,
        Exception exception = null, string ipAddress = null,
        string pageUrl = null,string referrerUrl = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            var fullMessage = exception?.ToString() ?? string.Empty;
            logger.Log(LogLevel.Warn, message, fullMessage, ipAddress, pageUrl,referrerUrl);
        }

        public static void Error(this ILogger logger, string message,
        Exception exception = null, string ipAddress = null,
        string pageUrl = null,string referrerUrl = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            var fullMessage = exception?.ToString() ?? string.Empty;
            logger.Log(LogLevel.Error, message, fullMessage, ipAddress, pageUrl,referrerUrl);
        }

        public static void Fatal(this ILogger logger, string message,
        Exception exception = null, string ipAddress = null,
        string pageUrl = null,string referrerUrl = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            var fullMessage = exception?.ToString() ?? string.Empty;
            logger.Log(LogLevel.Fatal, message, fullMessage, ipAddress, pageUrl,referrerUrl);
        }
    }
}