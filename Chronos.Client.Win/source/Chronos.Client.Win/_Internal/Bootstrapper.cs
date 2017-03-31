using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Microsoft.Practices.Unity;

namespace Chronos.Client.Win
{
    internal sealed class Bootstrapper : BootstrapperBase
    {
        private readonly IContainer _container;

        public Bootstrapper(IContainer container)
        {
            _container = container;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            object result;
            if (string.IsNullOrEmpty(key))
            {
                result = _container.Resolve(service);
            }
            else
            {
                result = _container.Resolve(service, key);
            }
            return result;
        }
    }
}
