using System;

namespace Rhiannon.Ribbon
{
	public interface IRibbon
	{
		ITabCollection Tabs { get; }

		IContextMenuCollection ContextMenus { get; }

		void Invalidate();

		event EventHandler BeforeInvalidate;

		event EventHandler AfterInvalidate;
	}
}
