using System.Windows;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
    public class AppDomainInfoControl : UnitInfoControl<AppDomainInfo>
    {
        static AppDomainInfoControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppDomainInfoControl), new FrameworkPropertyMetadata(typeof(AppDomainInfoControl)));
        }
    }
}
