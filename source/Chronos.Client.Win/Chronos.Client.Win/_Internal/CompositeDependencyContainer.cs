using Layex;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    internal sealed class CompositeDependencyContainer : IDependencyContainer
    {
        private readonly IDependencyContainer _currentContainer;
        private readonly IServiceContainer _serviceContainer;


        public CompositeDependencyContainer(IServiceContainer serviceContainer, IDependencyContainer currentContainer)
        {
            _serviceContainer = serviceContainer;
            _currentContainer = currentContainer;
        }

        public bool IsRegistered(Type type)
        {
            return _currentContainer.IsRegistered(type) || _serviceContainer.IsRegistered(type);
        }

        public bool IsRegistered(Type type, string key)
        {
            if (string.IsNullOrEmpty(key) && IsPublicService(type))
            {
                return _serviceContainer.IsRegistered(type);
            }
            return _currentContainer.IsRegistered(type, key);
        }

        public IDependencyContainer CreateChildContainer()
        {
            IDependencyContainer childContainer = _currentContainer.CreateChildContainer();
            return new CompositeDependencyContainer(_serviceContainer, childContainer);
        }

        public IDependencyContainer RegisterInstance(Type type, object instance)
        {
            _currentContainer.RegisterInstance(type, instance);
            return this;
        }

        public IDependencyContainer RegisterInstance(Type type, object instance, string key)
        {
            _currentContainer.RegisterInstance(type, instance, key);
            return this;
        }

        public IDependencyContainer RegisterType(Type from, Type to, bool singleton = false)
        {
            _currentContainer.RegisterType(from, to, singleton);
            return this;
        }

        public IDependencyContainer RegisterType(Type from, Type to, string key, bool singleton = false)
        {
            _currentContainer.RegisterType(from, to, key, singleton);
            return this;
        }

        public object Resolve(Type type)
        {
            if (IsPublicService(type))
            {
                return _serviceContainer.Resolve(type);
            }
            return _currentContainer.Resolve(type);
        }

        public object Resolve(Type type, string key)
        {
            if (string.IsNullOrEmpty(key) && IsPublicService(type))
            {
                return _serviceContainer.Resolve(type);
            }
            return _currentContainer.Resolve(type);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            if (IsPublicService(type))
            {
                return new[] { _serviceContainer.Resolve(type) };
            }
            return _currentContainer.ResolveAll(type);
        }

        private bool IsPublicService(Type type)
        {
            return type.GetCustomAttribute<PublicServiceAttribute>() != null;
        }
    }
}
