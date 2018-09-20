using Chronos.Accessibility.IIS;

namespace Chronos.Common.WebApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        private readonly IInternetInformationServiceAccessor _internetInformationService;
        private ProfilingTargetController _currentController;

        public ProfilingTargetAdapter(IInternetInformationServiceAccessor internetInformationService)
        {
            _internetInformationService = internetInformationService;
        }

        private bool IsProfilingActive
        {
            get { return _currentController != null; }
        } 

        public IProfilingTargetController CreateController(ConfigurationSettings settings)
        {
            if (IsProfilingActive)
            {
                return null;
            }
            ProfilingTargetSettings profilingTargetSettings = new ProfilingTargetSettings(settings.ProfilingTargetSettings);
            _currentController = new ProfilingTargetController(profilingTargetSettings, _internetInformationService);
            _currentController.TargetStopped += OnControllerTargetStopped;
            return _currentController;
        }

        private void OnControllerTargetStopped(object sender, ProfilingTargetControllerEventArgs e)
        {
            _currentController.TargetStopped -= OnControllerTargetStopped;
            _currentController = null;
        }

        public bool CanStartProfiling(ConfigurationSettings settings, int processId)
        {
            //using (Process process = Process.GetProcessById(processId))
            //{
            //    ProfilingTargetSettings profilingTargetSettings = new ProfilingTargetSettings(settings.ProfilingTargetSettings);
            //    string targetProcessName = Path.GetFileNameWithoutExtension(profilingTargetSettings.FileFullName);
            //    if (!string.Equals(process.ProcessName, targetProcessName, StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        public void ProfilingStarted(ConfigurationSettings configurationSettings, SessionSettings sessionSettings, int processId)
        {
            _currentController.AttachProcess(configurationSettings, sessionSettings, processId);
        }
    }
}
