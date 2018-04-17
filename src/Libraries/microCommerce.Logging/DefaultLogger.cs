using microCommerce.Common;
using System;
using System.IO;

namespace microCommerce.Logging
{
    public class DefaultLogger : ILogger
    {
        #region Fields

        #endregion

        #region Ctor
        public DefaultLogger()
        {

        }
        #endregion

        #region Methods
        public virtual void Log(LogLevel logLevel,
            string shortMessage,
            string fullMessage = null,
            string ipAddress = null,
            string pageUrl = null,
            string referrerUrl = null)
        {
            if (string.IsNullOrEmpty(shortMessage))
                return;
            
            string logMessage = string.Format("{0} - {1} - {2} - {3} - {4} - {5} - {6}",
                DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss"),
                logLevel.ToString(),
                ipAddress,
                shortMessage,
                fullMessage,
                pageUrl,
                referrerUrl);

            using (StreamWriter sw = File.AppendText(CommonHelper.MapContentPath(string.Format("logs/{0:dd.MM.yyyy}.txt", DateTime.UtcNow))))
            {
                sw.WriteLine(logMessage);
            }
        }
        #endregion
    }
}