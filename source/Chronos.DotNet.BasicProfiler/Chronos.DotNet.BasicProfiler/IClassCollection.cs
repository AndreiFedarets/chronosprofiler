using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.DotNet.BasicProfiler.ClassCollection))]
    public interface IClassCollection : IUnitCollection<ClassInfo>
    {
        ClassInfo FindByTypeToken(ulong moduleId, uint typeToken);
    }
}
