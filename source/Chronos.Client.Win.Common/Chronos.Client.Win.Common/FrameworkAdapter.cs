using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.Common
{
    public class FrameworkAdapter : IFrameworkAdapter, ILayoutProvider
    {
        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }

        public void InitializeSettings(FrameworkSettings settings)
        {
            
        }
    }
}
