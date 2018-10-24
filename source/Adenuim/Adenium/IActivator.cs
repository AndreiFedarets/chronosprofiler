using System;

namespace Adenium
{
    public interface IActivator
    {
        object Resolve(Type type);
    }

    public static class ActivatorExtensions
    {
        public static T Resolve<T>(this IActivator activator)
        {
            return (T)activator.Resolve(typeof(T));
        }
    }
}
