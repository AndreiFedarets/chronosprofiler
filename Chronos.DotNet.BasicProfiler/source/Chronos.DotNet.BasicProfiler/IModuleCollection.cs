using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.DotNet.BasicProfiler.ModuleCollection))]
    public interface IModuleCollection : IUnitCollection<ModuleInfo>
    {

    }
}
