using System;
using System.Reflection;
using System.Data.SQLite;

namespace Chronos.Storage
{
    internal sealed class DataTableColumn
    {
        private readonly PropertyInfo _property;
        private readonly string _columnName;
        private readonly string _typeName;
        private readonly bool _primaryKey;
        private string _parameterName;

        internal DataTableColumn(DataTableColumnAttribute attribute, PropertyInfo property)
        {
            _property = property;
            _primaryKey = attribute.PrimaryKey;
            _columnName = attribute.ColumnName;
            if (string.IsNullOrWhiteSpace(_columnName))
            {
                _columnName = property.Name;
            }
            _typeName = attribute.DataType;
            if (string.IsNullOrWhiteSpace(_typeName))
            {
                _typeName = ConvertTypeName(property.PropertyType);
            }
        }

        public bool PrimaryKey
        {
            get { return _primaryKey; }
        }

        public string ColumnName
        {
            get { return _columnName; }
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        internal string ParameterName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_parameterName))
                {
                    _parameterName = "@" + ColumnName;
                }
                return _parameterName;
            }
        }

        internal string CreateColumnQuery
        {
            get
            {
                if (PrimaryKey)
                {
                    return string.Format("{0} {1} PRIMARY KEY", _columnName, _typeName);
                }
                return string.Format("{0} {1}", _columnName, _typeName);
            }
        }

        internal object GetPropertyValue(object item)
        {
            return _property.GetValue(item, null);
        }

        internal static string ConvertTypeName(Type dataType)
        {
            //string ----------
            if (dataType == typeof(string))
            {
                return "TEXT";
            }
            //number ----------
            if (dataType == typeof(byte) ||
                dataType == typeof(short) || dataType == typeof(ushort) ||
                dataType == typeof(int) || dataType == typeof(uint) ||
                dataType == typeof(long) || dataType == typeof(ulong))
            {
                return "INT";
            }
            //enum ------------
            if (dataType.IsEnum)
            {
                return "INT";
            }
            throw new NotSupportedException();
        }

        internal void SetValue(object item, SQLiteDataReader reader)
        {
            object value = reader[ColumnName];
            if (value != null && value.GetType() != _property.PropertyType)
            {
                value = Convert.ChangeType(value, _property.PropertyType);
            }
            _property.SetValue(item, value, null);
        }
    }
}
