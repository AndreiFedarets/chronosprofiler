using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.DotNet.BasicProfiler.ThreadCollection))]
    public interface IThreadCollection : IUnitCollection<ThreadInfo>
    {

    }
}
