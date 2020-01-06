using Layex.ViewModels;

namespace Chronos.Client.Win
{
    public interface IApplicationBase : IChronosApplication
    {
        IFrameworkCollection Frameworks { get; }

        IProfilingTypeCollection ProfilingTypes { get; }

        IProfilingTargetCollection ProfilingTargets { get; }

        IProductivityCollection Productivities { get; }

        void Activate();
    }
}
