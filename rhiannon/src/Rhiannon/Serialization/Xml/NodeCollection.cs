using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Rhiannon.Serialization.Xml
{
	internal class NodeCollection : INodeCollection
	{
		private readonly XmlNode _node;
		private readonly XmlSerializationDocument _document;
		private IList<INode> _nodes;

		public NodeCollection(XmlNode node, XmlSerializationDocument document)
		{
			_node = node;
			_document = document;
			Initialize();
		}

		private XmlNode Node
		{
			get
			{
				if (_node is XmlDocument)
				{
					return (_node as XmlDocument).FirstChild;
				}
				return _node;
			}
		}

		private XmlDocument OwnerDocument
		{
			get
			{
				if (_node is XmlDocument)
				{
					return (XmlDocument)_node;
				}
				return _node.OwnerDocument;
			}
		}

		private XmlNodeList ChildNodes
		{
			get { return Node.ChildNodes; }
		}

		public INode FindFirst(string nodeName)
		{
			return _nodes.FirstOrDefault(x => string.Equals(x.Name, nodeName, StringComparison.InvariantCultureIgnoreCase));
		}

		public INode Add(string nodeName)
		{
			XmlNode nativeNode = OwnerDocument.CreateElement(nodeName);
			Node.AppendChild(nativeNode);
			INode node = new Node(nativeNode, Node, _document);
			_nodes.Add(node);
			return node;
		}

		private void Initialize()
		{
			if (_nodes == null)
			{
				_nodes = ChildNodes.Cast<XmlNode>().Where(x => x.NodeType == XmlNodeType.Element).Select(x => new Node(x, Node, _document)).Cast<INode>().ToList();
			}
		}

		public IEnumerator<INode> GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
