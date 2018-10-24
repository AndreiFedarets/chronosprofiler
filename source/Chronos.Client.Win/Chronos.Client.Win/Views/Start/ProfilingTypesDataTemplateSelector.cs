using System.Windows;
using System.Windows.Controls;
using Adenium;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Views.Start
{
    public class ProfilingTypesDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            UserControl userControl = container.FindParent<UserControl>();
            if (item is FrameworkViewModel)
            {
                return (DataTemplate)userControl.Resources["FrameworkDataTemplate"];
            }
            if (item is ProfilingTypeViewModel)
            {
                return (DataTemplate)userControl.Resources["ProflingTypeDataTemplate"];
            }
            return base.SelectTemplate(item, container);
        }
    }
}
