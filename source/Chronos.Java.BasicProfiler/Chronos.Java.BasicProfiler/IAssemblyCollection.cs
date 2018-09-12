using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.Java.BasicProfiler.AssemblyCollection))]
    public interface IAssemblyCollection : IUnitCollection<AssemblyInfo>
    {

    }
}
