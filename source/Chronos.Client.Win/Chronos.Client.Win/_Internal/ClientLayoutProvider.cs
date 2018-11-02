using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win
{
    internal sealed class ClientLayoutProvider : ILayoutProvider
    {
        public void ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
        }

        public string GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }
    }
}
