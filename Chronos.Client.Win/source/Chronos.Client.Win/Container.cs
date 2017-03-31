using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    public class Container : IContainer
    {
        private readonly IUnityContainer _container;

        public Container()
            : this(new UnityContainer(), null)
        {
        }

        private Container(IUnityContainer container, IContainer parent)
        {
            _container = container;
            Parent = parent;
        }

        public IContainer Parent { get; private set; }

        public IContainer CreateChildContainer()
        {
            IContainer child = new Container(_container.CreateChildContainer(), this);
            return child;
        }

        public IContainer RegisterInstance(Type type, object instance)
        {
            _container.RegisterInstance(type, instance);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, bool singleton = false)
        {
            if (singleton)
            {
                _container.RegisterType(from, to, new ContainerControlledLifetimeManager());
            }
            else
            {
                _container.RegisterType(from, to);
            }
            return this;
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, string key)
        {
            return _container.Resolve(type, key);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            IEnumerable<object> collection = _container.ResolveAll(type);
            return collection;
        }
    }
}
