using Adenium.Layouting;
using Chronos.Client.Win.Common.Properties;

namespace Chronos.Client.Win.Common.Menu
{
    internal sealed class CommonMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.CommonMenuItem_Text;
        }
    }
}
