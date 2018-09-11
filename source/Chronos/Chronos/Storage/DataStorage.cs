using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace Chronos.Storage
{
    public sealed class DataStorage : IDataStorage, IDisposable
    {
        private readonly string _fileFullName;
        private SQLiteConnection _connection;
        private readonly Dictionary<Type, object> _tables;

        public DataStorage(string fileFullName)
        {
            _fileFullName = fileFullName;
            _tables = new Dictionary<Type, object>();
        }

        private SQLiteConnection Connection
        {
            get
            {
                string connectionString = string.Format("Data Source='{0}';Version=3;New=False;Compress=True;", _fileFullName);
                try
                {
                    _connection = new SQLiteConnection(connectionString);
                    _connection.Open();
                }
                catch (Exception exception)
                {
                    LoggingProvider.Current.Log(TraceEventType.Critical, exception);
                    throw;
                }
                return _connection;
            }
        }

        public IDataTable<T> OpenTable<T>()
        {
            Type key = typeof(IDataTable<T>);
            object tableObject;
            if (!_tables.TryGetValue(key, out tableObject))
            {
                tableObject = new DataTable<T>(Connection);
                _tables.Add(key, tableObject);
            }
            return (IDataTable<T>)tableObject;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
