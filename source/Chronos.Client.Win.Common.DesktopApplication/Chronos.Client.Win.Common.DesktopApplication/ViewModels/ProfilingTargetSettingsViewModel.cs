using Chronos.Accessibility.IO;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.ViewModels.Common.DesktopApplication
{
    public class ProfilingTargetSettingsViewModel : ProfilingTargetSettingsBaseViewModel
    {
        private readonly Chronos.Common.DesktopApplication.ProfilingTargetSettings _profilingTargetSettings;
        private readonly IApplicationBase _application;

        public ProfilingTargetSettingsViewModel(IApplicationBase application, ConfigurationSettings configurationSettings, IHostApplicationSelector applicationSelector)
            : base(configurationSettings, applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.DesktopApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            _application = application;
        }

        public override bool DialogReady
        {
            get { return !string.IsNullOrWhiteSpace(FileFullName); }
        }

        public string FileFullName
        {
            get { return _profilingTargetSettings.FileFullName; }
            set
            {
                _profilingTargetSettings.FileFullName = value;
                NotifyOfPropertyChange(() => FileFullName);
                NotifyOfPropertyChange(() => WorkingDirectory);
                NotifyContractSourceChanged();
            }
        }

        public string Arguments
        {
            get { return _profilingTargetSettings.Arguments; }
            set
            {
                _profilingTargetSettings.Arguments = value;
                NotifyOfPropertyChange(() => Arguments);
            }
        }

        public string WorkingDirectory
        {
            get { return _profilingTargetSettings.WorkingDirectory; }
            set
            {
                _profilingTargetSettings.WorkingDirectory = value;
                NotifyOfPropertyChange(() => WorkingDirectory);
            }
        }

        public void BrowseFileFullName()
        {
            IFileSystemAccessor accessor = SelectedApplication.ServiceContainer.Resolve<IFileSystemAccessor>();
            OpenFileViewModel viewModel = new OpenFileViewModel(accessor);
            if (!string.IsNullOrEmpty(FileFullName))
            {
                viewModel.InitialDirectory = System.IO.Path.GetDirectoryName(FileFullName);
                viewModel.FileName = System.IO.Path.GetFileName(FileFullName);
            }
            viewModel.Filter = "Executable (*.exe)|*.exe";
            bool? result = _application.ViewModelManager.ShowDialog(viewModel);
            if (result.HasValue && result.Value)
            {
                FileFullName = viewModel.FileName;
            }
        }
    }
}
