using System;

namespace Chronos
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class PublicServiceAttribute : Attribute
    {
        public PublicServiceAttribute(Type serviceProxyType)
        {
            ServiceProxyType = serviceProxyType;
        }

        public PublicServiceAttribute()
            : this(null)
        {

        }

        public Type ServiceProxyType { get; private set; }
    }
}
