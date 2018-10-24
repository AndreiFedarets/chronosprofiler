using System;
using System.Collections.Generic;

namespace Adenium
{
    public interface IContainer : IActivator
    {
        IContainer Parent { get; }

        IContainer CreateChildContainer();

        IContainer RegisterInstance(Type type, object instance);

        IContainer RegisterType(Type from, Type to, bool singleton = false);

        object Resolve(Type type, string key);

        IEnumerable<object> ResolveAll(Type type);
    }

    public static class ContainerExtensions
    {
        public static IContainer RegisterInstance<T>(this IContainer container, T instance)
        {
            return container.RegisterInstance(typeof(T), instance);
        }

        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, bool singleton = false)
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), singleton);
        }

        public static T Resolve<T>(this IContainer container)
        {
            return (T)container.Resolve(typeof(T));
        }
    }
}
