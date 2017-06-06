using System;
using System.IO;
using Microsoft.Win32;

namespace Chronos.Registry
{
    public sealed class RegistryValue
    {
        private RegistryKey _parent;

        internal RegistryValue(string name, Microsoft.Win32.RegistryValueKind type, string value)
        {
            ValueName = name;
            Type = type;
            Value = value;
        }

        public string ValueName { get; private set; }

        public Microsoft.Win32.RegistryValueKind Type { get; private set; }

        public string Value { get; private set; }

        public string FullName
        {
            get { return Path.Combine(_parent.FullName, ValueName); }
        }

        internal string Name
        {
            get { return Path.Combine(_parent.Name, ValueName); }
        }

        public object TypedValue
        {
            get
            {
                return Value;
            }
        }

        internal void SetParent(RegistryKey parent)
        {
            _parent = parent;
        }

        internal void Import(VariableCollection variables)
        {
            Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.RegistryKey.OpenBaseKey(_parent.RegistryHive, _parent.RegistryView);
            Microsoft.Win32.RegistryKey key = rootKey.OpenSubKey(_parent.Name, true);
            if (key != null)
            {
                object value = variables.ReplaceVariables(TypedValue, Type);
                key.SetValue(ValueName, value, Type);
            }
        }

        internal void Remove()
        {
            Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.RegistryKey.OpenBaseKey(_parent.RegistryHive, _parent.RegistryView);
            Microsoft.Win32.RegistryKey key = rootKey.OpenSubKey(_parent.Name, true);
            if (key != null)
            {
                key.SetValue(ValueName, string.Empty, Type);
            }
        }
    }
}
