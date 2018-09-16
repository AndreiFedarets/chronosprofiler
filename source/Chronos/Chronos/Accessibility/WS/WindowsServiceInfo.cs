using System;

namespace Chronos.Accessibility.WS
{
    [Serializable]
    public sealed class WindowsServiceInfo
    {
        public WindowsServiceInfo(string serviceName, string displayName)
        {
            ServiceName = serviceName;
            DisplayName = displayName;
        }

        public string ServiceName { get; private set; }

        public string DisplayName { get; private set; }
    }
}
