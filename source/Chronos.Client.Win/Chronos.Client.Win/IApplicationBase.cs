using Adenium;

namespace Chronos.Client.Win
{
    public interface IApplicationBase : IChronosApplication
    {
        IContainerViewModel MainViewModel { get; }

        IFrameworkCollection Frameworks { get; }

        IProfilingTypeCollection ProfilingTypes { get; }

        IProfilingTargetCollection ProfilingTargets { get; }

        IProductivityCollection Productivities { get; }

        IViewModelManager ViewModelManager { get; }

        void Activate();
    }
}
