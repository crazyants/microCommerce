using MongoDB.Driver;
using System;

namespace microCommerce.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        #region Fields
        private readonly Lazy<string> _connectionString;
        private volatile string _databaseName;
        private volatile IMongoClient _client;
        private volatile IMongoDatabase _database;
        private readonly object _lock = new object();
        #endregion

        public MongoDbContext(string connectionString)
        {
            _connectionString = new Lazy<string>(connectionString);
        }

        /// <summary>
        /// Gets the mongodb database
        /// </summary>
        /// <returns></returns>
        public virtual IMongoDatabase GetDatabase()
        {
            if (_client != null && _database != null) return _database;

            lock (_lock)
            {
                if (_client != null && _database != null) return _database;

                var mongoUrl = MongoUrl.Create(_connectionString.Value);
                _client = new MongoClient(mongoUrl);
                _database = _client.GetDatabase(mongoUrl.DatabaseName);
                _databaseName = mongoUrl.DatabaseName;
            }

            return _database;
        }

        #region Properties
        /// <summary>
        /// Get connection string from Mongo Client
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString.Value;
            }
        }

        /// <summary>
        /// Get database name from Mongo Client
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }
        }
        #endregion
    }
}