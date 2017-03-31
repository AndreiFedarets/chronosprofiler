using System.Collections.Generic;

namespace Rhiannon.Ribbon
{
	public interface IContainerControl<T> : IControl, IEnumerable<T> where T : IControl
	{

	}
}
