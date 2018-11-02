using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.ThreadCollection))]
    public interface IThreadCollection : IUnitCollection<ThreadInfo>
    {

    }
}
