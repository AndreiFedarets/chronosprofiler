using System.Windows;
using System.Windows.Controls;
using Chronos.Core;

namespace Chronos.Client.Win.Views.References
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
