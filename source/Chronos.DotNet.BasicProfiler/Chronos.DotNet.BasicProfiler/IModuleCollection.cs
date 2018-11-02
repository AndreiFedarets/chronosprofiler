using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.ModuleCollection))]
    public interface IModuleCollection : IUnitCollection<ModuleInfo>
    {

    }
}
