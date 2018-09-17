using System.Collections.Generic;

namespace Chronos
{
    public interface IInternetInformationService
    {
        bool IsAvailable { get; }

        List<string> GetApplicationPools();
    }
}
