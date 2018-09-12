namespace Chronos
{
    public static class AdapterResolver
    {
        public static IProfilingTypeAdapter GetSafeAdapter(this IProfilingType profilingType)
        {
            return (IProfilingTypeAdapter)profilingType;
        }

        public static IProfilingTargetAdapter GetSafeAdapter(this IProfilingTarget profilingTarget)
        {
            return (IProfilingTargetAdapter)profilingTarget;
        }

        public static IFrameworkAdapter GetSafeAdapter(this IFramework framework)
        {
            return (IFrameworkAdapter)framework;
        }

        public static IApplicationExtensionAdapter GetSafeAdapter(this IApplicationExtension applicationExtension)
        {
            return (IApplicationExtensionAdapter)applicationExtension;
        }

        public static IProfilingTypeAdapter GetRealAdapter(this IProfilingType profilingType)
        {
            return (IProfilingTypeAdapter)((IWrapper)profilingType).UndrelyingObject;
        }

        public static IProfilingTargetAdapter GetRealAdapter(this IProfilingTarget profilingTarget)
        {
            return (IProfilingTargetAdapter)((IWrapper)profilingTarget).UndrelyingObject;
        }

        public static IFrameworkAdapter GetRealAdapter(this IFramework framework)
        {
            return (IFrameworkAdapter)((IWrapper)framework).UndrelyingObject;
        }

        public static IApplicationExtensionAdapter GetRealAdapter(this IApplicationExtension applicationExtension)
        {
            return (IApplicationExtensionAdapter)((IWrapper)applicationExtension).UndrelyingObject;
        }
    }
}
