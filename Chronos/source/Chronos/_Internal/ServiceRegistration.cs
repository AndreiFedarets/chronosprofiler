using System;
using System.Runtime.InteropServices;

namespace Chronos
{
    /// <summary>
    /// Represents registration of shared service in ServiceContainer
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    internal sealed class ServiceRegistration
    {
        public ServiceRegistration(Type serviceInterfaceType, Type serviceProxyType, object service)
        {
            ServiceInterfaceType = serviceInterfaceType;
            Service = service;
            ServiceProxyType = serviceProxyType;
        }

        public Type ServiceInterfaceType { get; private set; }

        public Type ServiceProxyType { get; private set; }

        public object Service { get; set; }

        public static ServiceRegistration FromService(object service)
        {
            Type serviceInterfaceType = FindServiceInterfaceType(service.GetType());
            if (serviceInterfaceType == null)
            {
                throw new TempException(string.Format("Type {0} is not marked as service", service.GetType()));
            }
            PublicServiceAttribute attribute = serviceInterfaceType.GetCustomAttribute<PublicServiceAttribute>();
            Type serviceProxyType = attribute.ServiceProxyType;
            ServiceRegistration registration = new ServiceRegistration(serviceInterfaceType, serviceProxyType, service);
            return registration;
        }

        public static ServiceRegistration FromType(Type serviceInterfaceType, object service)
        {
            PublicServiceAttribute attribute = serviceInterfaceType.GetCustomAttribute<PublicServiceAttribute>();
            Type serviceProxyType = attribute.ServiceProxyType;
            ServiceRegistration registration = new ServiceRegistration(serviceInterfaceType, serviceProxyType, service);
            return registration;
        }

        public static Type FindServiceInterfaceType(Type serviceType)
        {
            if (serviceType.IsInterface)
            {
                PublicServiceAttribute attribute = serviceType.GetCustomAttribute<PublicServiceAttribute>();
                if (attribute != null)
                {
                    return serviceType;
                }
            }
            Type[] serviceInterfaceTypes = serviceType.GetInterfaces();
            foreach (Type serviceInterfaceType in serviceInterfaceTypes)
            {
                PublicServiceAttribute attribute = serviceInterfaceType.GetCustomAttribute<PublicServiceAttribute>();
                if (attribute != null)
                {
                    return serviceInterfaceType;
                }
            }
            return null;
        }
    }
}
