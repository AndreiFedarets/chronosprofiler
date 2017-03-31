using System.IO;
using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler.Marshaling
{
    internal class AppDomainInfoMarshaler : NativeUnitMarshaler<AppDomainNativeInfo>
    {
        protected override void MarshalInternal(AppDomainNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override AppDomainNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(AppDomainNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<AppDomainNativeInfo>.Marshal(value, stream);
        }

        public new static AppDomainNativeInfo Demarshal(Stream stream)
        {
            AppDomainNativeInfo unit = NativeUnitMarshaler<AppDomainNativeInfo>.Demarshal(stream);
            return unit;
        }
    }
}
