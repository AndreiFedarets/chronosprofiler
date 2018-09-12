using System;

namespace Chronos
{
    public static class ObjectExtensions
    {
        internal static void TryDispose(this object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        internal static void TryInitialize(this object obj, object application)
        {
            IInitializable initializable = obj as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize(application);
            }
        }
    }
}
