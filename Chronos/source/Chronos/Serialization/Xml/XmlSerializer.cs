using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Chronos.Serialization.Xml
{
    internal class XmlSerializer : BaseSerializer
    {
        private readonly System.Xml.Serialization.XmlSerializer _serializer;
        private readonly XmlSerializerNamespaces _namespaces;

        public XmlSerializer(Type type)
        {
            _serializer = new System.Xml.Serialization.XmlSerializer(type);
            _namespaces = new XmlSerializerNamespaces();
            _namespaces.Add(string.Empty, string.Empty);
        }

        public override T Deserialize<T>(Stream stream)
        {
            T result = (T) _serializer.Deserialize(stream);
            return result;
        }

        public T Deserialize<T>(XmlReader reader)
        {
            T result = (T)_serializer.Deserialize(reader);
            return result;
        }

        public override void Serialize(Stream stream, object obj)
        {
            _serializer.Serialize(stream, obj, _namespaces);
        }
    }
}
