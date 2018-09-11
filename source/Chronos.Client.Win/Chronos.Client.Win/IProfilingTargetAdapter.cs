using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win
{
    public interface IProfilingTargetAdapter : Client.IProfilingTargetAdapter
    {
        ViewModel CreateConfigurationViewModel(ViewModels.Start.StartPageViewModel pageViewModel);
    }
}
