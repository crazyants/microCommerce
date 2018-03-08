using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using microCommerce.Domain;
using MySql.Data.MySqlClient;

namespace microCommerce.Dapper.Providers.MySql
{
    public class MySqlProvider : IProvider
    {
        #region Constant

        private const string INSERT_QUERY = "INSERT INTO `{0}` ({1}) VALUES(@{2}) SELECT LAST_INSERT_ID()";
        private const string INSERT_BULK_QUERY = "INSERT INTO `{0}` ({1}) VALUES ({2})\r\n";
        private const string UPDATE_QUERY = "UPDATE `{0}` SET {1} WHERE `Id` = @Id";
        private const string UPDATE_BULK_QUERY = "UPDATE `{0}` SET {1} WHERE `Id` = @Id\r\n";
        private const string DELETE_QUERY = "DELETE FROM `{0}` WHERE `Id` = @Id";
        private const string DELETE_BULK_QUERY = "DELETE FROM `{0}` WHERE `Id` IN(@Ids)";
        private const string SELECT_FIRST_QUERY = "SELECT\r\n{1} FROM `{0}` WHERE `Id` = @Id LIMIT 1";
        private const string EXISTING_QUERY = "SELECT CASE WHEN EXISTS (SELECT Id FROM `{0}` WHERE `Id` = @Id) THEN 1 ELSE 0 END";
        private const string COUNT_QUERY = "SELECT COUNT(`Id`) FROM `{0}}`";

        #endregion

        #region Methods

        public virtual IDbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public virtual string InsertQuery(string tableName, object entity, IEnumerable<string> columns)
        {
            IEnumerable<string> formattedColumns = columns.Select(p => string.Format("`{0}`", p));
            return string.Format(INSERT_QUERY,
                                 tableName,
                                 string.Join(", ", formattedColumns),
                                 string.Join(", @", columns));
        }

        public virtual string InsertBulkQuery(string tableName, IEnumerable<object> entities, IEnumerable<string> columns)
        {
            if (!entities.Any())
                throw new ArgumentException("collection is empty");

            IList<string> values = new List<string>();
            StringBuilder builder = new StringBuilder();
            string formattedColumns = string.Join(", ", columns.Select(p => string.Format("`{0}`", p)));
            for (int i = 0; i < entities.Count(); i++)
            {
                if (i != 0 && i % 100 == 0)
                    builder.Append("GO\r\n");

                string formattedValueColumns = string.Join(", ", columns.Select(p => string.Format("@{0}{1}", p, i + 1)));
                builder.AppendFormat(INSERT_BULK_QUERY,
                                 tableName,
                                 formattedColumns,
                                 formattedValueColumns);
            }

            return builder.ToString();
        }

        public virtual string UpdateQuery(string tableName, object entity, IEnumerable<string> columns)
        {
            string formattedColumns = string.Join(", ", columns.Select(p => string.Format("`{0}` = @{0}", p)));

            return string.Format(UPDATE_QUERY,
                                 tableName,
                                 formattedColumns);
        }

        public virtual string UpdateBulkQuery(string tableName, IEnumerable<object> entities, IEnumerable<string> columns)
        {
            if (!entities.Any())
                throw new ArgumentException("collection is empty");

            IList<string> values = new List<string>();
            object[] entityArray = entities.ToArray();

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < entityArray.Length; i++)
            {
                if (i != 0 && i % 100 == 0)
                    builder.Append("GO\r\n");

                string formattedColumns = string.Join(", ", columns.Select(p => string.Format("`{0}` = @{0}{1}", p, i + 1)));
                builder.AppendFormat(UPDATE_BULK_QUERY,
                                 tableName,
                                 formattedColumns);
            }

            return builder.ToString();
        }

        public virtual string DeleteQuery(string tableName)
        {
            return string.Format(DELETE_QUERY,
                                 tableName);
        }

        public virtual string DeleteBulkQuery(string tableName)
        {
            return string.Format(DELETE_BULK_QUERY,
                                 tableName);
        }

        public virtual string SelectFirstQuery<T>(string tableName, IEnumerable<string> columns) where T : BaseEntity
        {
            string formattedColumns = string.Join(",\r\n", columns.Select(p => string.Format("`{0}`", p)));

            return string.Format(SELECT_FIRST_QUERY,
                tableName,
                formattedColumns);
        }

        public virtual string ExistingQuery(string tableName)
        {
            string query = string.Format(EXISTING_QUERY,
                            tableName);

            return query;
        }

        public virtual string CountQuery(string tableName)
        {
            string query = string.Format(COUNT_QUERY,
                            tableName);

            return query;
        }
        #endregion
    }
}