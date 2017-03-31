using Rhiannon.Threading;
using Rhiannon.Windows.Presentation.Commands;
using Chronos.Core;

namespace Chronos.Client.Win.Commands
{
    public class ReloadProcessShadowCommand : AsyncCommand
    {
        private readonly IProcessShadow _processShadow;

        public ReloadProcessShadowCommand(IProcessShadow processShadow, ITaskFactory taskFactory)
            : base(taskFactory)
        {
            _processShadow = processShadow;
        }

        public override void Execute()
        {
            _processShadow.ReloadAll();
        }
    }
}
