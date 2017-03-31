using System;
using System.IO;
using System.Xml;

namespace Rhiannon.Serialization.Xml
{
	public class XmlSerializationDocument : IDocument
	{
		private AttributeCollection _attributes;
		private XmlDocument _nativeDocument;
		private INodeCollection _children;

		public void Load(string fileFullName)
		{
			FileInfo fileInfo = new FileInfo(fileFullName);
			_nativeDocument = new XmlDocument();
			using (FileStream stream = fileInfo.OpenRead())
			{
				_nativeDocument.Load(stream);
			}
			_attributes = new AttributeCollection(_nativeDocument);
			_children = new NodeCollection(_nativeDocument, this);
		}

		public void LoadXml(string xml)
		{
			_nativeDocument = new XmlDocument();
			_nativeDocument.LoadXml(xml);
			_attributes = new AttributeCollection(_nativeDocument);
			_children = new NodeCollection(_nativeDocument, this);
		}

		public INodeCollection Children
		{
			get { return _children; }
		}

		public IAttributeCollection Attributes
		{
			get { return _attributes; }
		}

		public IDocument Owner
		{
			get { return this; }
		}

		public string Name
		{
			get { return _nativeDocument.Name; }
		}

		public void Save(string fileFullName)
		{
			_nativeDocument.Save(fileFullName);
		}

		public static XmlSerializationDocument Open(string fileFullName)
		{
			XmlSerializationDocument document = new XmlSerializationDocument();
			document.Load(fileFullName);
			return document;
		}

		public static XmlSerializationDocument Create(string fileFullName, string nodeName)
		{
			XmlDocument nativeDocument = new XmlDocument();
			string path = Path.GetDirectoryName(fileFullName);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			using (FileStream stream = File.Create(fileFullName))
			{
				XmlElement node = nativeDocument.CreateElement(nodeName);
				nativeDocument.AppendChild(node);
				nativeDocument.Save(stream);
			}
			XmlSerializationDocument document = new XmlSerializationDocument();
			document.Load(fileFullName);
			return document;
		}

		public void Delete()
		{
			throw new NotSupportedException();
		}

		public string Value
		{
			get { return _nativeDocument.Value; }
			set { _nativeDocument.Value = value; }
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
