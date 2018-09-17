using System.Linq;

namespace Chronos.Accessibility.WS
{
    internal sealed class WindowsServicesAccessor : RemoteBaseObject, IWindowsServicesAccessor
    {
        private readonly IWindowsServiceCollection _services;

        public WindowsServicesAccessor(IWindowsServiceCollection services)
        {
            _services = services;
        }

        public WindowsServiceInfo[] GetServices()
        {
            WindowsServiceInfo[] collection = _services.Select(x => new WindowsServiceInfo(x.ServiceName, x.DisplayName)).ToArray();
            return collection;
        }
    }
}
