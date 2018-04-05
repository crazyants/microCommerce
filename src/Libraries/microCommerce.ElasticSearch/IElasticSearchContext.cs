using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microCommerce.ElasticSearch
{
    public interface IElasticSearchContext
    {
        /// <summary>
        /// Index a document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        IndexResponse Index<T>(string indexName, T document) where T : class;

        /// <summary>
        /// Index a document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        Task<IndexResponse> IndexAsync<T>(string indexName, T document) where T : class;

        /// <summary>
        /// Index document collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        IndexResponse BulkIndex<T>(string indexName, IEnumerable<T> documents) where T : class;

        /// <summary>
        /// Index document collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        Task<IndexResponse> BulkIndexAsync<T>(string indexName, IEnumerable<T> documents) where T : class;

        /// <summary>
        /// Search document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="indexName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        SearchResponse<T> Search<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query,
            string indexName,
            int pageIndex = 0,
            int pageSize = int.MaxValue) where T : class;

        /// <summary>
        /// Search document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="indexName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<SearchResponse<T>> SearchAsync<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query,
            string indexName,
            int pageIndex = 0,
            int pageSize = int.MaxValue) where T : class;
    }
}