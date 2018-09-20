using System.Collections.Generic;
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

        public bool HasPermissions
        {
            get { return _services.HasPermissions; }
        }

        public List<WindowsServiceInfo> GetServices()
        {
            List<WindowsServiceInfo> collection = _services.Select(x => new WindowsServiceInfo(x.ServiceName, x.DisplayName)).ToList();
            return collection;
        }
    }
}
