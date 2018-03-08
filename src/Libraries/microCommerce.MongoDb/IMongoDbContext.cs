using MongoDB.Driver;

namespace microCommerce.MongoDb
{
    public interface IMongoDbContext
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        IMongoCollection<T> Collection<T>() where T : MongoEntity;
    }
}