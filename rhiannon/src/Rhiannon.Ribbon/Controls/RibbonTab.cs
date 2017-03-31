using System.Windows;

namespace Rhiannon.Ribbon.Controls
{
	public class RibbonTab : RibbonControl
	{
		static RibbonTab()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonTab), new FrameworkPropertyMetadata(typeof(RibbonTab)));
		}
	}
}
