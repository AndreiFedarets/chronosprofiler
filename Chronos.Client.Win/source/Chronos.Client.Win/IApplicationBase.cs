using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win
{
    public interface IApplicationBase : IChronosApplication
    {
        PageViewModel MainViewModel { get; }

        IFrameworkCollection Frameworks { get; }

        IProfilingTypeCollection ProfilingTypes { get; }

        IProfilingTargetCollection ProfilingTargets { get; }

        IProductivityCollection Productivities { get; }

        IViewModelManager ViewModelManager { get; }
    }
}
