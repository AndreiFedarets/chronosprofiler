namespace Chronos.Client.Win.Contracts.Dialog
{
    public interface IContractConsumer : Contracts.IContractConsumer
    {
        void OnReadyChanged(bool ready);
    }
}
