using System.Linq;

namespace Adenium
{
    public sealed class DialogContract : ContractBase<IDialogContractSource, IDialogContractConsumer>
    {
        protected override void OnContractSourceChanged()
        {
            bool ready = Sources.All(x => x.DialogReady);
            Consumers.ForEach(x => x.OnReadyChanged(ready));
        }
    }
}
