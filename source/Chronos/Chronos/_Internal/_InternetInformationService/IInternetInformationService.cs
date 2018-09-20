using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos
{
    public interface IInternetInformationService
    {
        bool IsAvailable { get; }

        bool HasPermissions { get; }

        List<string> GetApplicationPools();

        void SetEnvironmentVariables(StringDictionary variables);

        void RemoveEnvironmentVariables();
    }
}
