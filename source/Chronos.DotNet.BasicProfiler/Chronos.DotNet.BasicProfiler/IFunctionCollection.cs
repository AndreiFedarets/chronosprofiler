using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.FunctionCollection))]
    public interface IFunctionCollection : IUnitCollection<FunctionInfo>
    {

    }
}
