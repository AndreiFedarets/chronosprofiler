using System;

namespace Chronos.Communication.Native
{
    public interface IRequestClient
    {
        object Invoke(Guid operationId, Type returnType, params object[] args);
    }

    public static class RequestClientExtensions
    {
        public static T Invoke<T>(this IRequestClient requestClient, Guid operationId, params object[] args)
        {
            return (T)requestClient.Invoke(operationId, typeof(T), args);
        }
    }
}
