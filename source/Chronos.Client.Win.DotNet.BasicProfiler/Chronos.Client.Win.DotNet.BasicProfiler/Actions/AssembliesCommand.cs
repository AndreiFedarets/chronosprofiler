using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Actions
{
    public sealed class AssembliesCommand : ActivateViewModelAction
    {
        public AssembliesCommand()
            : base(Constants.ViewModels.AssembliesViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.AssembliesMenuItem_Text; }
        }
    }
}
