using System;
using System.Collections.Generic;

namespace Chronos.Proxy
{
    internal sealed class ServiceContainer : ProxyBaseObject<IServiceContainer>, IServiceContainer
    {
        private readonly Dictionary<Type, ServiceRegistration> _registrations;

        public ServiceContainer(IServiceContainer remoteObject)
            : base(remoteObject, true)
        {
            _registrations = new Dictionary<Type, ServiceRegistration>();
            Register(this);
        }

        public object Resolve(Type type)
        {
            lock (_registrations)
            {
                // 1. Find registration in the local dictionary
                ServiceRegistration registration;
                if (_registrations.TryGetValue(type, out registration))
                {
                    return registration.Service;
                }
                // 2. Take service from the remote container
                object service = RemoteObject.Resolve(type);
                if (service == null)
                {
                    return null;
                }
                // 3. Get service registration by service interface type
                registration = ServiceRegistration.FromType(type, service);
                // 4. If registration was not found, then return null
                if (registration == null)
                {
                    return null;
                }
                // 5.Build proxy registration of the service
                registration = BuildRemoteServiceRegistration(registration);
                _registrations.Add(type, registration);
                return registration.Service;
            }
        }

        public bool IsRegistered(Type type)
        {
            lock (_registrations)
            {
                if (_registrations.ContainsKey(type))
                {
                    return true;
                }
            }
            return RemoteObject.IsRegistered(type);
        }

        public bool Register(object service)
        {
            ServiceRegistration registration = ServiceRegistration.FromService(service);
            lock (_registrations)
            {
                if (_registrations.ContainsKey(registration.ServiceInterfaceType))
                {
                    return false;
                }
                _registrations.Add(registration.ServiceInterfaceType, registration);
            }
            return true;
        }

        private ServiceRegistration BuildRemoteServiceRegistration(ServiceRegistration registration)
        {
            // Building proxy registration of the service:
            // 1. If registration of service has no proxy, then return original service registration
            Type serviceProxyType = registration.ServiceProxyType;
            if (serviceProxyType == null)
            {
                return registration;
            }
            // 2. Build service proxy
            object serviceProxy = this.BuildServiceProxy(registration.Service);
            // 3. Create proxy service registration
            ServiceRegistration proxyRegistration = new ServiceRegistration(registration.ServiceInterfaceType, registration.ServiceProxyType, serviceProxy);
            return proxyRegistration;
        }

        public override void Dispose()
        {
            ExecuteDispose(() =>
            {
                List<ServiceRegistration> collection;
                lock (_registrations)
                {
                    collection = new List<ServiceRegistration>(_registrations.Values);
                    _registrations.Clear();
                }
                foreach (ServiceRegistration registration in collection)
                {
                    object service = registration.Service;
                    if (Equals(service, this))
                    {
                        continue;
                    }
                    service.TryDispose();
                }
            });
            base.Dispose();
        }
    }
}
