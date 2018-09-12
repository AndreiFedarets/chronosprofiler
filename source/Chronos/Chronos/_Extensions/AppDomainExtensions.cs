using System;
using System.Reflection;

namespace Chronos
{
    public static class AppDomainExtensions
    {
        public static AppDomain Clone(this AppDomain sourceAppDomain, string friendlyName)
        {
            return sourceAppDomain.Clone(friendlyName, AppDomain.CurrentDomain.BaseDirectory);
        }

        public static AppDomain Clone(this AppDomain sourceAppDomain, string friendlyName, string baseDirectory)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationBase = baseDirectory;
            AppDomain appDomain = AppDomain.CreateDomain(friendlyName, null, appDomainSetup);
            return appDomain;
        }

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
