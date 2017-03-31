namespace Rhiannon.Ribbon.Controls
{
	internal static class RibbonControlFactory
	{
		public static RibbonButton CreateButton(IButton buttonObject)
		{
			RibbonButton button = new RibbonButton();
			button.ControlObject = buttonObject;
			return button;
		}

		public static RibbonTab CreateTab(ITab tabObject)
		{
			RibbonTab tab = new RibbonTab();
			tab.ControlObject = tabObject;
			return tab;
		}

		public static RibbonGroup CreateGroup(IGroup groupObject)
		{
			RibbonGroup group = new RibbonGroup();
			group.ControlObject = groupObject;
			return group;
		}

		internal static RibbonControl CreateControl(IControl controlObject)
		{
			RibbonControl control = null;
			if (controlObject is IButton)
			{
				control = CreateButton((IButton)controlObject);
			}
			if (controlObject is IGroup)
			{
				control = CreateGroup((IGroup)controlObject);
			}
			if (controlObject is ITab)
			{
				control = CreateTab((ITab)controlObject);
			}
			return control;
		}
	}
}
