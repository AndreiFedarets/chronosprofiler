using Adenium.Menu;
using Chronos.Client.Win.DotNet.Properties;

namespace Chronos.Client.Win.Menu.DotNet
{
    internal sealed class DotNetMenuItem : MenuItem
    {
        public override string Text
        {
            get { return Resources.DotNetMenuItem_Text; }
            protected set { }
        }
    }
}
