using System;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Chronos.Communication.NamedPipe
{
	public class NamedPipeServerStreamController : IServerStreamController
	{
		private readonly NamedPipeServerStream _stream;
		private bool _disposed;

		public NamedPipeServerStreamController(string pipeName)
		{
			PipeSecurity security = GetLowLevelPipeSecurity();
			_stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 0, 0, security);
			_stream.BeginWaitForConnection(BeginProcessRequest, null);
		}

		public Stream Stream
		{
			get { return _stream; }
		}

		public bool IsConnected
		{
			get { return _stream.IsConnected; }
		}

		public event ProcessRequestHandler ProcessRequest;

		private void BeginProcessRequest(IAsyncResult asyncResult)
		{
			try
			{
				_stream.EndWaitForConnection(asyncResult);
			}
			catch
			{
				return;
			}
			ProcessRequestHandler handler = ProcessRequest;
			if (handler != null)
			{
				handler(_stream);
			}
			_stream.Disconnect();
			_stream.BeginWaitForConnection(BeginProcessRequest, null);
		}

		public void Disconnect()
		{
			_stream.Disconnect();
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			if (_stream.IsConnected)
			{
				_stream.Disconnect();
			}
			_stream.Close();
			_stream.Dispose();
		}


		public static PipeSecurity GetLowLevelPipeSecurity()
		{
			PipeSecurity pipeSecurity = new PipeSecurity();
			PipeAccessRule aceClients = new PipeAccessRule(
				new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), // or some other group defining the allowed clients
				PipeAccessRights.ReadWrite,
				AccessControlType.Allow);
			PipeAccessRule aceOwner = new PipeAccessRule(
				WindowsIdentity.GetCurrent().Owner,
				PipeAccessRights.FullControl,
				AccessControlType.Allow);
			pipeSecurity.AddAccessRule(aceClients);
			pipeSecurity.AddAccessRule(aceOwner);
			return pipeSecurity;
		}
	}
}
