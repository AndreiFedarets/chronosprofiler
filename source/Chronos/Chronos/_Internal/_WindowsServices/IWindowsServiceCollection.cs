using System.Collections.Generic;
using System.Diagnostics;

namespace Chronos
{
    public interface IWindowsServiceCollection : IEnumerable<IWindowsService>
    {
        IWindowsService this[string serviceName] { get; }

        bool IsServiceHostProcess(Process process, string serviceName);
    }
}
