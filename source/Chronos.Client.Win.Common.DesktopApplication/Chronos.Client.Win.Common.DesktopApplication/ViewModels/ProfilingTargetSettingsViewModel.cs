using Chronos.Accessibility.IO;
using Chronos.Client.Win.ViewModels.Common;
using Chronos.Client.Win.ViewModels.Start;
using Layex.Extensions;
using Layex.ViewModels;

namespace Chronos.Client.Win.Common.DesktopApplication.ViewModels
{
    [ViewModel(Constants.ViewModels.ProfilingTargetSettings)]
    public class ProfilingTargetSettingsViewModel : ProfilingTargetSettingsBaseViewModel
    {
        private readonly Chronos.Common.DesktopApplication.ProfilingTargetSettings _profilingTargetSettings;
        private readonly IViewModelManager _viewModelManager;

        public ProfilingTargetSettingsViewModel(IViewModelManager viewModelManager, ConfigurationSettings configurationSettings, IHostApplicationSelector applicationSelector)
            : base(configurationSettings, applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.DesktopApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            _viewModelManager = viewModelManager;
        }

        public override string DisplayName
        {
            get { return "Target Executable"; }
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
            OpenFileViewModelSettings settings = new OpenFileViewModelSettings(accessor);
            if (!string.IsNullOrEmpty(FileFullName))
            {
                settings.InitialDirectory = System.IO.Path.GetDirectoryName(FileFullName);
                settings.FileName = System.IO.Path.GetFileName(FileFullName);
            }
            settings.Filters.Add("Executable (*.exe)|*.exe");
            IDialogViewModel openFileViewModel = (IDialogViewModel)_viewModelManager.Activate(Win.Constants.ViewModels.OpenFile, settings);
            bool? dialogResult = openFileViewModel.DialogResult;
            if (dialogResult.HasValue && dialogResult.Value)
            {
                FileFullName = settings.FileName;
            }
        }
    }
}
