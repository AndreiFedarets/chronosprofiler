using System;
using System.Reflection;

namespace Chronos.Client.Win
{
    internal static class AppDomainExtensions
    {
        public static object InvokeStaticMember(this AppDomain appDomain, Type type, string name, BindingFlags flags, params object[] args)
        {
            RemoteExecutor executor = RemoteActivator.CreateInstance<RemoteExecutor>(appDomain);
            return executor.InvokeStaticMember(type, name, flags, args);
        }

        internal class RemoteExecutor : MarshalByRefObject
        {
            public object InvokeStaticMember(Type type, string name, BindingFlags flags, params object[] args)
            {
                return type.InvokeMember(name, flags | BindingFlags.Static, null, null, args);
            }
        }
    }
}
