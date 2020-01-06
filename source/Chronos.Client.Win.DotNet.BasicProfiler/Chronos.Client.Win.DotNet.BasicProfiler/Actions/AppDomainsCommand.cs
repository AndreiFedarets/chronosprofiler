using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Actions
{
    public sealed class AppDomainsCommand : ActivateViewModelAction
    {
        public AppDomainsCommand()
            : base(Constants.ViewModels.AppDomainsViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.AppDomainsMenuItem_Text; }
        }
    }
}
