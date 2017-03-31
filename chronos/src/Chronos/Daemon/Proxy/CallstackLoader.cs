using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class CallstackLoader : ICallstackLoader
	{
		private readonly ICallstackLoader _callstackLoader;

		public CallstackLoader(ICallstackLoader callstackLoader)
		{
			_callstackLoader = callstackLoader;
		}

		public byte[] LoadCallstacks(IList<CallstackInfo> callstackInfos)
		{
			foreach (CallstackInfo callstackInfo in callstackInfos)
			{
				callstackInfo.SetProcessShadow(null);
			}
			return _callstackLoader.LoadCallstacks(callstackInfos);
		}
	}
}
