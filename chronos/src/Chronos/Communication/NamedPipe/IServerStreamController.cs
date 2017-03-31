using System;
using System.IO;

namespace Chronos.Communication.NamedPipe
{
	public delegate void ProcessRequestHandler(Stream stream);

	public interface IServerStreamController : IDisposable
	{
		bool IsConnected { get; }

		event ProcessRequestHandler ProcessRequest;

		void Disconnect();
	}
}
