using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public interface IClassCollection : IUnitCollection<ClassInfo>
    {
        ClassInfo FindByTypeToken(ulong moduleId, uint typeToken);
    }
}
