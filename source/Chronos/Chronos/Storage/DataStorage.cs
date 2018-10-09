using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Chronos.Storage
{
    public sealed class DataStorage : IDataStorage, IDisposable
    {
        private const string ConnectionStringTemplate = "Data Source='{0}';Version=3;New=False;Compress=True;";
        private readonly SQLiteConnection _connection;
        private readonly Dictionary<Type, object> _tables;

        private DataStorage(string fileFullName)
        {
            _tables = new Dictionary<Type, object>();
            _connection = CreateConnection(fileFullName);
        }

        public IDataTable<T> OpenTable<T>()
        {
            Type key = typeof(IDataTable<T>);
            object tableObject;
            if (!_tables.TryGetValue(key, out tableObject))
            {
                tableObject = new DataTable<T>(_connection);
                _tables.Add(key, tableObject);
            }
            return (IDataTable<T>)tableObject;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        private static SQLiteConnection CreateConnection(string fileFullName)
        {
            try
            {
                string connectionString = string.Format(ConnectionStringTemplate, fileFullName);
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Critical, exception);
                throw;
            }
        }

        internal static IDataStorage CreateNew(string directoryFullName, Guid sessionUid)
        {
            if (!Directory.Exists(directoryFullName))
            {
                Directory.CreateDirectory(directoryFullName);
            }
            string fileFullName = Path.Combine(directoryFullName, sessionUid.ToString("N"));
            fileFullName = Path.ChangeExtension(fileFullName, ".sqlite");
            if (File.Exists(fileFullName))
            {
                File.Delete(fileFullName);
            }
            SQLiteConnection.CreateFile(fileFullName);
            IDataStorage dataStorage = new DataStorage(fileFullName);
            return dataStorage;
        }
    }
}
