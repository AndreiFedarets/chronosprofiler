using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronos.Accessibility.WS
{
    [PublicService(typeof(Proxy.Accessibility.WS.WindowsServicesAccessor))]
    public interface IWindowsServicesAccessor
    {
        WindowsServiceInfo[] GetServices();

    }
}
