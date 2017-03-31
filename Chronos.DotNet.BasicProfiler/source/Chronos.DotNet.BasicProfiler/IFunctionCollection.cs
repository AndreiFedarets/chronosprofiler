using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.DotNet.BasicProfiler.FunctionCollection))]
    public interface IFunctionCollection : IUnitCollection<FunctionInfo>
    {

    }
}
