using System.Collections.Generic;

namespace microCommerce.ElasticSearch
{
    public class SearchResponse<T> : IndexResponse
    {
        /// <summary>
        /// Gets or sets the search response document collection
        /// </summary>
        public IEnumerable<T> Documents { get; set; }

        /// <summary>
        /// Gets or sets the search responde document total counts
        /// </summary>
        public int TotalRecord { get; set; }
    }
}