using MongoDB.Driver;

namespace microCommerce.MongoDb
{
    public interface IMongoDbContext
    {
        /// <summary>
        /// Gets mongodb database
        /// </summary>
        /// <returns></returns>
        IMongoDatabase GetDatabase();

        /// <summary>
        /// Gets the mongodb connection strings
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the mongodb current database name
        /// </summary>
        string DatabaseName { get; }
    }
}