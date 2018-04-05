using System;

namespace microCommerce.ElasticSearch
{
    public class IndexResponse
    {
        /// <summary>
        /// Gets or sets the is valid response
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the response status message
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the response exception
        /// </summary>
        public Exception Exception { get; set; }
    }
}