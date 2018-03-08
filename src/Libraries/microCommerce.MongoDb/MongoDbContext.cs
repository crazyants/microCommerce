using MongoDB.Driver;

namespace microCommerce.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        /// <summary>
        /// Get connection string from Mongo Client
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Get database name from Mongo Client
        /// </summary>
        public string DatabaseName { get; private set; }
        
        private IMongoClient client;
        private IMongoDatabase database;
        
        public MongoDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            DatabaseName = MongoUrl.Create(ConnectionString).DatabaseName;
            client = new MongoClient(ConnectionString);
            database = client.GetDatabase(DatabaseName);
        }

        public IMongoCollection<T> Collection<T>() where T : MongoEntity
        {
            return database.GetCollection<T>(typeof(T).Name);
        }
    }
}