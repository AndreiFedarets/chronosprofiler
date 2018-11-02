using System;
using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.DotNet.FindReference
{
    public class ProductivityAdapter : IProductivityAdapter, IInitializable, ILayoutProvider, IServiceConsumer
    {
        private static readonly Guid EventTreeUid;
        private bool _initialized;

        static ProductivityAdapter()
        {
            EventTreeUid = new Guid("{B3352C62-FCAB-45CA-8EEB-EA296E8C3122}");
        }

        void IInitializable.Initialize(IChronosApplication applicationObject)
        {
            IProfilingApplication application = applicationObject as IProfilingApplication;
            if (application == null)
            {
                return;
            }
            if (!application.ProfilingTypes.Contains(EventTreeUid))
            {
                return;
            }
            _initialized = true;
        }

        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            if (!_initialized)
            {
                return string.Empty;
            }
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {

        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            
        }
    }
}
