using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Actions
{
    public sealed class ClassesCommand : ActivateViewModelAction
    {
        public ClassesCommand()
            : base(Constants.ViewModels.ClassesViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.ClassesMenuItem_Text; }
        }
    }
}
