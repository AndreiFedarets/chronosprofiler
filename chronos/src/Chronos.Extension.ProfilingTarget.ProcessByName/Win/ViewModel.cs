using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Extension.ProfilingTarget.ProcessByName.Win
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly ProcessPlatformDetector _processPlatformDetector;
		private string _processName;
		private ProcessPlatform _processPlatform;

		public ViewModel()
		{
			_processPlatformDetector = new ProcessPlatformDetector();
		}

		public ICommand BrowseForProcessCommand { get; private set; }

		public string ProcessName
		{
			get { return _processName; }
			set { SetPropertyAndNotifyChanged(() => ProcessName, ref _processName, value); }
		}

		public ProcessPlatform ProcessPlatform
		{
			get { return _processPlatform; }
			set { SetPropertyAndNotifyChanged(() => ProcessPlatform, ref _processPlatform, value); }
		}

		public IEnumerable<ProcessPlatform> Platforms { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			Platforms = new[] {ProcessPlatform.I386, ProcessPlatform.X64, ProcessPlatform.Itanium};
			BrowseForProcessCommand = new SyncCommand(BrowseForProcess);
		}

		private void BrowseForProcess()
		{
			System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string fileFullName = dialog.FileName;
				ProcessPlatform = _processPlatformDetector.DetectProcessPlatform(fileFullName);
				ProcessName = Path.GetFileName(fileFullName).ToLowerInvariant();
			}
		}
	}
}
