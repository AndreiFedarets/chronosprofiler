using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Chronos.Serialization.Xml
{
    internal class AttributeCollection : IAttributeCollection
    {
        private readonly XmlNode _node;
        private List<IAttribute> _attributes;

        public AttributeCollection(XmlNode node)
        {
            _node = node;
        }

        public IAttribute FindByName(string attributeName)
        {
            Initialize();
            IAttribute attribute = _attributes.FirstOrDefault(x => string.Equals(x.Name, attributeName, StringComparison.InvariantCulture));
            return attribute;
        }

        public IAttribute FindByFullName(string attributeFullName)
        {
            Initialize();
            IAttribute attribute = _attributes.FirstOrDefault(x => string.Equals(x.FullName, attributeFullName, StringComparison.InvariantCulture));
            return attribute;
        }

        private XmlDocument OwnerDocument
        {
            get { return _node.OwnerDocument; }
        }

        private XmlAttributeCollection Attributes
        {
            get { return _node.Attributes; }
        }

        public IAttribute Add(string name)
        {
            XmlAttribute nativeAttribute = OwnerDocument.CreateAttribute(name);
            Attributes.Append(nativeAttribute);
            IAttribute attribute = new Attribute(nativeAttribute);
            if (_attributes != null)
            {
                _attributes.Add(attribute);
            }
            return attribute;
        }

        public IAttribute Add(string name, string @namespace)
        {
            XmlAttribute nativeAttribute = OwnerDocument.CreateAttribute(name, @namespace);
            Attributes.Append(nativeAttribute);
            IAttribute attribute = new Attribute(nativeAttribute);
            if (_attributes != null)
            {
                _attributes.Add(attribute);
            }
            return attribute;
        }

        public bool Exists(string name)
        {
            IAttribute attribute = _attributes.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCulture));
            return attribute != null;
        }

        private void Initialize()
        {
            if (_attributes != null)
            {
                return;
            }
            _attributes = Attributes.Cast<XmlAttribute>().Select(x => new Attribute(x)).Cast<IAttribute>().ToList();
        }

        public IEnumerator<IAttribute> GetEnumerator()
        {
            Initialize();
            return _attributes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
