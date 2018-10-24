using Adenium;

namespace Chronos.Client.Win
{
    public interface IProfilingTargetAdapter : Client.IProfilingTargetAdapter
    {
        IViewModel CreateConfigurationViewModel(IContainerViewModel startViewModel);
    }
}
