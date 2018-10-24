namespace Adenium
{
    public interface IDialogContractSource : IContractSource
    {
        bool DialogReady { get; }
    }
}
