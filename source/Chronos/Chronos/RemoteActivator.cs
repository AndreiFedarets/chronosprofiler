using System;
using System.Globalization;
using System.Reflection;

namespace Chronos
{
    public static class RemoteActivator
    {
        public static T CreateInstance<T>(AppDomain appDomain, params object[] args)
        {
            Type type = typeof(T);
            return (T)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName,
                    false, BindingFlags.CreateInstance, null, args, CultureInfo.CurrentCulture, null);
        }
    }
}
