using Chronos.Core;

namespace Chronos.Client
{
	public interface IEventNameFormatter
	{
		string FormatName(IEvent @event);

        string FormatName(byte[] eventData);
    }
}
