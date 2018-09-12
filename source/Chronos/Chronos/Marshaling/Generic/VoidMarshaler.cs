using System;
using System.IO;

namespace Chronos.Marshaling
{
    public sealed class VoidMarshaler : ITypeMarshaler
    {
        public Type ManagedType
        {
            get { return typeof (void); }
        }

        public void MarshalObject(object value, Stream stream)
        {
            
        }

        public object DemarshalObject(Stream stream)
        {
            return null;
        }
    }
}
