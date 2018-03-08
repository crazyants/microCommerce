using System;
using microCommerce.MongoDb;

namespace microCommerce.Logging
{
    public class Log : MongoEntity
    {
        public string LogLevel { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public string IpAddress { get; set; }
        public string PageUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}