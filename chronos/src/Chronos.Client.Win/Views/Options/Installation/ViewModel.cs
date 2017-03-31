using System.Windows.Input;
using Chronos.Installation;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Options.Installation
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IProfilerInstaller _installer;
		private readonly ITaskFactory _taskFactory;
		private bool _isServerStarted;

		public ViewModel(IProfilerInstaller installer, ITaskFactory taskFactory)
		{
			_installer = installer;
			_taskFactory = taskFactory;
		}

		public bool IsServerStarted
		{
			get { return _isServerStarted; }
			set
			{
				_isServerStarted = value;
				NotifyPropertyChanged(() => IsServerStarted);
			}
		}

		public ICommand InstallProfilerObjectCommand { get; private set; }

		public ICommand UninstallProfilerObjectCommand { get; private set; }

		public ICommand StartProfilerServerCommand { get; private set; }

		public ICommand StopProfilerServerCommand { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			InstallProfilerObjectCommand = new AsyncCommand(InstallProfilerObject, _taskFactory);
			UninstallProfilerObjectCommand = new AsyncCommand(UninstallProfilerObject, _taskFactory);
		}

		private void InstallProfilerObject()
		{
			_installer.InstallProfiler();
		}

		private void UninstallProfilerObject()
		{
			_installer.UninstallProfiler();
		}
	}
}
