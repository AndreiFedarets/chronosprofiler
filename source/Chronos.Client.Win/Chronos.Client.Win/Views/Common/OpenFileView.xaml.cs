using System;
using System.Collections.Specialized;
using System.Windows;
using Chronos.Client.Win.ViewModels.Common;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chronos.Client.Win.Views.Common
{
    public partial class OpenFileView
    {
        public OpenFileView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OpenFileViewModel viewModel = e.OldValue as OpenFileViewModel;
            if (viewModel != null)
            {
                viewModel.FileSystemInfosChanged -= OnFileSystemInfosChanged;
            }
            viewModel = e.NewValue as OpenFileViewModel;
            if (viewModel != null)
            {
                viewModel.FileSystemInfosChanged += OnFileSystemInfosChanged;
                viewModel.Initialize();
            }
            ResetFileSystemInfosSelection();
        }

        private void OnFileSystemInfosChanged(object sender, EventArgs e)
        {
            ResetFileSystemInfosSelection();
        }

        private void ResetFileSystemInfosSelection()
        {
            if (FileSystemInfosList.Items.Count > 0)
            {
                FileSystemInfosList.SelectedIndex = 0;
            }
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
                case Key.Enter:
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

        private void OnFilesListBoxItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileViewModel viewModel = (OpenFileViewModel)DataContext;
            viewModel.OpenSelectedFileSystemInfo();
        }
    }
}
