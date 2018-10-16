using Chronos.Client.Win.ViewModels;
using Chronos.Messaging;

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

        IMessageBus MessageBus { get; }

        void Activate();
    }
}
