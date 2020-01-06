using Chronos.Client.Win.DotNet.ExceptionMonitor.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.ExceptionMonitor.Actions
{
    public sealed class ExceptionsCommand : ActivateViewModelAction
    {
        public ExceptionsCommand() 
            : base(Constants.ViewModels.ExceptionsViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.ExceptionsMenuItem_Text; }
        }
    }
}
