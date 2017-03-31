using Chronos.Client.Win.ViewModels.Common;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chronos.Client.Win.Views.Common
{
    public partial class OpenFileView
    {
        public OpenFileView()
        {
            InitializeComponent();
        }

        private void OnFilesListBoxKeyDown(object sender, KeyEventArgs e)
        {
            OpenFileViewModel viewModel = (OpenFileViewModel)DataContext;
            switch (e.Key)
            {
                case Key.Enter:
                    viewModel.OpenSelectedFileSystemInfo();
                    break;
                case Key.Back:
                    viewModel.OpenParentFileSystemInfo();
                    break;
            }
        }

        private void OnCurrentPathTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    TextBox textBox = (TextBox)sender;
                    OpenFileViewModel viewModel = (OpenFileViewModel)DataContext;
                    viewModel.OpenFileSystemInfo(textBox.Text);
                    break;
            }
        }

        private void OnFilesListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            listBox.Focus();
        }
    }
}
