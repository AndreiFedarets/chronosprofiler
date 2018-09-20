using System.Collections.Generic;

namespace Chronos.Accessibility.IIS
{
    [PublicService(typeof(Proxy.Accessibility.IIS.InternetInformationServiceAccessor))]
    public interface IInternetInformationServiceAccessor
    {
        bool IsAvailable { get; }

        bool HasPermissions { get; }

        List<string> GetApplicationPools();
    }
}
