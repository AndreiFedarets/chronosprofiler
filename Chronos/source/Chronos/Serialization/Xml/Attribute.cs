using System;
using System.Xml;

namespace Chronos.Serialization.Xml
{
    internal class Attribute : IAttribute
    {
        private readonly XmlAttribute _attribute;
        private string _fullName;

        public Attribute(XmlAttribute attribute)
        {
            _attribute = attribute;
        }

        public string Value
        {
            get { return _attribute.Value; }
            set { _attribute.Value = value; }
        }

        public string Name
        {
            get { return _attribute.Name; }
        }

        public string LocalName
        {
            get { return _attribute.LocalName; }
        }

        public string Namespace
        {
            get { return _attribute.NamespaceURI; }
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                {
                    _fullName = Node.GetNodeFullName(LocalName, Namespace);
                }
                return _fullName;
            }
        }

        public T GetValueAs<T>()
        {
            if (typeof (T).IsEnum)
            {
                return (T) Enum.Parse(typeof (T), Value);
            }
            return (T) Convert.ChangeType(Value, typeof (T));
        }

        public void SetValueAs<T>(T value)
        {
            string serializedValue;
            if (typeof (T) == typeof (Guid))
            {
                serializedValue = value.ToString();
            }
            else
            {
                serializedValue = (string) Convert.ChangeType(value, typeof (string));
            }
            Value = serializedValue;
        }
    }
}
