using System.Linq;

namespace Chronos.Accessibility.WS
{
    internal sealed class WindowsServicesAccessor : RemoteBaseObject, IWindowsServicesAccessor
    {
        public WindowsServiceInfo[] GetServices()
        {
            ServiceControllerCollection controllers = new ServiceControllerCollection();
            WindowsServiceInfo[] collection = controllers.Select(x => new WindowsServiceInfo(x.ServiceName, x.DisplayName)).ToArray();
            return collection;
        }
    }
}
