using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace microCommerce.MongoDb
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoEntity
    {
        private readonly IMongoDbContext _context;
        private IMongoCollection<T> _document;

        public MongoRepository(IMongoDbContext context)
        {
            _context = context;
        }

        #region Properties
        protected IMongoCollection<T> Document
        {
            get
            {
                if (_document == null)
                    _document = _context.Collection<T>();

                return _document;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IMongoDbContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Get type of IMongoQueryable document
        /// If you want an IEnumerable list make 'Table.ToList()'
        /// </summary>
        public IMongoQueryable<T> Table
        {
            get
            {
                return Document.AsQueryable();
            }
        }
        #endregion

        #region Sync Methods

        #region Get Sync
        /// <summary>
        /// Get single document by expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> filter)
        {
            return Document.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Get single document by object id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Find(object Id)
        {
            return Document.Find(x => x.Id == (Id as string)).FirstOrDefault();
        }

        /// <summary>
        /// Get document list by expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            return Document.Find(filter).ToList();
        }
        #endregion

        #region Insert

        /// <summary>
        /// Add new document range
        /// </summary>
        /// <param name="entities"></param>
        public void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new NullReferenceException(nameof(entities));

                Document.InsertMany(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add new document
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new NullReferenceException(nameof(entity));

                Document.InsertOne(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// Update document range
        /// </summary>
        /// <param name="entities"></param>
        public void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new NullReferenceException(nameof(entities));

                foreach (var entity in entities)
                    Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update document
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new NullReferenceException(nameof(entity));

                var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
                Document.ReplaceOne(filter, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete

        /// <summary>
        /// Delete document range
        /// </summary>
        /// <param name="documents"></param>
        public void Delete(IEnumerable<T> documents)
        {
            try
            {
                if (documents == null)
                    throw new NullReferenceException(nameof(documents));

                foreach (var entity in documents)
                    Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete document
        /// </summary>
        /// <param name="document"></param>
        public void Delete(T document)
        {
            try
            {
                if (document == null)
                    throw new NullReferenceException(nameof(document));

                var filter = Builders<T>.Filter.Eq(x => x.Id, document.Id);
                Document.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Count Sync
        /// <summary>
        /// Get all document count
        /// </summary>
        public int Count
        {
            get
            {
                return Table.Count();
            }
        }
        #endregion

        #endregion

        #region Async Methods

        #region Get Async
        public virtual async Task<T> FindAsync(object Id)
        {
            return await Document.Find(x => x.Id == (Id as string)).FirstOrDefaultAsync();
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> filter)
        {
            return await Document.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<IList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await Document.Find(filter).ToListAsync();
        }
        #endregion

        #region Insert Async
        public virtual async Task InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new NullReferenceException(nameof(entity));

                await Document.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task InsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new NullReferenceException(nameof(entities));

                await Document.InsertManyAsync(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update Async
        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new NullReferenceException(nameof(entity));

                var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
                await Document.ReplaceOneAsync(filter, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new NullReferenceException(nameof(entities));

                foreach (var entity in entities)
                    await UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete Async
        public virtual async Task DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new NullReferenceException(nameof(entity));

                var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
                await Document.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new NullReferenceException(nameof(entities));

                foreach (var entity in entities)
                    await DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Count Async
        public Task<int> CountAsync
        {
            get
            {
                return Table.CountAsync();
            }
        }
        #endregion

        #endregion
    }
}