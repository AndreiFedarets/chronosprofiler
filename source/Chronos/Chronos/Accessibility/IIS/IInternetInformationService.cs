using System.Collections.Generic;

namespace Chronos.Accessibility.IIS
{
    [PublicService(typeof(Proxy.Accessibility.IIS.InternetInformationService))]
    public interface IInternetInformationService
    {
        bool IsAvailable { get; }

        bool IsRunning { get; }

        List<string> GetApplicationPools();
    }
}
