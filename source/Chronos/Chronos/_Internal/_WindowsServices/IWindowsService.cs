using System.Collections.Specialized;
using System.Diagnostics;

namespace Chronos
{
    public interface IWindowsService
    {
        bool IsRunning { get; }

        string ServiceName { get; }

        string DisplayName { get; }

        Process GetServiceProcess();

        void Stop();

        void Start();

        void SetEnvironmentVariables(StringDictionary variables);

        void RemoveEnvironmentVariables();
    }
}
