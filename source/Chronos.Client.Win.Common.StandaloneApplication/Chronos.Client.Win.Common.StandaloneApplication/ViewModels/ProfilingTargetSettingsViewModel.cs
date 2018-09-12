using Chronos.Remote.IO;
using System;

namespace Chronos.Client.Win.ViewModels.Common.StandaloneApplication
{
    public class ProfilingTargetSettingsViewModel : ViewModel, Contracts.Dialog.IContractSource
    {
        private readonly Chronos.Common.StandaloneApplication.ProfilingTargetSettings _profilingTargetSettings;
        private readonly IHostApplicationSelector _applicationSelector;
        private readonly IApplicationBase _application;

        public ProfilingTargetSettingsViewModel(IApplicationBase application, ConfigurationSettings configurationSettings,
            IHostApplicationSelector applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.StandaloneApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            _application = application;
            _applicationSelector = applicationSelector;
            _applicationSelector.SelectionChanged += ApplicationSelectorSelectionChanged;
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

        public bool ProfileChildProcess
        {
            get { return _profilingTargetSettings.ProfileChildProcess; }
            set
            {
                _profilingTargetSettings.ProfileChildProcess = value;
                NotifyOfPropertyChange(() => ProfileChildProcess);
            }
        }

        public Host.IApplication SelectedApplication
        {
            get { return _applicationSelector.SelectedApplication; }
        }

        public event EventHandler ContractSourceChanged;

        //public bool CanBrowseFileFullName()
        //{
        //    return 
        //}

        public void BrowseFileFullName()
        {
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            //openFileDialog1.ShowDialog();

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

        public override void Dispose()
        {
            _applicationSelector.SelectionChanged -= ApplicationSelectorSelectionChanged;
            base.Dispose();
        }

        private void ApplicationSelectorSelectionChanged(object sender, System.EventArgs e)
        {
            NotifyOfPropertyChange(() => SelectedApplication);
        }

        private void NotifyContractSourceChanged()
        {
            EventHandler handler = ContractSourceChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
