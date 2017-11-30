using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.Java.BasicProfiler.FunctionCollection))]
    public interface IFunctionCollection : IUnitCollection<FunctionInfo>
    {

    }
}
