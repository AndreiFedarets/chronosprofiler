using Chronos.Core;

namespace Chronos.Client
{
	public class EventsComparer : IEventsComparer
	{
		public int Compare(IEvent x, IEvent y)
		{
			if (x.Time > y.Time)
			{
				return -1;
			}
			if (x.Time == y.Time)
			{
				return 0;
			}
			return 1;
		}
	}
}
