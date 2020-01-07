using Layex;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    internal sealed class CompositeDependencyContainer : BuiltinDependencyContainer
    {
        private readonly IServiceContainer _serviceContainer;

        public CompositeDependencyContainer(IServiceContainer serviceContainer)
            : this(serviceContainer, null)
        {
        }

        private CompositeDependencyContainer(IServiceContainer serviceContainer, BuiltinDependencyContainer parent)
            : base(parent)
        {
            _serviceContainer = serviceContainer;
        }

        protected override bool TryGetRegistration(Type type, string key, out IRegistration registration)
        {
            if (IsPublicService(type) && _serviceContainer.IsRegistered(type))
            {
                object instance = _serviceContainer.Resolve(type);
                registration = new InstanceRegistration(instance);
                return true;
            }
            return base.TryGetRegistration(type, key, out registration);
        }

        public override IDependencyContainer CreateChildContainer()
        {
            return new CompositeDependencyContainer(_serviceContainer, this);
        }

        private bool IsPublicService(Type type)
        {
            return type.GetCustomAttribute<PublicServiceAttribute>() != null;
        }
    }
}
