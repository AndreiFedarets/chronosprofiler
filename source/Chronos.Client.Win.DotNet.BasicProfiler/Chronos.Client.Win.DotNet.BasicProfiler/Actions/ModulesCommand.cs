using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Actions
{
    public sealed class ModulesCommand : ActivateViewModelAction
    {
        public ModulesCommand()
            : base(Constants.ViewModels.ModulesViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.ModulesMenuItem_Text; }
        }
    }
}
