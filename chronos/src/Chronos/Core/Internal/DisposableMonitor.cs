using System;
using System.Threading;

namespace Chronos.Core.Internal
{
	internal class DisposableMonitor : MarshalByRefObject, IDisposable
	{
		private readonly object _syncObject;

		public DisposableMonitor(object syncObject)
		{
			_syncObject = syncObject;
			Monitor.Enter(_syncObject);
		}

		public void Dispose()
		{
			Monitor.Exit(_syncObject);
		}
	}
}
