using System.Collections.Generic;
using System.Linq;

namespace Chronos.Storage
{
    internal static class QueryBuilder
    {
        public static string CheckTableQuery(string tableName)
        {
            string query = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", tableName);
            return query;
        }

        public static string CreateTableQuery(string tableName, DataTableColumn[] columns)
        {
            IEnumerable<string> parts = columns.Select(column => column.CreateColumnQuery);
            string tableColumns = string.Join(", ", parts);
            string query = string.Format("CREATE TABLE IF NOT EXISTS {0} ({1})", tableName, tableColumns);
            return query;
        }

        public static string AddOrUpdateRecordQuery(string tableName, DataTableColumn[] columns)
        {
            List<string> parts = columns.Select(column => column.ParameterName).ToList();
            string tableColumns = string.Join(", ", parts);
            string query = string.Format("INSERT INTO {0} VALUES({1})", tableName, tableColumns);
            return query;
        }

        public static string SelectAllQuery(string tableName)
        {
            string query = string.Format("SELECT * FROM {0}", tableName);
            return query;
        }

        public static string SelectQuery(string tableName, string columnName, IEnumerable<object> ids)
        {
            string array = string.Join(",", ids.Select(x => x.ToString()));
            string query = string.Format("SELECT * FROM {0} WHERE {1} IN ({2})", tableName, columnName, array);
            return query;
        }
    }
}
