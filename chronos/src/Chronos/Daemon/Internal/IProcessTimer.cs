using System;

namespace Chronos.Daemon.Internal
{
	internal interface IProcessTimer : IDisposable
	{
		uint GetTime();
	}
}
