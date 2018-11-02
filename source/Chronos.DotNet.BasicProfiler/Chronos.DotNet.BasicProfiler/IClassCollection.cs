using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.ClassCollection))]
    public interface IClassCollection : IUnitCollection<ClassInfo>
    {
        ClassInfo FindByTypeToken(ulong moduleId, uint typeToken);
    }
}
