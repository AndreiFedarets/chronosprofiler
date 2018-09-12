using System.Windows.Forms;

namespace Chronos.Client.Win
{
    internal sealed class OpenFileDialog : IOpenFileDialog
    {
        private readonly System.Windows.Forms.OpenFileDialog _dialog;

        public OpenFileDialog(Host.IApplication hostApplication)
        {
            _dialog = new System.Windows.Forms.OpenFileDialog();
        }

        public string Filter
        {
            get { return _dialog.Filter; }
            set { _dialog.Filter = value; }
        }

        public string InitialDirectory
        {
            get { return _dialog.InitialDirectory; }
            set { _dialog.InitialDirectory = value; }
        }

        public string FileName
        {
            get { return _dialog.FileName; }
            set { _dialog.FileName = value; }
        }

        public bool? ShowDialog()
        {
            DialogResult dialogResult = _dialog.ShowDialog();
            bool? result = null;
            switch (dialogResult)
            {
                case DialogResult.OK:
                    result = true;
                    break;
                case DialogResult.Cancel:
                    result = false;
                    break;
            }
            return result;
        }
    }
}
