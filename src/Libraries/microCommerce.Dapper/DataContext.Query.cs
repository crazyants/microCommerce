using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using microCommerce.Domain;
using Dapper;

namespace microCommerce.Dapper
{
    public partial class DataContext
    {
        /// <summary>
        /// Find item by identifier
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id">Default parameter name is @Id</param>
        /// <returns></returns>
        public virtual T Find<T>(int Id) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.SelectFirstQuery<T>(entityType.Name, GetColumns(entityType, true));
            
            //execute first query
            return _connection.QueryFirstOrDefault<T>(commandText, new { Id });
        }

        /// <summary>
        /// Find item by identifier
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T> FindAsync<T>(int Id) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.SelectFirstQuery<T>(entityType.Name, GetColumns(entityType, true));
            
            //execute first query
            return await _connection.QueryFirstOrDefaultAsync<T>(commandText, new { Id });
        }

        public virtual bool Exist<T>(int Id) where T : BaseEntity
        {
            string commandText = _provider.ExistingQuery(typeof(T).Name);
            
            //execute existing query
            return _connection.ExecuteScalar<bool>(commandText, new { Id });
        }

        public virtual async Task<bool> ExistAsync<T>(int Id) where T : BaseEntity
        {
            string commandText = _provider.ExistingQuery(typeof(T).Name);
            
            //execute existing query
            return await _connection.ExecuteScalarAsync<bool>(commandText, new { Id });
        }

        public virtual int Count<T>() where T : BaseEntity
        {
            string commandText = _provider.CountQuery(typeof(T).Name);
            
            //execute existing query
            return _connection.ExecuteScalar<int>(commandText);
        }

        public virtual async Task<int> CountAsync<T>() where T : BaseEntity
        {
            string commandText = _provider.CountQuery(typeof(T).Name);
            
            //execute existing query
            return await _connection.ExecuteScalarAsync<int>(commandText);
        }
    }
}