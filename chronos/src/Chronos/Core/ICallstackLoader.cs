using System.Collections.Generic;

namespace Chronos.Core
{
	public interface ICallstackLoader
	{
		byte[] LoadCallstacks(IList<CallstackInfo> callstackInfos);
	}
}
