using System;

namespace Chronos.Proxy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceProxyInitializationMethodAttribute : Attribute
    {
    }
}
