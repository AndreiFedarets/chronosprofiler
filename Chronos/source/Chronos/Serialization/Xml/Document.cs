using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Chronos.Serialization.Xml
{
    public class Document : IDocument
    {
        private AttributeCollection _attributes;
        private XmlDocument _nativeDocument;
        private INodeCollection _children;
        private INode _documentNode;
        private string _fullName;

        public Document()
        {
            _nativeDocument = new XmlDocument();
        }

        public INode DocumentNode
        {
            get { return _documentNode; }
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

        public string LocalName
        {
            get { return _nativeDocument.LocalName; }
        }

        public string Namespace
        {
            get { return _nativeDocument.NamespaceURI; }
        }

        public string InnerXml
        {
            get { return _nativeDocument.InnerXml; }
            set
            {
                _nativeDocument.InnerXml = value;
                Initialize();
            }
        }

        public void Save(string fileFullName)
        {
            _nativeDocument.Save(fileFullName);
        }

        public void Load(string fileFullName)
        {
            FileInfo fileInfo = new FileInfo(fileFullName);
            using (FileStream stream = fileInfo.OpenRead())
            {
                _nativeDocument.Load(stream);
            }
            Initialize();
        }

        public void LoadXml(string xml)
        {
            _nativeDocument.LoadXml(xml);
            Initialize();
        }

        private void Initialize()
        {
            _attributes = new AttributeCollection(_nativeDocument);
            _children = new NodeCollection(_nativeDocument, this);
            _documentNode = new Node(_nativeDocument.DocumentElement, _nativeDocument, this);
        }

        public static Document Open(string fileFullName)
        {
            Document document = new Document();
            document.Load(fileFullName);
            return document;
        }

        public static Document Create(string fileFullName, string nodeName)
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
            Document document = new Document();
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