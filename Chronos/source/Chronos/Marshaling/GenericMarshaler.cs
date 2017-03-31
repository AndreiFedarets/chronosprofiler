using System;
using System.Diagnostics;
using System.IO;

namespace Chronos.Marshaling
{
    public abstract class GenericMarshaler<T> : ITypeMarshaler
    {
        public Type ManagedType
        {
            get { return typeof(T); }
        }

        public void MarshalObject(object value, Stream stream)
        {
            Debug.Assert(value is T, string.Format("{0}: value is not {1}", GetType(), ManagedType));
            MarshalInternal((T) value, stream);
        }

        public object DemarshalObject(Stream stream)
        {
            return DemarshalInternal(stream);
        }

        protected abstract void MarshalInternal(T value, Stream stream);

        protected abstract T DemarshalInternal(Stream stream);
    }
}
