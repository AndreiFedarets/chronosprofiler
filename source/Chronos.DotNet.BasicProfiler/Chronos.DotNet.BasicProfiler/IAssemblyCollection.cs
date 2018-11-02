using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.AssemblyCollection))]
    public interface IAssemblyCollection : IUnitCollection<AssemblyInfo>
    {

    }
}
