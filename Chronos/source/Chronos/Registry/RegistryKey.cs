using System;
using System.Collections.Generic;
using System.IO;

namespace Chronos.Registry
{
    public sealed class RegistryKey
    {
        private RegistryKey _parent;
        private readonly RemoveType _removeType;
        private readonly Microsoft.Win32.RegistryView _registryView;
        private string _name;

        internal RegistryKey(string name, RemoveType removeType, List<RegistryKey> keys, List<RegistryValue> values, Microsoft.Win32.RegistryView registryView)
        {
            KeyName = name;
            _removeType = removeType;
            Keys = new RegistryKeyCollection(keys);
            Values = new RegistryValueCollection(values);
            _registryView = registryView;
        }

        public string KeyName { get; private set; }

        public RegistryKeyCollection Keys { get; private set; }

        public RegistryValueCollection Values { get; private set; }

        public string FullName
        {
            get
            {
                string fullName = _parent == null ? KeyName : Path.Combine(_parent.FullName, KeyName);
                return fullName;
            }
        }

        internal string Name
        {
            get
            {
                if (_name == null)
                {
                    if (_parent == null)
                    {
                        _name = string.Empty;
                    }
                    else
                    {
                        _name = Path.Combine(_parent.Name, KeyName);
                    }
                }
                return _name;
            }
        }

        internal Microsoft.Win32.RegistryHive RegistryHive
        {
            get
            {
                Microsoft.Win32.RegistryHive registryHive;
                if (_parent == null)
                {
                    registryHive = RegistryExtensions.GetRegistryHive(FullName);
                }
                else
                {
                    registryHive = _parent.RegistryHive;
                }
                return registryHive;
            }
        }

        internal Microsoft.Win32.RegistryView RegistryView
        {
            get
            {
                Microsoft.Win32.RegistryView registryView;
                if (_parent != null && _parent.RegistryView != Microsoft.Win32.RegistryView.Default)
                {
                    registryView = _parent.RegistryView;
                }
                else
                {
                    registryView = _registryView;
                }
                return registryView;
            }
        }

        internal void SetParent(RegistryKey parent)
        {
            _parent = parent;
            Keys.SetParent(this);
            Values.SetParent(this);
        }

        internal void Import(VariableCollection variables)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive, RegistryView);
                Microsoft.Win32.RegistryKey key = rootKey.OpenSubKey(Name);
                if (key == null)
                {
                    rootKey.CreateSubKey(Name);
                }
            }
            Keys.Import(variables);
            Values.Import(variables);
        }

        internal void Remove()
        {
            if (_removeType == RemoveType.Force)
            {
                if (!string.IsNullOrEmpty(Name) && _parent != null)
                {
                    Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive, RegistryView);
                    Microsoft.Win32.RegistryKey key = rootKey.OpenSubKey(_parent.Name, true);
                    if (key != null)
                    {
                        try
                        {
                            key.DeleteSubKeyTree(KeyName);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            Keys.Remove();
            Values.Remove();
        }
    }
}
