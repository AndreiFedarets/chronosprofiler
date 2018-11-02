using Adenium;
using Adenium.Layouting;
using Chronos.DotNet.ExceptionMonitor;

namespace Chronos.Client.Win.DotNet.ExceptionMonitor
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, ILayoutProvider, IServiceConsumer
    {
        private IExceptionCollection _exceptions;

        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
            container.RegisterInstance(_exceptions);
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _exceptions = container.Resolve<IExceptionCollection>();
        }
    }
}
