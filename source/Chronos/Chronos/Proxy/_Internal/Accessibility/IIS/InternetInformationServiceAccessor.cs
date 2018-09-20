using System.Collections.Generic;
using Chronos.Accessibility.IIS;

namespace Chronos.Proxy.Accessibility.IIS
{
    internal sealed class InternetInformationServiceAccessor : ProxyBaseObject<IInternetInformationServiceAccessor>, IInternetInformationServiceAccessor
    {
        public InternetInformationServiceAccessor(IInternetInformationServiceAccessor remoteObject)
            : base(remoteObject)
        {
        }

        public bool IsAvailable
        {
            get { return Execute(() => RemoteObject.IsAvailable); }
        }

        public bool HasPermissions
        {
            get { return Execute(() => RemoteObject.HasPermissions); }
        }

        public List<string> GetApplicationPools()
        {
            return Execute(() => RemoteObject.GetApplicationPools());
        }
    }
}
