using System;

namespace Chronos.Client.Win
{
    internal static class ObjectExtensions
    {
        internal static void TryDispose(this object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
