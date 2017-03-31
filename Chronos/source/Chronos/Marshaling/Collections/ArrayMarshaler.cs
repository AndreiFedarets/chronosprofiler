using System;
using System.IO;

namespace Chronos.Marshaling
{
    public class ArrayMarshaler : ITypeMarshaler
    {
        private readonly ITypeMarshaler _elementMarshaler;

        public ArrayMarshaler(Type type, ITypeMarshaler elementMarshaler)
        {
            ManagedType = type;
            _elementMarshaler = elementMarshaler;
        }

        public Type ManagedType { get; private set; }

        public void MarshalObject(object value, Stream stream)
        {
            Array array = (Array) value;
            Int32Marshaler.Marshal(array.Length, stream);
            foreach (object element in array)
            {
                _elementMarshaler.MarshalObject(element, stream);
            }
        }

        public object DemarshalObject(Stream stream)
        {
            int count = Int32Marshaler.Demarshal(stream);
            Type elementType = _elementMarshaler.ManagedType;
            Array array = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
            {
                object element = _elementMarshaler.DemarshalObject(stream);
                array.SetValue(element, i);
            }
            return array;
        }
    }
}
