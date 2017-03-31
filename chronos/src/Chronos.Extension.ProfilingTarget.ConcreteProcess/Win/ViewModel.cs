using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ConcreteProcess.Win
{
    public class ViewModel : ViewModelBase, IViewModel
    {
        private string _fileFullName;
        private string _arguments;
        private bool _profileChildProcess;

        public string FileFullName
        {
            get { return _fileFullName; }
            set { SetPropertyAndNotifyChanged(() => FileFullName, ref _fileFullName, value); }
        }

        public string Arguments
        {
            get { return _arguments; }
            set { SetPropertyAndNotifyChanged(() => Arguments, ref _arguments, value); }
        }


        public bool ProfileChildProcess
        {
            get { return _profileChildProcess; }
            set { SetPropertyAndNotifyChanged(() => ProfileChildProcess, ref _profileChildProcess, value); }
        }
    }
}
