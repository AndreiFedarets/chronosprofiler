using System.Windows;

namespace Rhiannon.Ribbon.Controls
{
	public class RibbonGroup : RibbonControl
	{
		static RibbonGroup()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonGroup), new FrameworkPropertyMetadata(typeof(RibbonGroup)));
		}
	}
}
