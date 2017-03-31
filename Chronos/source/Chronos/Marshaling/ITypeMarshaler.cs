using System;
using System.IO;

namespace Chronos.Marshaling
{
    public interface ITypeMarshaler
    {
        Type ManagedType { get; }

        void MarshalObject(object value, Stream stream);

        object DemarshalObject(Stream stream);
    }
}
