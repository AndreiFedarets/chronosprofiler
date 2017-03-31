using System;
using System.Runtime.InteropServices;
using Chronos.Proxy;

namespace Chronos
{
    /// <summary>
    /// Represents registration of shared service in ServiceContainer
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    public sealed class ProxyRegistration
    {
        public ProxyRegistration(Guid key, Type type)
        {
            Key = key;
            Type = type;
        }

        public Guid Key { get; private set; }

        public Type Type { get; private set; }

        internal static ProxyRegistration FromService(object service)
        {
            ServiceAttribute attribute = service.GetType().GetCustomAttribute<ServiceAttribute>();
            if (attribute == null)
            {
                return null;
            }
            ProxyRegistration registration = new ProxyRegistration(attribute.ProxyKey, attribute.ServiceProxyType);
            return registration;
        }

        internal static Guid GetKeyFromType(Type type)
        {
            return type.GUID;
        }
    }
}
