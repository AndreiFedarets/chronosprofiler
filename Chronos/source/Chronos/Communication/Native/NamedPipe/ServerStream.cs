using System;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Chronos.Communication.Native.NamedPipe
{
    public class ServerStream : IServerStream
    {
        private readonly NamedPipeServerStream _stream;
        private bool _disposed;

        public ServerStream(string pipeName, PipeDirection direction)
        {
            PipeSecurity security = GetLowLevelPipeSecurity();
            _stream = new NamedPipeServerStream(pipeName, direction, 1, PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous, 3 * 1024 * 1024, 4096, security);
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

        public bool IsDisposed
        {
            get { return _disposed; }
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
            if (_stream.IsConnected)
            {
                _stream.Disconnect();
                _stream.BeginWaitForConnection(BeginProcessRequest, null);
            }
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

        private static PipeSecurity GetLowLevelPipeSecurity()
        {
            PipeSecurity pipeSecurity = new PipeSecurity();
            PipeAccessRule aceClients = new PipeAccessRule(
                new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null),
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
