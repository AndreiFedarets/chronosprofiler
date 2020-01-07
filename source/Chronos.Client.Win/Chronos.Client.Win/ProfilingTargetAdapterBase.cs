using Layex.Layouts;
using Layex.ViewModels;

namespace Chronos.Client.Win
{
    public abstract class ProfilingTargetAdapterBase : IProfilingTargetAdapter, ILayoutProvider
    {
        private readonly ILayoutProvider _layoutProvider;

        protected ProfilingTargetAdapterBase()
        {
            string baseDirectory = GetType().Assembly.GetAssemblyPath();
            _layoutProvider = new FileSystemLayoutProvider(baseDirectory);
        }

        Layout ILayoutProvider.GetLayout(IViewModel viewModel)
        {
            return _layoutProvider.GetLayout(viewModel);
        }
    }
}
