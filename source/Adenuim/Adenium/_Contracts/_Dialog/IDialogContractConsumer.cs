namespace Adenium
{
    public interface IDialogContractConsumer : IContractConsumer
    {
        void OnReadyChanged(bool ready);
    }
}
