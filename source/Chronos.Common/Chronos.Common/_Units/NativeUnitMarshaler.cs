using System.IO;
using Chronos.Marshaling;

namespace Chronos.Common
{
    public abstract class NativeUnitMarshaler<T> : GenericMarshaler<T> where T : NativeUnitBase, new()
    {
        protected override void MarshalInternal(T value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override T DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static void Marshal(T value, Stream stream)
        {
            UInt64Marshaler.Marshal(value.Uid, stream);
            UInt64Marshaler.Marshal(value.Id, stream);
            UInt64Marshaler.Marshal(value.BeginLifetime, stream);
            UInt64Marshaler.Marshal(value.EndLifetime, stream);
            StringMarshaler.Marshal(value.Name, stream);
        }

        public static T Demarshal(Stream stream)
        {
            T value = new T();
            value.Uid = UInt32Marshaler.Demarshal(stream);
            value.Id = UInt64Marshaler.Demarshal(stream);
            value.BeginLifetime = UInt32Marshaler.Demarshal(stream);
            value.EndLifetime = UInt32Marshaler.Demarshal(stream);
            value.Name = StringMarshaler.Demarshal(stream);
            return value;
        }
    }
}
