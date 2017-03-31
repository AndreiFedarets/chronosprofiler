using System.Windows;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
	public class ClassInfoControl : UnitInfoControl<ClassInfo>
    {
		static ClassInfoControl()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ClassInfoControl), new FrameworkPropertyMetadata(typeof(ClassInfoControl)));
        }
    }
}
