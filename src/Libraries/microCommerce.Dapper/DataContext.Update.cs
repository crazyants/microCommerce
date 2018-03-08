using Dapper;
using microCommerce.Common;
using microCommerce.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace microCommerce.Dapper
{
    public partial class DataContext
    {
        /// <summary>
        /// Update an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual int Update<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNull(item);

            Type entityType = typeof(T);
            string commandText = _provider.UpdateQuery(entityType.Name, item, GetColumns(entityType));

            //execute
            return _connection.Execute(commandText, item, transaction);
        }

        /// <summary>
        /// Update item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        public virtual int UpdateBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNullOrEmpty(items);

            Type entityType = typeof(T);
            string commandText = _provider.UpdateBulkQuery(entityType.Name, items, GetColumns(entityType));
            var parameters = GetParameters(items);

            //execute
            return _connection.Execute(commandText, parameters, transaction);
        }

        public async Task<int> UpdateAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNull(item);

            Type entityType = typeof(T);
            string commandText = _provider.UpdateQuery(entityType.Name, item, GetColumns(entityType));

            //execute
            return await _connection.ExecuteAsync(commandText, item, transaction);
        }

        public async Task<int> UpdateBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Check.IsNullOrEmpty(items);

            Type entityType = typeof(T);
            string commandText = _provider.UpdateBulkQuery(entityType.Name, items, GetColumns(entityType));
            var parameters = GetParameters(items);

            //execute
            return await _connection.ExecuteAsync(commandText, parameters, transaction);
        }
    }
}