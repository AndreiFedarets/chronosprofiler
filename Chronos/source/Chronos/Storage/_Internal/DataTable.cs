using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace Chronos.Storage
{
    internal sealed class DataTable<T> : IDataTable<T>
    {
        private readonly object _lock;
        private readonly SQLiteConnection _connection;
        private readonly DataTableColumn[] _columns;
        private string _tableName;

        public DataTable(SQLiteConnection connection)
        {
            _lock = new object();
            _connection = connection;
            _columns = ExtractDataColumns();
            EnsureTableExists();
        }

        public string TableName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_tableName))
                {
                    DataTableAttribute dataTableAttribute = typeof(T).GetCustomAttribute<DataTableAttribute>();
                    if (dataTableAttribute == null || string.IsNullOrWhiteSpace(dataTableAttribute.TableName))
                    {
                        _tableName = typeof(T).Name;
                    }
                    else
                    {
                        _tableName = dataTableAttribute.TableName;
                    }
                }
                return _tableName;
            }
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                SQLiteCommand command = _connection.CreateCommand();
                command.CommandText = QueryBuilder.InsertTableQuery(TableName, _columns);
                foreach (DataTableColumn column in _columns)
                {
                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.ParameterName = column.ParameterName;
                    parameter.Value = column.GetPropertyValue(item);
                    command.Parameters.Add(parameter);
                }
                command.ExecuteNonQuery();
            }
        }

        public void Add(IEnumerable<T> items)
        {
            lock (_lock)
            {
                SQLiteTransaction transaction = _connection.BeginTransaction();
                SQLiteCommand command = transaction.Connection.CreateCommand();
                foreach (T item in items)
                {
                    command.CommandText = QueryBuilder.InsertTableQuery(TableName, _columns);
                    foreach (DataTableColumn column in _columns)
                    {
                        SQLiteParameter parameter = command.CreateParameter();
                        parameter.ParameterName = column.ParameterName;
                        parameter.Value = column.GetPropertyValue(item);
                        command.Parameters.Add(parameter);
                    }
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public List<T> Select(IEnumerable<object> ids)
        {
            DataTableColumn column = _columns.First(x => x.PrimaryKey);
            string query = QueryBuilder.SelectQuery(TableName, column.ColumnName, ids);
            List<T> items = LoadItems(query);
            return items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            string query = QueryBuilder.SelectAllQuery(TableName);
            List<T> items = LoadItems(query);
            return items.GetEnumerator();
        }

        private List<T> LoadItems(string query)
        {
            lock (_lock)
            {
                List<T> items = new List<T>();
                SQLiteCommand command = _connection.CreateCommand();
                command.CommandText = query;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T item = ParseItem(reader);
                    items.Add(item);
                }
                return items;
            }
        }

        private T ParseItem(SQLiteDataReader reader)
        {
            T item = (T) Activator.CreateInstance(typeof (T));
            foreach (DataTableColumn column in _columns)
            {
                column.SetValue(item, reader);
            }
            return item;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void EnsureTableExists()
        {
            SQLiteCommand command = _connection.CreateCommand();
            command.CommandText = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", TableName);
            if (command.ExecuteScalar() == null)
            {
                command = _connection.CreateCommand();
                command.CommandText = QueryBuilder.CreateTableQuery(TableName, _columns);
                command.ExecuteScalar();
            }
        }

        private static DataTableColumn[] ExtractDataColumns()
        {
            List<DataTableColumn> columns = new List<DataTableColumn>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                DataTableColumnAttribute attribute = property.GetCustomAttribute<DataTableColumnAttribute>();
                if (attribute != null)
                {
                    DataTableColumn column = new DataTableColumn(attribute, property);
                    columns.Add(column);
                }
            }
            return columns.ToArray();
        }
    }
}
