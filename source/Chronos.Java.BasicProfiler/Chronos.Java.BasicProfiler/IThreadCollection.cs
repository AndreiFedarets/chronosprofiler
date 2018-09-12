using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.Java.BasicProfiler.ThreadCollection))]
    public interface IThreadCollection : IUnitCollection<ThreadInfo>
    {

    }
}
