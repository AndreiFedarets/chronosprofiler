using System;
using System.IO;
using Microsoft.Win32;

namespace Chronos
{
    public static class RegistryExtensions
    {
        private const string LocalMachine = "HKEY_LOCAL_MACHINE";
        private const string CurrentUser = "HKEY_CURRENT_USER";
        private const string ClassesRoot = "HKEY_CLASSES_ROOT";

        public static RegistryHive GetRegistryHive(string fullName)
        {
            int index = fullName.IndexOf('\\');
            string root = index >= 0 ? fullName.Substring(0, index) : fullName;
            switch (root.ToUpperInvariant())
            {
                case LocalMachine:
                    return RegistryHive.LocalMachine;
                case CurrentUser:
                    return RegistryHive.CurrentUser;
                case ClassesRoot:
                    return RegistryHive.ClassesRoot;
            }
            throw new ArgumentException();
        }

        public static string GetRegistryName(string fullName)
        {
            int index = fullName.IndexOf('\\');
            string name = index >= 0 ? fullName.Substring(fullName.IndexOf('\\') + 1) : string.Empty;
            return name;
        }

        public static RegistryKey OpenSubKey(string fullName, RegistryView view)
        {
            RegistryHive hive = GetRegistryHive(fullName);
            string path = GetRegistryName(fullName);
            RegistryKey key = RegistryKey.OpenBaseKey(hive, view);
            if (!string.IsNullOrEmpty(path))
            {
                key = key.OpenSubKey(path);
            }
            return key;
        }

        public static RegistryKey CreateSubKey(string fullName, RegistryView view)
        {
            RegistryHive hive = GetRegistryHive(fullName);
            string path = GetRegistryName(fullName);
            RegistryKey key = RegistryKey.OpenBaseKey(hive, view);
            key = key.CreateSubKey(path);
            return key;
        }
    }
}
