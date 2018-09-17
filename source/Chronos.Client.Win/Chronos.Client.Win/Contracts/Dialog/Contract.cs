using System.Linq;

namespace Chronos.Client.Win.Contracts.Dialog
{
    public sealed class Contract : ContractBase<IContractSource, IContractConsumer>
    {
        protected override void OnContractSourceChanged()
        {
            bool ready = Sources.All(x => x.Ready);
            Consumers.ForEach(x => x.OnReadyChanged(ready));
        }
    }
}
