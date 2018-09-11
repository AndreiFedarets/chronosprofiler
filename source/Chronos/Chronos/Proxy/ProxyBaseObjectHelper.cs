using System;

namespace Chronos.Proxy
{
    public static class ProxyBaseObjectHelper
    {
        public static T ResolveRealRemoteObject<T>(T remoteObject)
        {
            IProxyObject proxyObject = remoteObject as IProxyObject;
            if (proxyObject == null)
            {
                return remoteObject;
            }
            try
            {
                return (T)proxyObject.GetRemoteObject();
            }
            catch (Exception)
            {
                return remoteObject;
            }
        }

    }
}
