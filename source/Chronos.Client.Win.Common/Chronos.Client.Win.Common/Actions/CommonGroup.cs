using Chronos.Client.Win.Common.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.Common.Actions
{
    internal sealed class CommonGroup : ActionGroup
    {
        public override string DisplayName
        {
            get { return Resources.CommonMenuItem_Text; }
        }
    }
}
