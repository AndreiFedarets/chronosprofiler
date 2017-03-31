using System.Windows;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
    public class ModuleInfoControl : UnitInfoControl<ModuleInfo>
    {
		static ModuleInfoControl()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ModuleInfoControl), new FrameworkPropertyMetadata(typeof(ModuleInfoControl)));
        }
    }
}
