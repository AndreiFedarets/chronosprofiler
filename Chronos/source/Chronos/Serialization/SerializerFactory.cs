using System;
using System.Collections.Generic;
using Chronos.Serialization.Binary;
using Chronos.Serialization.Xml;

namespace Chronos.Serialization
{
    public class SerializerFactory : ISerializerFactory
    {
        private readonly IDictionary<Type, ISerializer> _xmlSerializers;
        private readonly ISerializer _binarySerializer;

        public SerializerFactory()
        {
            _xmlSerializers = new Dictionary<Type, ISerializer>();
            _binarySerializer = new BinarySerializer();
        }

        public ISerializer Create<T>(SerializerType serializerType)
        {
            if (serializerType == SerializerType.Xml)
            {
                return CreateXml<T>();
            }
            return CreateBinary();
        }

        public ISerializer CreateXml<T>()
        {
            Type targetType = typeof (T);
            ISerializer serializer;
            if (!_xmlSerializers.TryGetValue(targetType, out serializer))
            {
                serializer = new XmlSerializer(targetType);
                _xmlSerializers.Add(targetType, serializer);
            }
            return serializer;
        }

        public ISerializer CreateBinary()
        {
            return _binarySerializer;
        }
    }
}
