using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace microCommerce.MongoDb
{
    public interface IMongoRepository<T> where T : MongoEntity
    {
        IMongoDbContext Context { get; }
        IMongoQueryable<T> Table { get; }
        int Count { get; }
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        T Find(object Id);
        T Find(Expression<Func<T, bool>> filter);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter);

        Task<int> CountAsync { get; }
        Task InsertAsync(T entity);
        Task InsertAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);
        Task<T> FindAsync(object Id);
        Task<T> FindAsync(Expression<Func<T, bool>> filter);
        Task<IList<T>> GetAsync(Expression<Func<T, bool>> filter);
    }
}