using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Actions
{
    public sealed class ThreadsCommand : ActivateViewModelAction
    {
        public ThreadsCommand()
            : base(Constants.ViewModels.ThreadsViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.ThreadsMenuItem_Text; }
        }
    }
}
