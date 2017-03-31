using System;
using System.Xml;

namespace Rhiannon.Serialization.Xml
{
	internal class Node : INode
	{
		private readonly XmlSerializationDocument _document;
		private readonly AttributeCollection _attributes;
		private readonly XmlNode _nativeNode;
		private readonly INodeCollection _children;
		protected XmlNode Parent;

		public Node(XmlNode node, XmlNode parent, XmlSerializationDocument document)
		{
			_nativeNode = node;
			_attributes = new AttributeCollection(node);
			_children = new NodeCollection(node, document);
			Parent = parent;
			_document = document;
		}

		public IDocument Owner
		{
			get { return _document; }
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

		public virtual void Delete()
		{
			Parent.RemoveChild(_nativeNode);
		}

		public T GetValueAs<T>()
		{
			if (typeof(T).IsEnum)
			{
				return (T)Enum.Parse(typeof(T), Value);
			}
			return (T)Convert.ChangeType(Value, typeof(T));
		}

		public void SetValueAs<T>(T value)
		{
			string serializedValue;
			if (typeof(T) == typeof(Guid))
			{
				serializedValue = value.ToString();
			}
			else
			{
				serializedValue = (string)Convert.ChangeType(value, typeof(string));
			}
			Value = serializedValue;
		}
	}
}
