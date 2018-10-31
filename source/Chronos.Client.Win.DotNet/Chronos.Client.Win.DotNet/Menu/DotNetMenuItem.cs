using Adenium.Layouting;
using Chronos.Client.Win.DotNet.Properties;

namespace Chronos.Client.Win.DotNet.Menu
{
    internal sealed class DotNetMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.DotNetMenuItem_Text;
        }
    }
}
