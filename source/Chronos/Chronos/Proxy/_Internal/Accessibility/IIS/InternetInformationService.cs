using System.Collections.Generic;
using Chronos.Accessibility.IIS;

namespace Chronos.Proxy.Accessibility.IIS
{
    internal sealed class InternetInformationService : ProxyBaseObject<IInternetInformationService>, IInternetInformationService
    {
        public InternetInformationService(IInternetInformationService remoteObject)
            : base(remoteObject)
        {
        }

        public bool IsAvailable
        {
            get { return Execute(() => RemoteObject.IsAvailable); }
        }

        public bool IsRunning
        {
            get { return Execute(() => RemoteObject.IsRunning); }
        }

        public List<string> GetApplicationPools()
        {
            return Execute(() => RemoteObject.GetApplicationPools());
        }
    }
}
