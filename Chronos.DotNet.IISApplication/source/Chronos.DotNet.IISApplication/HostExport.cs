using Chronos.Core;

namespace Chronos.DotNet.IISApplication
{
    public class HostExport : IExtensionExport
    {
        public void Initialize(IChronosApplication application)
        {
            IHostApplication hostApplication = (IHostApplication)application;
            hostApplication.ProfilingTargets.Register(new ProfilingTarget());
        }

        public void Dispose()
        {
        }
    }
}
