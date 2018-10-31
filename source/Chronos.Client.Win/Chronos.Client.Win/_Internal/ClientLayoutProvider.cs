using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win
{
    internal sealed class ClientLayoutProvider : ILayoutProvider
    {
        public void ConfigureContainer(IContainer container)
        {
        }

        public string GetLayout(IViewModel viewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(viewModel);
        }
    }
}
