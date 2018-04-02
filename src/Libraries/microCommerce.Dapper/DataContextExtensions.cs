using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace microCommerce.Dapper
{
    public static class DataContextExtensions
    {
        #region Sql Command
        public static bool Exist(this IDataContext context, string commandText, object parameters = null)
        {
            return context.Connection.ExecuteScalar<bool>(commandText, parameters);
        }

        public static async Task<bool> ExistAsync(this IDataContext context, string commandText, object parameters = null)
        {
            return await context.Connection.ExecuteScalarAsync<bool>(commandText, parameters);
        }

        public static int Count(this IDataContext context, string commandText, object parameters = null)
        {
            return context.Connection.ExecuteScalar<int>(commandText, parameters);
        }

        public static async Task<int> CountAsync(this IDataContext context, string commandText, object parameters = null)
        {
            return await context.Connection.ExecuteScalarAsync<int>(commandText, parameters);
        }

        public static int Execute(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.Execute(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static async Task<int> ExecuteAsync(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteAsync(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static IDataReader ExecuteReader(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.ExecuteReader(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static async Task<IDataReader> ExecuteReaderAsync(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteReaderAsync(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static T ExecuteScalar<T>(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.ExecuteScalar<T>(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static object ExecuteScalar(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.ExecuteScalar(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static async Task<T> ExecuteScalarAsync<T>(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteScalarAsync<T>(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static async Task<object> ExecuteScalarAsync(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteScalarAsync(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static T First<T>(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.QueryFirstOrDefault<T>(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static async Task<T> FirstAsync<T>(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static IEnumerable<T> Query<T>(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.Query<T>(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDataContext context, string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.QueryAsync<T>(new CommandDefinition(commandText, parameters, transaction, context.ExecutionTimeOut, CommandType.Text));
        }
        #endregion

        #region Stored Procedure
        public static int ExecuteProcedure(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.Execute(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static async Task<int> ExecuteProcedureAsync(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteAsync(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static IDataReader ExecuteProcedureReader(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.ExecuteReader(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static async Task<IDataReader> ExecuteReaderProcedureAsync(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteReaderAsync(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static T ExecuteScalarProcedure<T>(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.ExecuteScalar<T>(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static object ExecuteScalarProcedure(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.ExecuteScalar(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static async Task<T> ExecuteScalarProcedureAsync<T>(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteScalarAsync<T>(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static async Task<object> ExecuteScalarProcedureAsync(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.ExecuteScalarAsync(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static T FirstProcedure<T>(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.QueryFirstOrDefault<T>(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static async Task<T> FirstProcedureAsync<T>(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static IEnumerable<T> QueryProcedure<T>(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return context.Connection.Query<T>(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }

        public static async Task<IEnumerable<T>> QueryProcedureAsync<T>(this IDataContext context, string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await context.Connection.QueryAsync<T>(new CommandDefinition(procedureName, parameters, transaction, context.ExecutionTimeOut, CommandType.StoredProcedure));
        }
        #endregion
    }
}