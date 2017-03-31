using Chronos.Client.Win.Common.EventsTree;
using Chronos.Client.Win.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.TracingProfiler
{
    internal class ProfilingTypeSession : IProfilingTypeSession
    {
        public void IntegrateViewModel(object profilingResultsViewModel)
        {
            
        }

        public void ExportServices(IServiceContainer container)
        {
        }

        public void ImportServices(IServiceContainer container)
        {
            IEventsFormatter eventsFormatter = container.Resolve<IEventsFormatter>();
            IFunctionCollection functions = container.Resolve<IFunctionCollection>();
            IThreadCollection threads = container.Resolve<IThreadCollection>();
            eventsFormatter.RegisterFormatter(new FunctionCallEventFormatter(functions));
            eventsFormatter.RegisterFormatter(new RootEventFormatter(threads));
        }

        public void ReloadData()
        {
            
        }
    }
}
