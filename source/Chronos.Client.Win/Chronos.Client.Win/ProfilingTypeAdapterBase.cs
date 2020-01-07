using Layex.Layouts;
using Layex.ViewModels;

namespace Chronos.Client.Win
{
    public abstract class ProfilingTypeAdapterBase : IProfilingTypeAdapter, ILayoutProvider
    {
        private readonly ILayoutProvider _layoutProvider;

        protected ProfilingTypeAdapterBase()
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
