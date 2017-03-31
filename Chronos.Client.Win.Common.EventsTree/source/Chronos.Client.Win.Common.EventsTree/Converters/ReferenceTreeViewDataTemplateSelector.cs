using System.Windows;
using System.Windows.Controls;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Converters.Common.EventsTree
{
    public class ReferenceTreeViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UnitDataTemplate { get; set; }

        public DataTemplate EventDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Reference<IEvent>)
            {
                return EventDataTemplate;
            }
            return UnitDataTemplate;
        }
    }
}
