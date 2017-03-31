using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IEvent
	{
		ThreadInfo ThreadInfo { get; }

		uint Hits { get; }

		uint Time { get; }

		long StackFullTime { get; }

		EventType EventType { get; }

		bool HasChildren { get; }

		uint Unit { get; }

		double Percent { get; }

		IEnumerable<IEvent> Children { get; }

        IEvent Parent { get; }

	}
}
