using Chronos.DotNet.BasicProfiler;
using System;

namespace Chronos.Client.Win.DotNet.FindReference.BasicProfiler
{
    public class ProductivityAdapter : IProductivityAdapter, IInitializable, IDisposable
    {
        private static readonly Guid BasicProfilerUid;
        private static readonly Guid EventTreeUid;
        private ContextMenuIntegrationCollection _integration;

        static ProductivityAdapter()
        {
            BasicProfilerUid = new Guid("{3F6FCEF6-8A28-4F44-8F39-51DA5A804724}");
            EventTreeUid = new Guid("{B3352C62-FCAB-45CA-8EEB-EA296E8C3122}");
        }

        public void Initialize(IChronosApplication applicationObject)
        {
            IProfilingApplication application = (IProfilingApplication)applicationObject;
            if (!application.ProfilingTypes.Contains(EventTreeUid) ||
                !application.ProfilingTypes.Contains(BasicProfilerUid))
            {
                return;
            }
            _integration = new ContextMenuIntegrationCollection(application);
            _integration.Register<AssemblyInfo, AssemblyMenuIntegration>();
            _integration.Register<ClassInfo, ClassMenuIntegration>();
            _integration.Register<FunctionInfo, FunctionMenuIntegration>();
        }

        public void Dispose()
        {
            _integration.Dispose();
        }
    }
}
