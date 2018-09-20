using System.Collections.Generic;
using Chronos.Accessibility.WS;

namespace Chronos.Proxy.Accessibility.WS
{
    internal class WindowsServicesAccessor : ProxyBaseObject<IWindowsServicesAccessor>, IWindowsServicesAccessor
    {
        public WindowsServicesAccessor(IWindowsServicesAccessor remoteObject)
            : base(remoteObject)
        {
        }

        public bool HasPermissions
        {
            get { return Execute(() => RemoteObject.HasPermissions); }
        }

        public List<WindowsServiceInfo> GetServices()
        {
            return Execute(() => RemoteObject.GetServices());
        }
    }
}
