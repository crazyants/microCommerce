using microCommerce.Dapper.Providers;
using microCommerce.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace microCommerce.Dapper
{
    public partial class DataContext : IDataContext, IDisposable
    {
        #region Fields
        private readonly IProvider _provider;
        private readonly IDbConnection _connection;
        private readonly int _executionTimeOut = 30;
        #endregion

        #region Ctor
        public DataContext(IProvider provider, IDbConnection connection)
        {
            _provider = provider;
            _connection = connection;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Get parameter name and value from item collections to dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> GetParameters<T>(IEnumerable<T> items)
        {
            var parameters = new Dictionary<string, object>();
            var entityArray = items.ToArray();
            var entityType = entityArray[0].GetType();
            for (int i = 0; i < entityArray.Length; i++)
            {
                var properties = entityArray[i].GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                properties = properties.Where(x => x.Name != "Id").ToArray();

                foreach (var property in properties)
                    parameters.Add(property.Name + (i + 1), entityType.GetProperty(property.Name).GetValue(entityArray[i], null));
            }

            return parameters;
        }

        private static readonly ConcurrentDictionary<Type, IList<string>> _cachedParameters = new ConcurrentDictionary<Type, IList<string>>();

        /// <summary>
        /// Get entity columns with thread safe
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="includeIdentity"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetColumns(Type entityType, bool includeIdentity = false)
        {
            if (!_cachedParameters.TryGetValue(entityType, out IList<string> parameters))
            {
                parameters = new List<string>();
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetGetMethod(false) != null);
                foreach (var prop in properties)
                {
                    var attribute = prop.GetCustomAttribute(typeof(ColumnIgnoreAttribute), true);
                    if (attribute == null)
                        parameters.Add(prop.Name);
                }
                _cachedParameters[entityType] = parameters;
            }

            if (!includeIdentity)
                return parameters.Where(p => p != "Id");

            return parameters;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Begin transcation scope
        /// </summary>
        /// <returns></returns>
        public virtual IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        /// <summary>
        /// Open connection with whether open or close
        /// </summary>
        public virtual void OpenConnection()
        {
            if (_connection != null &&
                (_connection.State != ConnectionState.Open || _connection.State != ConnectionState.Connecting))
                _connection.Open();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current connection
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        /// <summary>
        /// Gets the execution time out second
        /// </summary>
        /// <returns></returns>
        public virtual int ExecutionTimeOut
        {
            get
            {
                return _executionTimeOut > 0 ? _executionTimeOut : 30;
            }
        }

        /// <summary>
        /// Dispose the current connection
        /// </summary>
        public virtual void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
        #endregion
    }
}