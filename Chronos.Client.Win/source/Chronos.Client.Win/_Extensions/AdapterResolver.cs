namespace Chronos.Client.Win
{
    public static class AdapterResolver
    {
        public static IProductivityAdapter GetWinAdapter(this IProductivity productivity)
        {
            return (IProductivityAdapter)productivity.GetRealAdapter();
        }

        public static IProfilingTypeAdapter GetWinAdapter(this IProfilingType profilingType)
        {
            return (IProfilingTypeAdapter)profilingType.GetRealAdapter();
        }

        public static IProfilingTargetAdapter GetWinAdapter(this IProfilingTarget profilingTarget)
        {
            return (IProfilingTargetAdapter) profilingTarget.GetRealAdapter();
        }

        public static IFrameworkAdapter GetWinAdapter(this IFramework framework)
        {
            return (IFrameworkAdapter) framework.GetRealAdapter();
        }
    }
}
