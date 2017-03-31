using System.Windows;
using System.Windows.Controls;

namespace Rhiannon.Windows.Controls
{
	[TemplatePart(Name = BrowseButtonPartName, Type = typeof(Button))]
	public class BrowseDirectory : Control
	{
		private const string BrowseButtonPartName = "BROWSE_BUTTON_PART";

		public static readonly DependencyProperty PathProperty;

		private Button _browseButton;

		static BrowseDirectory()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BrowseDirectory), new FrameworkPropertyMetadata(typeof(BrowseDirectory)));
			PathProperty = DependencyProperty.Register("Path", typeof (string), typeof (BrowseDirectory));
		}

		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
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
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.SelectedPath = Path;
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Path = dialog.SelectedPath;
			}
		}
	}
}
