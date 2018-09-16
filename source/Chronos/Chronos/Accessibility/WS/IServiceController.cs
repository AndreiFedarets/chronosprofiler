using System.Collections.Specialized;
using System.Diagnostics;

namespace Chronos.Accessibility.WS
{
    public interface IServiceController
    {
        string ServiceName { get; }

        string DisplayName { get; }

        Process GetServiceProcess();

        void Stop();

        void Start();

        void SetEnvironmentVariables(StringDictionary variables);

        void RemoveEnvironmentVariables();
    }
}
