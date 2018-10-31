using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.Common
{
    public class FrameworkAdapter : IFrameworkAdapter, ILayoutProvider
    {
        void ILayoutProvider.ConfigureContainer(IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel viewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(viewModel);
        }
    }
}
