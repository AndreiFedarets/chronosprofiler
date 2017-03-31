using Rhiannon.Windows.Presentation.Commands;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win.Commands
{
    public class CloseDocumentCommand : SyncCommand<IDocument>
    {
        public override bool CanExecute(IDocument parameter)
        {
            return parameter != null;
        }

        public override void Execute(IDocument parameter)
        {
            parameter.Close();
        }
    }
}
