using System.Windows;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
    public class AssemblyInfoControl : UnitInfoControl<AssemblyInfo>
    {
		static AssemblyInfoControl()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AssemblyInfoControl), new FrameworkPropertyMetadata(typeof(AssemblyInfoControl)));
        }
    }
}
