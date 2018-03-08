using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using microCommerce.Domain;

namespace microCommerce.Dapper
{
    public partial interface IDataContext
    {
        T Find<T>(int Id) where T : BaseEntity;
        Task<T> FindAsync<T>(int Id) where T : BaseEntity;
        bool Exist<T>(int Id) where T : BaseEntity;
        Task<bool> ExistAsync<T>(int Id) where T : BaseEntity;
        int Count<T>() where T : BaseEntity;
        Task<int> CountAsync<T>() where T : BaseEntity;

        IDbTransaction BeginTransaction();
        void OpenConnection();
        IDbConnection Connection { get; }
        int ExecutionTimeOut { get; }

        void Insert<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
        Task InsertAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> InsertBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;

        int Update<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        int UpdateBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> UpdateAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> UpdateBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;

        int Delete<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        int DeleteBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> DeleteAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> DeleteBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
    }
}