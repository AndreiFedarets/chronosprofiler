using System;
using System.Collections;
using System.IO;

namespace Chronos.Marshaling
{
    public class ListMarshaler : ITypeMarshaler
    {
        private readonly ITypeMarshaler _elementMarshaler;

        public ListMarshaler(Type type, ITypeMarshaler elementMarshaler)
        {
            ManagedType = type;
            _elementMarshaler = elementMarshaler;
        }

        public Type ManagedType { get; private set; }

        public void MarshalObject(object value, Stream stream)
        {
            IList list = (IList) value;
            Int32Marshaler.Marshal(list.Count, stream);
            foreach (object element in list)
            {
                MarshalingManager.Marshal(element, stream);
            }
        }

        public object DemarshalObject(Stream stream)
        {
            int count = Int32Marshaler.Demarshal(stream);
            IList list = (IList)Activator.CreateInstance(ManagedType);
            for (int i = 0; i < count; i++)
            {
                object element = _elementMarshaler.DemarshalObject(stream);
                list.Add(element);
            }
            return list;
        }
    }
}
