using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.Java.BasicProfiler.ModuleCollection))]
    public interface IModuleCollection : IUnitCollection<ModuleInfo>
    {

    }
}
