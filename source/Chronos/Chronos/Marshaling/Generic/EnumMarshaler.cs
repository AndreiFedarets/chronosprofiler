using System;
using System.IO;

namespace Chronos.Marshaling
{
    public class EnumMarshaler : ITypeMarshaler
    {
        private readonly ITypeMarshaler _underlyingMarshaler;

        internal EnumMarshaler(Type type, ITypeMarshaler underlyingMarshaler)
        {
            ManagedType = type;
            _underlyingMarshaler = underlyingMarshaler;
        }

        public Type ManagedType { get; private set; }

        public void MarshalObject(object value, Stream stream)
        {
            object underlyingValue = Convert.ChangeType(value, _underlyingMarshaler.ManagedType);
            _underlyingMarshaler.MarshalObject(underlyingValue, stream);
        }

        public object DemarshalObject(Stream stream)
        {
            object value = _underlyingMarshaler.DemarshalObject(stream);
            return value;
        }
    }
}