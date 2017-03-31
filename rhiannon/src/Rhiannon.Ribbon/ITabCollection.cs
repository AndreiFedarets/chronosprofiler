using System.Collections.Generic;

namespace Rhiannon.Ribbon
{
	public interface ITabCollection :IEnumerable<ITab>
	{
		void Invalidate();
	}
}
