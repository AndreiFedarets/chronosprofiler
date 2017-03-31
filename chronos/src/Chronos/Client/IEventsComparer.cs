using Chronos.Core;

namespace Chronos.Client
{
	public interface IEventsComparer
	{
		int Compare(IEvent x, IEvent y);
	}
}
