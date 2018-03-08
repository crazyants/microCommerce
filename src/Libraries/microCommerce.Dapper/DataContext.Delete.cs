using microCommerce.Domain;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using microCommerce.Common;

namespace microCommerce.Dapper
{
    public partial class DataContext
    {
        /// <summary>
        /// Delete an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual int Delete<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNull(item);

            string commandText = _provider.DeleteQuery(typeof(T).Name);

            //execute
            return _connection.Execute(commandText, new { item.Id }, transaction);
        }

        /// <summary>
        /// Delete item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        public virtual int DeleteBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNullOrEmpty(items);

            string commandText = _provider.DeleteBulkQuery(typeof(T).Name);
            var parameters = items.Select(x => x.Id).ToArray();

            //execute
            return _connection.Execute(commandText, new { Ids = parameters });
        }

        public async Task<int> DeleteAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNull(item);

            string commandText = _provider.DeleteQuery(typeof(T).Name);

            //execute
            return await _connection.ExecuteAsync(commandText, new { item.Id }, transaction);
        }

        public async Task<int> DeleteBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNullOrEmpty(items);

            string commandText = _provider.DeleteBulkQuery(typeof(T).Name);
            var parameters = GetParameters(items);

            //execute
            return await _connection.ExecuteAsync(commandText, parameters, transaction);
        }
    }
}