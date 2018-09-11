using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Chronos.Registry
{
    public sealed class RegistryRoot
    {
        private const string RegistryElementName = "registry";
        private const string KeyElementName = "key";
        private const string ValueElementName = "value";

        private const string KeyRegistryViewAttributeName = "view";
        private const string KeyNameAttributeName = "name";
        private const string KeyRemoveTypeAttributeName = "remove";
        private const string ValueNameAttributeName = "name";
        private const string ValueValueAttributeName = "value";
        private const string ValueTypeAttributeName = "type";

        private readonly List<RegistryKey> _keys;

        internal RegistryRoot(List<RegistryKey> keys)
        {
            _keys = keys;
            SetParent();
        }

        public IEnumerable<RegistryKey> Keys
        {
            get { return _keys; }
        }

        public void Import(VariableCollection variables)
        {
            try
            {
                foreach (RegistryKey key in _keys)
                {
                    key.Import(variables);
                }
            }
            catch (Exception)
            {
                Remove();
                throw;
            }
        }

        public void Remove()
        {
            foreach (RegistryKey key in _keys)
            {
                key.Remove();
            }
        }

        private void SetParent()
        {
            foreach (RegistryKey key in _keys)
            {
                key.SetParent(null);
            }
        }

        public static RegistryRoot Parse(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    RegistryRoot registry = ReadRegistry(reader);
                    return registry;
                }
            }
        }

        private static RegistryRoot ReadRegistry(XmlReader reader)
        {
            List<RegistryKey> keys = new List<RegistryKey>();
            reader.MoveToElement();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(RegistryElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case KeyElementName:
                        RegistryKey registryKey = ReadRegistryKey(reader);
                        keys.Add(registryKey);
                        break;
                }
            }
            RegistryRoot registry = new RegistryRoot(keys);
            return registry;
        }

        private static RegistryKey ReadRegistryKey(XmlReader reader)
        {
            Microsoft.Win32.RegistryView registryView = Microsoft.Win32.RegistryView.Default;
            RemoveType removeType = RemoveType.No;
            string name = string.Empty;
            List<RegistryKey> keys = new List<RegistryKey>();
            List<RegistryValue> values = new List<RegistryValue>();

            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case KeyNameAttributeName:
                        name = reader.ReadContentAsString();
                        break;
                    case KeyRegistryViewAttributeName:
                        registryView = reader.ReadContentAsEnum<Microsoft.Win32.RegistryView>();
                        break;
                    case KeyRemoveTypeAttributeName:
                        removeType = reader.ReadContentAsEnum<RemoveType>();
                        break;
                }
            }

            reader.MoveToElement();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(KeyElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case KeyElementName:
                        RegistryKey registryKey = ReadRegistryKey(reader);
                        keys.Add(registryKey);
                        break;
                    case ValueElementName:
                        RegistryValue registryValue = ReadRegistryValue(reader);
                        values.Add(registryValue);
                        break;
                }
            }
            RegistryKey registry = new RegistryKey(name, removeType, keys, values, registryView);
            return registry;
        }

        private static RegistryValue ReadRegistryValue(XmlReader reader)
        {
            string name = string.Empty;
            Microsoft.Win32.RegistryValueKind type = Microsoft.Win32.RegistryValueKind.String;
            string value = string.Empty;

            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case ValueNameAttributeName:
                        name = reader.ReadContentAsString();
                        break;
                    case ValueTypeAttributeName:
                        type = reader.ReadContentAsEnum<Microsoft.Win32.RegistryValueKind>();
                        break;
                    case ValueValueAttributeName:
                        value = reader.ReadContentAsString();
                        break;
                }
            }
            reader.MoveToElement();
            return new RegistryValue(name, type, value);
        }

    }
}
