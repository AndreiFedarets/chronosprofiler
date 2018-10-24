using Chronos.Client.Win.Controls.Common.EventsTree;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal interface IEventSearchAdapter
    {
        string SearchText { get; }

        bool Match(EventTreeItem item);
    }
}
