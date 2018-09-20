using System.Collections.Generic;
using System.Diagnostics;

namespace Chronos
{
    public interface IWindowsServiceCollection : IEnumerable<IWindowsService>
    {
        bool HasPermissions { get; }

        IWindowsService this[string serviceName] { get; }

        bool IsServiceHostProcess(Process process, string serviceName);
    }
}
