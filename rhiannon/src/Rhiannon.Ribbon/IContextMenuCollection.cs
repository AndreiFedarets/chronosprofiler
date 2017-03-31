using System.Collections.Generic;

namespace Rhiannon.Ribbon
{
	public interface IContextMenuCollection : IEnumerable<IContextMenu>
	{
		void Invalidate();
	}
}
