using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ConcreteProcess.Win
{
    public interface IViewModel : IViewModelBase
    {
        string FileFullName { get; set; }

        string Arguments { get; set; }

        bool ProfileChildProcess { get; set; }
    }
}
