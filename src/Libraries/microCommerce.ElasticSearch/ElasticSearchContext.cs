using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.ElasticSearch
{
    public class ElasticSearchContext : IElasticSearchContext
    {
        #region Fields
        private readonly Lazy<string> _connectionString;
        private volatile IElasticClient _client;
        private readonly object _lock = new object();
        #endregion

        #region Ctor
        public ElasticSearchContext(string connectionString)
        {
            _connectionString = new Lazy<string>(connectionString);
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Gets the elastic client
        /// </summary>
        /// <returns></returns>
        protected virtual IElasticClient GetElasticClient()
        {
            if (_client != null) return _client;

            lock (_lock)
            {
                if (_client != null) return _client;

                var connectionSettings = new ConnectionSettings(new Uri(_connectionString.Value));
                _client = new ElasticClient(connectionSettings);
            }

            return _client;
        }
        #endregion

        #region Index
        /// <summary>
        /// Index a document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual IndexResponse Index<T>(string indexName, T document) where T : class
        {
            var response = GetElasticClient().Index(document, i => i.Index(indexName).Type<T>());

            var indexResponse = new IndexResponse
            {
                IsValid = response.IsValid,
                StatusMessage = response.DebugInformation,
                Exception = response.OriginalException
            };

            return indexResponse;
        }

        /// <summary>
        /// Index a document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task<IndexResponse> IndexAsync<T>(string indexName, T document) where T : class
        {
            var response = await GetElasticClient().IndexAsync(document, i => i.Index(indexName).Type<T>());

            var indexResponse = new IndexResponse
            {
                IsValid = response.IsValid,
                StatusMessage = response.DebugInformation,
                Exception = response.OriginalException
            };

            return indexResponse;
        }

        /// <summary>
        /// Index document collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public virtual IndexResponse BulkIndex<T>(string indexName, IEnumerable<T> documents) where T : class
        {
            var response = GetElasticClient().IndexMany(documents, indexName);

            var indexResponse = new IndexResponse
            {
                IsValid = response.IsValid,
                StatusMessage = response.DebugInformation,
                Exception = response.OriginalException
            };

            return indexResponse;
        }

        /// <summary>
        /// Index document collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public virtual async Task<IndexResponse> BulkIndexAsync<T>(string indexName, IEnumerable<T> documents) where T : class
        {
            var response = await GetElasticClient().IndexManyAsync(documents, indexName);

            var indexResponse = new IndexResponse
            {
                IsValid = response.IsValid,
                StatusMessage = response.DebugInformation,
                Exception = response.OriginalException
            };

            return indexResponse;
        }
        #endregion

        #region Search
        /// <summary>
        /// Search document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="indexName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual SearchResponse<T> Search<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query,
            string indexName,
            int pageIndex = 0,
            int pageSize = int.MaxValue) where T : class
        {
            var response = GetElasticClient().Search<T>(s => s
            .From(pageIndex)
            .Size(pageSize)
            .Query(query));

            var searchResponse = new SearchResponse<T>
            {
                Documents = response.Documents.ToList(),
                IsValid = response.IsValid,
                StatusMessage = response.DebugInformation,
                Exception = response.OriginalException
            };

            return searchResponse;
        }

        public virtual async Task<SearchResponse<T>> SearchAsync<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query,
            string indexName,
            int pageIndex = 0,
            int pageSize = int.MaxValue) where T : class
        {
            var response = await GetElasticClient().SearchAsync<T>(s => s
             .From(pageIndex)
             .Size(pageSize)
             .Query(query));

            var searchResponse = new SearchResponse<T>
            {
                Documents = response.Documents.ToList(),
                IsValid = response.IsValid,
                StatusMessage = response.DebugInformation,
                Exception = response.OriginalException
            };

            return searchResponse;
        }
        #endregion
    }
}