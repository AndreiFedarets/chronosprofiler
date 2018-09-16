using Chronos.Accessibility.WS;

namespace Chronos.Proxy.Accessibility.WS
{
    internal class WindowsServicesAccessor : ProxyBaseObject<IWindowsServicesAccessor>, IWindowsServicesAccessor
    {
        public WindowsServicesAccessor(IWindowsServicesAccessor remoteObject)
            : base(remoteObject)
        {
        }

        public WindowsServiceInfo[] GetServices()
        {
            return Execute(() => RemoteObject.GetServices());
        }
    }
}
