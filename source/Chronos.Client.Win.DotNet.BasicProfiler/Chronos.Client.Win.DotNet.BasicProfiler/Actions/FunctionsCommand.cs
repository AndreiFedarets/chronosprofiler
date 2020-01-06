using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Actions
{
    public sealed class FunctionsCommand : ActivateViewModelAction
    {
        public FunctionsCommand()
            : base(Constants.ViewModels.FunctionsViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.FunctionsMenuItem_Text; }
        }
    }
}
