using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.Java.BasicProfiler.ClassCollection))]
    public interface IClassCollection : IUnitCollection<ClassInfo>
    {
        ClassInfo FindByTypeToken(ulong moduleId, uint typeToken);
    }
}
