using System;
using System.Collections.Generic;

namespace Chronos
{
    public sealed class ServiceContainer : RemoteBaseObject, IServiceContainer
    {
        private readonly Dictionary<Type, ServiceRegistration> _registrations;

        public ServiceContainer()
        {
            _registrations = new Dictionary<Type, ServiceRegistration>();
            Register(this);
        }

        public object Resolve(Type type)
        {
            lock (_registrations)
            {
                ServiceRegistration registration;
                if (_registrations.TryGetValue(type, out registration))
                {
                    return registration.Service;
                }
                return null;
            }
        }

        public bool IsRegistered(Type type)
        {
            lock (_registrations)
            {
                return _registrations.ContainsKey(type);
            }
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

        public override void Dispose()
        {
            lock (_registrations)
            {
                foreach (ServiceRegistration registration in _registrations.Values)
                {
                    object service = registration.Service;
                    if (object.Equals(service, this))
                    {
                        continue;
                    }
                    else if (service is IDisposable)
                    {
                        ((IDisposable)service).Dispose();
                    }
                }
                _registrations.Clear();
            }
            base.Dispose();
        }
    }
}
