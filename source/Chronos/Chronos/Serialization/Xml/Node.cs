using System;
using System.Xml;

namespace Chronos.Serialization.Xml
{
    internal sealed class Node : INode
    {
        private readonly Document _document;
        private readonly AttributeCollection _attributes;
        private readonly XmlNode _nativeNode;
        private readonly INodeCollection _children;
        private readonly XmlNode _parent;
        private string _fullName;

        public Node(XmlNode node, XmlNode parent, Document document)
        {
            _nativeNode = node;
            _attributes = new AttributeCollection(node);
            _children = new NodeCollection(node, document);
            _parent = parent;
            _document = document;
        }

        public IDocument Owner
        {
            get { return _document; }
        }

        internal XmlDocument OwnerDocument
        {
            get { return _nativeNode.OwnerDocument; }
        }

        internal XmlNode InternalNode
        {
            get { return _nativeNode; }
        }

        public IAttributeCollection Attributes
        {
            get { return _attributes; }
        }

        public string Value
        {
            get { return _nativeNode.Value; }
            set { _nativeNode.Value = value; }
        }

        public string Name
        {
            get { return _nativeNode.Name; }
        }

        public INodeCollection Children
        {
            get { return _children; }
        }

        public string LocalName
        {
            get { return _nativeNode.LocalName; }
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                {
                    _fullName = GetNodeFullName(LocalName, Namespace);
                }
                return _fullName;
            }
        }

        public string Namespace
        {
            get { return _nativeNode.NamespaceURI; }
        }

        public string InnerXml
        {
            get { return _nativeNode.InnerXml; }
            set { _nativeNode.InnerXml = value; }
        }

        public void Delete()
        {
            _parent.RemoveChild(_nativeNode);
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

        internal static string GetNodeFullName(string localName, string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
            {
                return localName;
            }
            return string.Concat(@namespace, ":", localName);
        }
    }
}
