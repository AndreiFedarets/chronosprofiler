using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.DotNet.BasicProfiler.AssemblyCollection))]
    public interface IAssemblyCollection : IUnitCollection<AssemblyInfo>
    {

    }
}
