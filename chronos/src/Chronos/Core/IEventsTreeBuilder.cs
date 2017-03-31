using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IEventsTreeBuilder
	{
		List<IEvent> BuildChildren(IEvent parent, byte[] data, int offset, uint parentTime, ThreadInfo threadInfo);

        List<IEvent> BuildChildren(IEvent parent, byte[] data, uint parentTime, ThreadInfo threadInfo);
	}
}
