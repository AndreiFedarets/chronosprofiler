using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    [TemplatePart(Name = BrowseButtonPartName, Type = typeof (Button))]
    public class BrowseFile : Control
    {
        private const string BrowseButtonPartName = "BROWSE_BUTTON_PART";

        public static readonly DependencyProperty PathProperty;
        public static readonly DependencyProperty FilterProperty;

        private Button _browseButton;

        static BrowseFile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (BrowseFile), new FrameworkPropertyMetadata(typeof (BrowseFile)));
            PathProperty = DependencyProperty.Register("Path", typeof (string), typeof (BrowseFile));
            FilterProperty = DependencyProperty.Register("Filter", typeof (string), typeof (BrowseFile));
        }

        public string Path
        {
            get { return (string) GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public string Filter
        {
            get { return (string) GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //SEARCH_BUTTON_PART
            if (_browseButton != null)
            {
                _browseButton.Click -= OnBrowseButtonClick;
            }
            _browseButton = GetTemplateChild(BrowseButtonPartName) as Button;
            if (_browseButton != null)
            {
                _browseButton.Click += OnBrowseButtonClick;
            }
        }

        private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = Filter;
            if (!string.IsNullOrEmpty(Path))
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Path);
                dialog.FileName = System.IO.Path.GetFileName(Path);
            }
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Path = dialog.FileName;
            }
        }
    }
}
