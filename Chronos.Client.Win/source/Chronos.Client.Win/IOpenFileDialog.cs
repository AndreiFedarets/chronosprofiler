namespace Chronos.Client.Win
{
    public interface IOpenFileDialog
    {
        string Filter { get; set; }

        string InitialDirectory { get; set; }

        string FileName { get; set; }

        bool? ShowDialog();
    }
}
