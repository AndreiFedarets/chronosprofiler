using Chronos.Client.Win.DotNet.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.DotNet.Actions
{
    public sealed class DotNetGroup : ActionGroup
    {
        public override string DisplayName
        {
            get { return Resources.DotNetMenuItem_Text; }
        }
    }
}
