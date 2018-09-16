using System.Collections.Generic;
using System.Diagnostics;

namespace Chronos.Accessibility.WS
{
    public interface IServiceControllerCollection : IEnumerable<IServiceController>
    {
        IServiceController this[string serviceName] { get; }

        bool IsServiceHostProcess(Process process, string serviceName);
    }
}
